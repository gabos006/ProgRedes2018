using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Data.Entities
{
    [Serializable]
    public class GameMatch
    {
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }

        public List<Result> Results { get; private set; }

        public GameMatch()
        {
            this.Id = Guid.NewGuid();
            this.Date = DateTime.Now;
            this.Results = new List<Result>();
        }

    }
}
