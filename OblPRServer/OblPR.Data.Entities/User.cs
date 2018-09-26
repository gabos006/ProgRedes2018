using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Data.Entities
{
    public class User
    {
        public User(string nickname)
        {
            Nick = nickname;
        }
        public string Nick { get; }
    }
}
