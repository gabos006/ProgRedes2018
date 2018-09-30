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
        public Point Position { get; set; }

        public int Health
        {
            get { return _character.Health; }
            set { _character.Health = value; }
        }


        public int Ap { get { return _character.AP; } }

        public Player Player
        {
            get { return _character.CurentPlayer; }
        }

        public IClientNotifier Notifier { get; private set; }


        private readonly IGameController _controller;
        private readonly AbstractCharacter _character;

        public CharacterHandler(GameController gameController, IClientNotifier notifier, AbstractCharacter character)
        {
            this.Notifier = notifier;
            this._controller = gameController;
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

        public void Move(Point pos)
        {
            _controller.MovePlayer(this, pos);
        }

        public bool IsCharacterDead()
        {
            return Health <= 0;
        }





    }
}
