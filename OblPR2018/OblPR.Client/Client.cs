using System.Net;
using System.Net.Sockets;

namespace OblPR.Client
{
    class Client
    {
        private string ip;
        private int port;
        private Socket socket;

        public Client(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        private IPEndPoint IpEndpoint() => new IPEndPoint(IPAddress.Parse(ip), port);

        public void Connect(ServerEndpoint serverEndpoint)
        {
            var ipEndpoint = IpEndpoint();

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipEndpoint);
            socket.Connect(serverEndpoint.Socket);
            this.socket = socket;
        }


    }
}

