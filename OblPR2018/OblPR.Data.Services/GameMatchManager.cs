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

        public List<Tuple<Player, int>> GetRanking()
        {
            var results = _matches.SelectMany(x => x.Results.Select(y => new {A = y.Item1, Points = y.Item2})).ToList();

            var query = from element in results
                group element by element.A
                into g
                select new
                {
                    B = g.Key.CurentPlayer,
                    Sum = g.Sum(u => u.Points),
                };


            var ranking = from res in query.AsEnumerable()
                select Tuple.Create(res.B, res.Sum);

            return ranking.ToList();
        }
    }
}