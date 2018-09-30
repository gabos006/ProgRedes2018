using System;
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
        private List<Player> _deadPlayers;

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
            for (int i = 0; i < Constants.BOARD_SIZE; i++)
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
                throw new NotImplementedException();
            }
        }

        public void PlayerExit(ICharacterHandler characterHandler)
        {
            lock (_actionLock)
            {
                throw new NotImplementedException();
            }
        }

        public void MovePlayer(ICharacterHandler characterHandler, int x, int y)
        {
            lock (_actionLock)
            {
                var handler = (CharacterHandler) characterHandler;
            }
        }

        public ICharacterHandler JoinGame(IClientNotifier notifier, AbstractCharacter character)
        {
            lock (_actionLock)
            {
                if (_activePlayers >= Constants.MAX_PLAYERS)
                    throw new InvalidPlayerException();
                if (IsDeadPlayer(character))
                    throw new InvalidPlayerException();

                Point p = GetEmptyCell();


                var charHandler = new CharacterHandler(this, notifier, character);
                charHandler.CoordX = p.X;
                charHandler.CoordY = p.Y;

                _activePlayers++;
                NotifyPlayerNear(charHandler);
                return charHandler;
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

        private void NotifyPlayerNear(CharacterHandler charHandler)
        {
            throw new NotImplementedException();
        }

        private bool IsDeadPlayer(AbstractCharacter character)
        {
            return _deadPlayers.Any(x => x.Nick.Equals(character.CurentPlayer.Nick));
            ;
        }
    }
}
