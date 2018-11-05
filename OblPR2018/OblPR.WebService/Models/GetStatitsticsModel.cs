using OblPR.Data.Entities;
using System;
using System.Collections.Generic;

namespace OblPR.WebService
{
    public class GetStatitsticsModel
    {
        public Guid Id;
        public DateTime Date;
        public List<Tuple<Character, int>> Results;

        public GetStatitsticsModel(Guid id, DateTime date, List<Tuple<Character, int>> results)
        {
            this.Id = id;
            this.Date = date;
            this.Results = results;
        }
    }
}