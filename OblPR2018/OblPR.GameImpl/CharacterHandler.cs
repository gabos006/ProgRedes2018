using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OblPR.Data.Entities;
using OblPR.Game;

namespace OblPR.GameImpl
{
    public class CharacterHandler : ICharacterHandler
    {
        public Point Position { get; }
        public Character Char { get; }
        public IClientNotifier Notifier { get; }




        private readonly IGameController _controller;

        public CharacterHandler(GameController gameController, IClientNotifier notifier, Character character)
        {
            this.Char = character;
            this.Notifier = notifier;
            this._controller = gameController;
        }

        public void Attack()
        {
            _controller.Attack(this);
        }

        public void ExitMatch()
        {
            _controller.PlayerExit(this, "Player exited");
        }

        public void Move(Point pos)
        {
            _controller.MovePlayer(this, pos);
        }

        public bool IsCharacterDead()
        {
            return Char.Health <= 0;
        }

    }
}
