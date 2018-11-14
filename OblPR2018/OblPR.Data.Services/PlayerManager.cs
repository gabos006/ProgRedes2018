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
                if (PlayerExistsByNick(player.Nick))
                    throw new PlayerExistsException("Player exists");
                _playerData.RegisteredPlayers.Add(player);
            }
        }

        public void DeletePlayer(string playerName)
        {
            lock (Locker)
            {
                if (!PlayerExistsByNick(playerName))
                    throw new PlayerNotFoundException("Player doesnt Exists");
                if(_playerData.ActivePlayers.Any(x => x.Nick.Equals(playerName)))
                    throw new PlayerInUseException("Cannot delete active player");
                _playerData.RegisteredPlayers.RemoveAll(x => x.Nick.Equals(playerName));
            }
        }

        public void UpdatePlayer(Player player)
        {
            lock (Locker)
            {
                if(!PlayerExist(player))
                    throw new PlayerNotFoundException("Player doesnt Exists");
                if (_playerData.ActivePlayers.Any(x => x.Id.Equals(player.Id)))
                    throw new PlayerInUseException("Cannot update active player");
                var update = _playerData.RegisteredPlayers.FirstOrDefault(x => x.Id.Equals(player.Id));
                update.Image = player.Image;

            }
        }

        private bool PlayerExist(Player player)
        {
            return _playerData.RegisteredPlayers.Any(x => x.Id.Equals(player.Id));
        }

        private bool PlayerExistsByNick(string playerName)
        {
            return _playerData.RegisteredPlayers.Any(x => (x.Nick.Equals(playerName)) && (x.IsActive()));
        }

        public List<Player> GetAllActivePlayers()
        {
            return _playerData.ActivePlayers.ToList();
        }

        public List<Player> GetAllRegisteredPlayers()
        {
            return _playerData.RegisteredPlayers.Where(x => x.IsActive()).ToList();
        }
    }
}
