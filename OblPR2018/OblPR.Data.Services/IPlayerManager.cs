using System.Collections.Generic;
using OblPR.Data.Entities;

namespace OblPR.Data.Services
{
    public interface IPlayerManager
    {
        void AddPlayer(Player player);
        List<Player> GetAllRegisteredPlayers();
        List<Player> GetAllActivePlayers();
        void DeletePlayer(string playerName);
        void UpdatePlayer(Player player);

    }
}