using System.Collections.Generic;
using OblPR.Data.Entities;

namespace OblPR.Data.DataAccess
{
    public interface IPlayerRepository
    {
        void AddPlayer(Player player);
        IEnumerator<Player> GetPlayers();
        bool PlayerExists(Player player);
    }
}