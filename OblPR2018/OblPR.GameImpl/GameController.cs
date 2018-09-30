﻿using System;
using System.Collections.Generic;
using System.Linq;
using OblPR.Data.Entities;
using OblPR.Game;
using OblPR.GameImpl;


namespace OblPR.GameImp
{
    public class GameController : IGameController, IGameServer
    {
        private CharacterHandler[][] _board;
        private readonly List<Player> _deadPlayers;

        private readonly object _actionLock = new object();
        private int _activePlayers = 0;
        private bool _isRunning = false;

        public GameController()
        {
            _deadPlayers = new List<Player>();
            InitGameBoard();
        }

        private void InitGameBoard()
        {
            _board = new CharacterHandler[Constants.BOARD_SIZE][];
            for (var i = 0; i < Constants.BOARD_SIZE; i++)
            {
                _board[i] = new CharacterHandler[Constants.BOARD_SIZE];
            }
        }

        public void Reset()
        {
            InitGameBoard();
            _deadPlayers.Clear();
            _isRunning = true;
        }

        public void Start()
        {
            _isRunning = true;
        }

        public void Attack(ICharacterHandler characterHandler)
        {
            lock (_actionLock)
            {
                var handler = (CharacterHandler)characterHandler;
                var ap = handler.Ap;
                AttackNearbyPlayers(handler.Position, handler.Ap);
            }
        }

        public void PlayerExit(ICharacterHandler characterHandler)
        {
            lock (_actionLock)
            {
                var handler = (CharacterHandler)characterHandler;
                _board[handler.Position.X][handler.Position.Y] = null;
                handler.ExitMatch();
            }
        }


        public void MovePlayer(ICharacterHandler characterHandler, Point p)
        {
            lock (_actionLock)
            {
                var handler = (CharacterHandler)characterHandler;
                if (!IsValidMove(new Point(handler.Position.X, handler.Position.Y), p))
                    throw new InvalidMoveException();
                if (!CellAvailable(p))
                    throw new InvalidMoveException();

                MovePlayerToCell(handler, p);
                NotifyPlayerNear(p);
            }
        }

        private bool IsValidMove(Point orig, Point dest)
        {
            var despX = Math.Abs(orig.X - dest.X);
            var despY = Math.Abs(orig.Y - dest.Y);
            return orig.X >= 0 && orig.X < Constants.BOARD_SIZE && orig.Y >= 0 && orig.Y < Constants.BOARD_SIZE && despY + despX <= 2;

        }

        private bool CellAvailable(Point p)
        {
            return _board[p.X][p.Y] == null;
        }

        public ICharacterHandler JoinGame(IClientNotifier notifier, AbstractCharacter character)
        {
            lock (_actionLock)
            {
                if (_activePlayers >= Constants.MAX_PLAYERS)
                    throw new InvalidPlayerException();
                if (IsDeadPlayer(character))
                    throw new InvalidPlayerException();

                var p = GetEmptyCell();


                var charHandler = new CharacterHandler(this, notifier, character);
                MovePlayerToCell(charHandler, p);

                _activePlayers++;

                NotifyPlayerNear(p);
                return charHandler;
            }

        }

        private void MovePlayerToCell(CharacterHandler charHandler, Point p)
        {
            _board[p.X][p.Y] = charHandler;
            charHandler.Position.X = p.X;
            charHandler.Position.Y = p.Y;
        }

        private void NotifyPlayerNear(Point p)
        {
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 || j == 0) continue;
                    if (p.X + i >= 0 && p.X + i < Constants.BOARD_SIZE && p.Y + i >= 0 && p.Y + i < Constants.BOARD_SIZE)
                    {
                        _board[p.X + i][p.Y + j].Notifier.NotifyPlayerNear();
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
                    if (p.X + i >= 0 && p.X + i < Constants.BOARD_SIZE && p.Y + i >= 0 && p.Y + i < Constants.BOARD_SIZE)
                    {
                        var handler = _board[p.X + i][p.Y + j];
                        if ( handler!= null)
                        {
                            handler.Health -= ap;
                            if (handler.IsCharacterDead())
                            {
                                _deadPlayers.Add(handler.Player);
                                PlayerExit(handler);
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
            while (x < Constants.BOARD_SIZE)
            {
                while (y < Constants.BOARD_SIZE)
                {
                    if (_board[x][y] == null)
                    {
                        return new Point(x, y);
                    }
                    y++;
                }
                x++;
            }
            throw new NoCellAvailableException();
        }


        private bool IsDeadPlayer(AbstractCharacter character)
        {
            return _deadPlayers.Any(x => x.Nick.Equals(character.CurentPlayer.Nick));
            ;
        }
    }
}
