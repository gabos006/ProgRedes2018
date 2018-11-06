using OblPR.Data.Entities;
using System;
using System.Collections.Generic;

namespace OblPR.WebService
{
    public class GetStatitsticsModel
    {
        public Guid Id;
        public DateTime Date;
        public List<MatchResultModel> Results;

        public GetStatitsticsModel(GameMatch game)
        {
            Id = game.Id;
            Date = game.Date;
            Results = new List<MatchResultModel>();
            foreach (var item in game.Results)
            {
                var character = item.Item1;
                var points = item.Item2;

                string gameResult = "LOSER";
                if (points > 0)
                {
                    gameResult = "WINNER";
                }

                var matchResult = new MatchResultModel(character.CurentPlayer.Nick, character.CharacterRole.ToString(), gameResult);
                Results.Add(matchResult);
            }
        }
    }
}