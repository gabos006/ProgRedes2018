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

            Console.Clear();
            Console.CursorVisible = false;

            while (_isRunning)
            {
                Console.SetCursorPosition(0, 0);
                for (var i = 0; i < GameConstants.BOARD_SIZE; i++)
                {
                    string line = "";

                    for (var j = 0; j < GameConstants.BOARD_SIZE; j++)
                    {
                        var cell = _board[j][i];
                        if (cell == null)
                            line += " X ";
                        if (cell != null)
                        {
                            if (cell.Char.CharacterRole == Role.Monster)
                                line += " M ";
                            if (cell.Char.CharacterRole == Role.Survivor)
                                line += " S ";
                        }
                    }
                    Console.WriteLine("");

                    Console.WriteLine(line);
                }
            }
            Console.CursorVisible = true;
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

            var auxiliarList = _activePlayers.ToList();

            if (survivors == 0 && monsters == 0)
            {
                //no gana nadie
                return;
            }

            if (survivors == 0 && monsters > 0)
            {
                foreach (var handler in auxiliarList)
                {
                    PlayerExit(handler, "monsters win");
                }
                return;
            }


            if (survivors > 0)
            {

                foreach (var handler in auxiliarList)
                {
                    PlayerExit(handler, "surivors win");
                }

            }

        }

        public void Attack(ICharacterHandler characterHandler)
        {
            lock (_actionLock)
            {
                var handler = (CharacterHandler)characterHandler;
                AttackNearbyPlayers(handler.Position, handler.Char);
            }
        }

        public void PlayerExit(ICharacterHandler characterHandler, string reason)
        {
            lock (_actionLock)
            {
                var handler = (CharacterHandler)characterHandler;
                _board[handler.Position.X][handler.Position.Y] = null;
                _activePlayers.Remove(handler);
                _deadPlayers.Add(handler.Char.CurentPlayer);
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
                if (!CellEmpty(p))
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

        private bool CellEmpty(Point p)
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
            var x = charHandler.Position.X;
            var y = charHandler.Position.Y;

            charHandler.Position.X = p.X;
            charHandler.Position.Y = p.Y;

            _board[p.X][p.Y] = charHandler;

            if (x >= 0 && y >= 0)
                _board[x][y] = null;
            NotifyPlayerNear(p, charHandler.Char);

        }

        private void NotifyPlayerNear(Point p, Character character)
        {
            var pos = new int[] { -1, 0, 1 };
            foreach (var i in pos)
            {
                foreach (var j in pos)
                {
                    if (i == 0 && j == 0)
                        continue;

                    var point = new Point(p.X + i, p.Y + j);
                    if (!IsValidPoint(point))
                        continue;
                    ;
                    if (CellEmpty(point))
                        continue;
                    _board[point.X][point.Y].Handler.NotifyPlayerNear(character.CharacterRole.ToString() + " near");
                }
            }
        }

        private bool IsValidPoint(Point point)
        {
            return (point.X >= 0 && point.X < GameConstants.BOARD_SIZE) &&
                   (point.Y >= 0 && point.Y < GameConstants.BOARD_SIZE);
        }


        private void AttackNearbyPlayers(Point p, Character attacker)
        {
            var pos = new int[] { -1,0,1 };
            foreach (var i in pos)
            {
                foreach (var j in pos)
                {
                    if(i==0 && j ==0)
                        continue;

                    var point = new Point(p.X + i, p.Y + j);
                    
                    if (!IsValidPoint(point))
                        continue;
                    ;
                    if (CellEmpty(point))
                        continue;

                    var handler = _board[point.X][point.Y];

                    attacker.Attack(handler.Char);

                    if (!handler.IsCharacterDead()) continue;
                    PlayerExit(handler, "You died");
                }
            }
        }

        private Point GetEmptyCell()
        {
            var x = 0;
            var y = 0;
            while (x < GameConstants.BOARD_SIZE)
            {
                while (y < GameConstants.BOARD_SIZE)
                {
                    var point = new Point(x, y);
                    if (CellEmpty(point))
                    {
                        return point;
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
