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

        public void AddUser(Player player)
        {
            lock (Locker)
            {
                if (_playerData.RegisteredPlayers.Any(x => (x.Nick.Equals(player.Nick)) && (x.IsActive())))
                    throw new PlayerExistsException("Player exists");
                _playerData.RegisteredPlayers.Add(player);
            }
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
