using System;
using System.Collections.Generic;
using System.Linq;
using OblPR.Data.Entities;

namespace OblPR.Data.Services
{
    public class PlayerManager : MarshalByRefObject, IPlayerManager
    {
        private static readonly object Locker = new object();

        private readonly PlayerData _playerData;
        public PlayerManager(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public void AddPlayer(Player player)
        {
            lock (Locker)
            {
                if (playerExistsByNick(player.Nick))
                    throw new PlayerExistsException("Player exists");
                _playerData.RegisteredPlayers.Add(player);
            }
        }

        public void DeletePlayer(string playerName)
        {
            lock (Locker)
            {
                if (playerExistsByNick(playerName))
                    throw new PlayerExistsException("Player exists");
                if(_playerData.ActivePlayers.Any(x => x.Nick.Equals(playerName)))
                    throw new PlayerInUseException("Cannot delete active player");
                _playerData.RegisteredPlayers.RemoveAll(x => x.Nick.Equals(playerName));
            }
        }

        private bool playerExistsByNick(string playerName)
        {
            return _playerData.RegisteredPlayers.Any(x => (x.Nick.Equals(playerName)) && (x.IsActive()));
        }

        public IEnumerator<Player> GetAllActivePlayers()
        {
            return _playerData.ActivePlayers.GetEnumerator();
        }

        public IEnumerator<Player> GetAllRegisteredPlayers()
        {
            return _playerData.RegisteredPlayers.Where(x => x.IsActive()).GetEnumerator();
        }
    }
}
