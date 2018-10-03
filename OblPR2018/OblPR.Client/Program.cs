using System.Configuration;

namespace OblPR.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var SERVER_IP = ConfigurationManager.AppSettings["serverIp"];
            var SERVER_PORT = int.Parse(ConfigurationManager.AppSettings["serverPort"]);

            var CLIENT_IP = ConfigurationManager.AppSettings["clientIp"];
            var CLIENT_PORT = int.Parse(ConfigurationManager.AppSettings["clientPort"]);

            var MULTI = bool.Parse(ConfigurationManager.AppSettings["multi"]);


            var client = new Client(SERVER_IP,SERVER_PORT,CLIENT_IP,CLIENT_PORT, MULTI);
            client.Start();
        }
    }
}
