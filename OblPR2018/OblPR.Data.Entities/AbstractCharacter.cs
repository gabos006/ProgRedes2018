using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Data.Entities
{
    public abstract class AbstractCharacter
    {
        public Player CurentPlayer {get; private set; }

        public AbstractCharacter(Player player)
        {
            CurentPlayer = player;
        }

        public abstract int Health { get; set; }
        public abstract int AP { get;  }
    }
}
