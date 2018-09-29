using System;
using System.Collections.Generic;
using OblPR.Data.Entities;
using OblPR.Game;
using OblPR.GameImpl;


namespace OblPR.GameImp
{
    public class GameController:IGameController, IGameServer
    {
        private CharacterHandler[][] _board;
        private List<Player> _deadPlayers;
        private readonly object _actionLock = new object();


        public GameController()
        {
            _deadPlayers = new List<Player>();
            InitGameBoard();
        }

        private void InitGameBoard()
        {
            _board = new CharacterHandler[8][];
            for (int i = 0; i < 8; i++)
            {
                _board[i] = new CharacterHandler[8];
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
        }

        public ICharacterHandler JoinGame(IClientNotifier notifier, AbstractCharacter character)
        {
            lock (_actionLock)
            {

                var charHandler = new CharacterHandler(this, notifier, character);
                return null;
            }

        }
    }
}
