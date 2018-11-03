using System;
using System.Collections;
using System.Collections.Generic;
using OblPR.Data.Entities;

namespace OblPR.Game
{
    public class GameMatchManager : IGameMatchManager
    {
        private readonly Stack<GameMatch> _matches;

        public GameMatchManager()
        {
            this._matches = new Stack<GameMatch>();
        }

        public void Add(GameMatch match)
        {
            this._matches.Push(match);
        }
    }
}