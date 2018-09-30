using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OblPR.Data.Entities;
using OblPR.Game;
using OblPR.Protocol;

namespace OblPR.Game
{
    public class GameLogic : IGameLogic, IControlsProvider
    {
        private bool _isRunning = false;

        private Timer _timer;
        private CharacterHandler[][] _board;
        private readonly List<Player> _deadPlayers;
        private readonly List<CharacterHandler> _activePlayers;

        private readonly object _actionLock = new object();

        public GameLogic()
        {
            _deadPlayers = new List<Player>();
            _activePlayers = new List<CharacterHandler>();
            InitGameBoard();
        }

        private void InitGameBoard()
        {
            _board = new CharacterHandler[GameConstants.BOARD_SIZE][];
            for (var i = 0; i < GameConstants.BOARD_SIZE; i++)
            {
                _board[i] = new CharacterHandler[GameConstants.BOARD_SIZE];
            }
        }

        public void Start()
        {
            InitGameBoard();
            _deadPlayers.Clear();
            _activePlayers.Clear();
            _isRunning = true;
            StartTimer();
            while (_isRunning) { }
        }

        private void StartTimer()
        {
            _timer = new System.Threading.Timer(
                this.TimerEnd, null, GameConstants.MILISECONDS, GameConstants.MILISECONDS);
        }

        private void TimerEnd(object state)
        {
            _isRunning = false;
            _timer.Dispose();
            EndMatch();

        }

        private void EndMatch()
        {
            var survivors = _activePlayers.Count(x => x.Char.CharacterRole.Equals(Role.Survivor));
            var monsters = _activePlayers.Count(x => x.Char.CharacterRole.Equals(Role.Monster));

            if (survivors == 0 && monsters == 0)
            {
                //no gana nadie
                return;
            }

            if (survivors == 0 && monsters > 0)
            {
                foreach (var handler in _activePlayers)
                {
                    PlayerExit(handler, "monsters win");
                }
                return;
            }


            if (survivors > 0)
            {
                var handlers = _activePlayers.ToList().GetEnumerator();

                while (handlers.MoveNext())
                {
                    PlayerExit(handlers.Current, "surivors win");

                }
                return;
            }

        }

        public void Attack(ICharacterHandler characterHandler)
        {
            lock (_actionLock)
            {
                var handler = (CharacterHandler)characterHandler;
                var ap = handler.Char.Ap;
                AttackNearbyPlayers(handler.Position, handler.Char.Ap);
            }
        }

        public void PlayerExit(ICharacterHandler characterHandler, string reason)
        {
            lock (_actionLock)
            {
                var handler = (CharacterHandler)characterHandler;
                _board[handler.Position.X][handler.Position.Y] = null;
                _activePlayers.Remove(handler);
                handler.Handler.NotifyMatchEnd(reason);
            }
        }


        public void MovePlayer(ICharacterHandler characterHandler, Point p)
        {
            lock (_actionLock)
            {
                var handler = (CharacterHandler)characterHandler;
                if (!IsValidMove(new Point(handler.Position.X, handler.Position.Y), p))
                    throw new InvalidMoveException("Invalid move");
                if (!CellAvailable(p))
                    throw new InvalidMoveException("Invalid move");

                MovePlayerToCell(handler, p);
                NotifyPlayerNear(p, handler.Char);
            }
        }

        private bool IsValidMove(Point orig, Point dest)
        {
            var despX = Math.Abs(orig.X - dest.X);
            var despY = Math.Abs(orig.Y - dest.Y);
            return orig.X >= 0 && orig.X < GameConstants.BOARD_SIZE && orig.Y >= 0 && orig.Y < GameConstants.BOARD_SIZE && despY + despX <= 2;

        }

        private bool CellAvailable(Point p)
        {
            return _board[p.X][p.Y] == null;
        }

        public ICharacterHandler JoinGame(IClientHandler handler, Character character)
        {
            lock (_actionLock)
            {
                if (!_isRunning)
                    throw new InvalidMatchException("Invalid match");
                if (_activePlayers.Count >= GameConstants.MAX_PLAYERS)
                    throw new InvalidPlayerException("Max players reached");
                if (IsDeadPlayer(character))
                    throw new InvalidPlayerException("Invalid player");

                var p = GetEmptyCell();


                var charHandler = new CharacterHandler(this, handler, character);
                _activePlayers.Add(charHandler);

                MovePlayerToCell(charHandler, p);


                return charHandler;
            }

        }

        private void MovePlayerToCell(CharacterHandler charHandler, Point p)
        {
            _board[p.X][p.Y] = charHandler;
            charHandler.Position.X = p.X;
            charHandler.Position.Y = p.Y;
            NotifyPlayerNear(p, charHandler.Char);
        }

        private void NotifyPlayerNear(Point p, Character character)
        {
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 || j == 0) continue;
                    if (p.X + i >= 0 && p.X + i < GameConstants.BOARD_SIZE && p.Y + j >= 0 && p.Y + j < GameConstants.BOARD_SIZE)
                    {
                        if (_board[p.X + i][p.Y + j] != null)
                            _board[p.X + i][p.Y + j].Handler.NotifyPlayerNear(character.CharacterRole.ToString() + " near");
                    }
                }

            }
        }

        private void AttackNearbyPlayers(Point p, int ap)
        {
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 || j == 0) continue;
                    if (p.X + i >= 0 && p.X + i < GameConstants.BOARD_SIZE && p.Y + j >= 0 && p.Y + j < GameConstants.BOARD_SIZE)
                    {
                        var handler = _board[p.X + i][p.Y + j];
                        if (handler != null)
                        {
                            handler.Char.Health -= ap;
                            if (handler.IsCharacterDead())
                            {
                                _deadPlayers.Add(handler.Char.CurentPlayer);
                                PlayerExit(handler, "You died");
                            }
                        }
                    }
                }

            }
        }


        private Point GetEmptyCell()
        {
            int x = 0;
            int y = 0;
            while (x < GameConstants.BOARD_SIZE)
            {
                while (y < GameConstants.BOARD_SIZE)
                {
                    if (_board[x][y] == null)
                    {
                        return new Point(x, y);
                    }
                    y++;
                }
                x++;
            }
            throw new NoCellAvailableException("Max Players Reached");
        }


        private bool IsDeadPlayer(Character character)
        {
            return _deadPlayers.Any(x => x.Nick.Equals(character.CurentPlayer.Nick));

        }

    }
}
