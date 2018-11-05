using System;
using System.Collections.Generic;
using OblPR.Data.Entities;

namespace OblPR.Data.Services
{
    public interface IGameMatchManager
    {
        void AddMatch(GameMatch match);
        List<Tuple<Player, int>> GetRanking();
        List<GameMatch> GetStatistics();
    }
}