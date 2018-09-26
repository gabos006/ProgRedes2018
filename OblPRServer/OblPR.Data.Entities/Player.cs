using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Data.Entities
{
    public class Player
    {
        public Player(string nickname)
        {
            Nick = nickname;
        }
        public string Nick { get; }
    }
}
