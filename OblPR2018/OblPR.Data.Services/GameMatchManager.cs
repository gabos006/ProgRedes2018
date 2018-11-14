using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OblPR.Data.Entities;

namespace OblPR.Data.Services

{
    public class GameMatchManager : MarshalByRefObject, IGameMatchManager
    {
        private readonly Stack<GameMatch> _matches;

        public GameMatchManager()
        {
            this._matches = new Stack<GameMatch>();
        }

        public void AddMatch(GameMatch match)
        {
            this._matches.Push(match);
        }

        public List<GameMatch> GetStatistics()
        {
            return _matches.OrderByDescending(x => x.Date).Take(10).ToList();
        }

        public List<PlayerScore> GetRanking()
        {
            var results = _matches.SelectMany(x => x.Results.Select(y => new {A = y.Character, Points = y.Points})).ToList();

            var query = (from element in results
                group element by element.A.CurentPlayer
                into g
                select new
                {
                    B = g.Key,
                    Sum = g.Sum(u => u.Points),
                }).OrderByDescending(x => x.Sum);


            var ranking = from res in query.AsEnumerable()
                select new PlayerScore(res.B, res.Sum);

            return ranking.ToList();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}