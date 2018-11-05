using OblPR.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OblPR.WebService
{
    public class GetRankingModel
    {
        public String Nick;
        public int Points;
        
        public GetRankingModel(Tuple<Player,int> playerPoints)
        {
            Nick = playerPoints.Item1.Nick;
            Points = playerPoints.Item2;
        }
    }
}