﻿using System;
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




        private readonly IGameController _controller;

        public CharacterHandler(GameController gameController, IClientHandler handler, Character character)
        {
            this.Char = character;
            this.Handler = handler;
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
