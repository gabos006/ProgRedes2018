using System;
using System.Net;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class Client
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

        public bool Connect(ServerEndpoint serverEndpoint)
        {
            var connected = false;

            try
            {
                var ipEndpoint = IpEndpoint();

                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(serverEndpoint.Socket);
                this.socket = socket;
                connected = true;
            }
            catch (SocketException)
            {
                Console.WriteLine("The server is down!!");
                connected = false;
            }

            return connected;
        }

        public void Disconnect()
        {
            if (socket != null )
                socket.Close();
        }

        public bool Login()
        {
            Login login = new Login();
            return login.DoLogin(socket);
        }

        public void AddPlayer()
        {
            AddPlayer addPlayer = new AddPlayer();
            addPlayer.DoAction(socket);
        }

        public void Move(int? command)
        {
            PlayGame play = new PlayGame(command);
            play.DoAction(socket);
        }

        public void Attack(int? command)
        {
            PlayGame play = new PlayGame(command);
            play.DoAction(socket);
        }

        public bool JoinGame(int? command)
        {
            StartActiveGame game = new StartActiveGame(command);
            return game.JoinActiveGame(socket);
        }
    }
}

