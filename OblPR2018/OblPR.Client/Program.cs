using System.Configuration;

namespace OblPR.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string SERVER_IP = ConfigurationManager.AppSettings["serverIp"];
            int SERVER_PORT = int.Parse(ConfigurationManager.AppSettings["serverPort"]);
            string CLIENT_IP = ConfigurationManager.AppSettings["clientIp"];
            int CLIENT_PORT = int.Parse(ConfigurationManager.AppSettings["clientPort"]);

            var client = new StartClient(SERVER_IP,SERVER_PORT,CLIENT_IP,CLIENT_PORT);
            client.Start();
        }
    }
}
