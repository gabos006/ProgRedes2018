using OblPR.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OblPR.WebService
{
    public class GetStatitsticsModel
    {
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public List<MatchResultModel> Results { get; private set; }

        public GetStatitsticsModel(GameMatch game)
        {
            this.Id = game.Id;
            this.Date = game.Date;
            this.Results =
                game.Results.Select(x => new MatchResultModel(x.Character.CurentPlayer.Nick, 
                x.Character.CharacterRole.ToString(),
                x.Points)).ToList();

        }
    }
}