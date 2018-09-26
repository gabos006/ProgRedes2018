using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OblPR.Data.Entities;

namespace OblPR.Data.DataAccess
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly List<Player> _players;
        public PlayerRepository()
        {
            _players = new List<Player>();
            AddTestData();
        }

        private void AddTestData()
        {
            _players.Add(new Player("Dario"));
            _players.Add(new Player("Gabriel"));
        }

        public void AddPlayer(Player player)
        {

            if (PlayerExists(player))
                throw new PlayerExistsException();
            _players.Add(player);


        }

        public IEnumerator<Player> GetPlayers()
        {
            return _players.GetEnumerator();
        }

        public bool PlayerExists(Player player)
        {
            return _players.Any(x => x.Nick.Equals(player.Nick));
        }
    }
}
