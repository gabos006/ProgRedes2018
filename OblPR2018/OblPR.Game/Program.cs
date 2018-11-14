using System;
using OblPR.Data.Services;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using OblPR.Protocol;
using System.Messaging;

namespace OblPR.Game
{
    public class Program
    {


        static void Main(string[] args)
        {
            var ip = ConfigurationManager.AppSettings["serverIp"];
            var port = ConfigurationManager.AppSettings["serverPort"];
            var remotingPort = ConfigurationManager.AppSettings["remotingPort"];
            var queueName = ConfigurationManager.AppSettings["queueName"];
         
            var data = new PlayerData();
            var playerManager = new PlayerManager(data);
            var loginManager = new LoginManager(data);
            var matchManager = new GameMatchManager();
            var logger = new ActionLogger(queueName);
            try
            {
                ChannelServices.RegisterChannel(new TcpChannel(int.Parse(remotingPort)), false);
                RemotingServices.Marshal(playerManager, ServiceNames.PlayerManager);
                RemotingServices.Marshal(matchManager, ServiceNames.MatchManager);

                var gameServer = new GameServer(playerManager, loginManager, matchManager, logger);

                gameServer.StartServer(ip, int.Parse(port));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
