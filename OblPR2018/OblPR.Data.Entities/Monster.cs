using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Data.Entities
{
    public class Monster:AbstractCharacter
    {
        
        public Monster(Player player) : base(player)
        {
            Health = 100;
            AP = 10;
        }

        public override int Health { get; set; }
        public override int AP { get; }
    }
}
