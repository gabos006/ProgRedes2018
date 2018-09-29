using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Game
{
    public class GameController:IGameController
    {
        private CharacterHandler[][] board;
        public GameController()
        {
            InitGameBoard();
        }

        private void InitGameBoard()
        {
            board = new CharacterHandler[8][];
            for (int i = 0; i < 8; i++)
            {
                board[i] = new CharacterHandler[8];
            }
        }

        internal void Reset()
        {
            throw new NotImplementedException();
        }

        internal void Start()
        {
            throw new NotImplementedException();
        }

        public void Attack(ICharacterHandler characterHandler)
        {
            throw new NotImplementedException();
        }

        public void PlayerExit(ICharacterHandler characterHandler)
        {
            throw new NotImplementedException();
        }

        public void MovePlayer(ICharacterHandler characterHandler, int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
