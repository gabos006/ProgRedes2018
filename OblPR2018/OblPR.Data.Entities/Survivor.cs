using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Data.Entities
{
    public class Survivor : AbstractCharacter
    {
        public Survivor(Player player) : base(player)
        {
            Health = 20;
            AP = 5;
        }

        public override int Health { get; set; }
        public override int AP { get; }
    }
}
