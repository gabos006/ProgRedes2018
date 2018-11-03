using OblPR.Data.Services;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;


namespace OblPR.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            var ip = ConfigurationManager.AppSettings["serverIp"];
            var port = ConfigurationManager.AppSettings["serverPort"];
            var remotingPort = ConfigurationManager.AppSettings["remotingPort"];


            var data = new PlayerData();
            var playerManager = new PlayerManager(data);
            var loginManager = new LoginManager(data);
            var matchManager = new GameMatchManager();

            var remotingTcpChannel = new TcpChannel(int.Parse(remotingPort));
            ChannelServices.RegisterChannel(
                remotingTcpChannel,
                false);
            RemotingServices.Marshal(playerManager, "PlayerManager");

            var gameServer = new GameServer(playerManager, loginManager, matchManager);

            gameServer.StartServer(ip, int.Parse(port));
        }
    }
}
