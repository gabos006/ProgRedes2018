using OblPR.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OblPR.WebService
{
    public class GetRankingModel
    {
        public string Nick { get; private set; }
        public int Points { get; private set; }

        public GetRankingModel(PlayerScore playerPoints)
        {
            Nick = playerPoints.Player.Nick;
            Points = playerPoints.Score;
        }
    }
}