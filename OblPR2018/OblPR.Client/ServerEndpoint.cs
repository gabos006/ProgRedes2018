using System.Net;

namespace OblPR.Client
{
    public class ServerEndpoint
    {

        private string ip;
        private int port;

        public IPEndPoint Socket { get; }

        public ServerEndpoint(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            Socket = new IPEndPoint(IPAddress.Parse(ip), port);
        }
    }
}