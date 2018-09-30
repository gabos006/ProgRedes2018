using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OblPR.Data.Entities;
using OblPR.Game;

namespace OblPR.Game
{
    public class CharacterHandler : ICharacterHandler
    {
        public Point Position { get; }
        public Character Char { get; }
        public IClientHandler Handler { get; }




        private readonly IGameLogic _logic;

        public CharacterHandler(GameLogic gameLogic, IClientHandler handler, Character character)
        {
            this.Char = character;
            this.Handler = handler;
            this.Position = new Point(0,0);
            this._logic = gameLogic;
        }

        public void Attack()
        {
            _logic.Attack(this);
        }

        public void ExitMatch()
        {
            _logic.PlayerExit(this, "Player exited");
        }

        public void Move(Point pos)
        {
            _logic.MovePlayer(this, pos);
        }

        public bool IsCharacterDead()
        {
            return Char.Health <= 0;
        }

    }
}
