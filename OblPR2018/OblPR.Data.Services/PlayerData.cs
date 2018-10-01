using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OblPR.Data.Entities;

namespace OblPR.Data.Services
{
    public class PlayerData
    {
        public PlayerData()
        {
            ActivePlayers = new List<Player>();
            RegisteredPlayers = new List<Player>();

            RegisteredPlayers.Add(new Player("Dario", null));
            RegisteredPlayers.Add(new Player("Gabriel", null));

        }

        public List<Player> ActivePlayers { get; private set; }
        public List<Player> RegisteredPlayers { get; private set; }

    }
}
