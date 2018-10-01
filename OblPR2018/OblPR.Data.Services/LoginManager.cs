using System.Linq;
using OblPR.Data.Entities;

namespace OblPR.Data.Services
{
    public class LoginManager : ILoginManager
    {
        private static readonly object Locker = new object();
        private readonly PlayerData _playerData;

        public LoginManager(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public Player Login(string userName)
        {
            lock (Locker)
            {
                if (_playerData.ActivePlayers.Any(x => x.Nick.Equals(userName)))
                    throw new PlayerInUseException();
                if (!_playerData.RegisteredPlayers.Any(x => x.Nick.Equals(userName)))
                    throw new PlayerNotFoundException();
                var player = _playerData.RegisteredPlayers.FirstOrDefault((x => x.Nick.Equals(userName)));
                _playerData.ActivePlayers.Add(player);
                return player;
            }
        }

        public void Logout(string userName)
        {
            lock (Locker)
            {

                if (_playerData.RegisteredPlayers.Any(x => x.Nick.Equals(userName)))
                    _playerData.ActivePlayers.RemoveAll(x => x.Nick.Equals(userName));
            }
        }
    }
}
