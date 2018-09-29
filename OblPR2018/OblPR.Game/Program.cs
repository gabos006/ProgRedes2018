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
            gameServer.StartServer("192.168.1.110", 4000);
        }
    }
}
