using OblPR.Data.Services;
using System.Configuration;


namespace OblPR.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = ConfigurationManager.AppSettings["serverIp"];
            string port = ConfigurationManager.AppSettings["serverPort"];

            var data = new PlayerData();
            IPlayerManager playerManager = new PlayerManager(data);
            ILoginManager loginManager = new LoginManager(data);

            var gameServer = new GameServer(playerManager, loginManager);

            gameServer.StartServer(ip, int.Parse(port));
        }
    }
}
