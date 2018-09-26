using OblPR.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new PlayerData();
            IPlayerManager playerManager = new PlayerManager(data);
            ILoginManager loginManager = new LoginManager(data);
            var gameServer = new GameServer(playerManager, loginManager);
            gameServer.StartServer("127.0.0.1", 4000);
        }
    }
}
