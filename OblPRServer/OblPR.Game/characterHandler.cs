using OblPR.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Game
{
    public class CharacterHandler : ICharacterHandler
    {
        private int _x;
        private int _y;

        private IGameController _controller;
        private IClientNotifier _notifier;


        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void ExitMatch()
        {
            throw new NotImplementedException();
        }

        public void Move(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
