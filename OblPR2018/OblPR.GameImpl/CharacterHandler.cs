using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OblPR.Data.Entities;
using OblPR.Game;
using OblPR.GameImp;

namespace OblPR.GameImpl
{
    public class CharacterHandler : ICharacterHandler
    {
        public int CoordX { get; set; }
        public int CoordY { get; set; }

        private readonly IGameController _controller;
        private readonly IClientNotifier _notifier;
        private readonly AbstractCharacter _character;

        public CharacterHandler(GameController gameController, IClientNotifier notifier, AbstractCharacter character)
        {
            this._controller = gameController;
            this._notifier = notifier;
            this._character = character;
        }

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
