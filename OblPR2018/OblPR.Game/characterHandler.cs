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
        public int CoordX { get; set; }
        public int CoordY { get; set; }

        private IGameController _controller;
        private IClientNotifier _notifier;


        public void Attack()
        {
            _controller.Attack(this);
        }

        public void ExitMatch()
        {
            _controller.PlayerExit(this);
        }

        public void Move(int x, int y)
        {
            _controller.MovePlayer(this, x, y);
        }
    }
}
