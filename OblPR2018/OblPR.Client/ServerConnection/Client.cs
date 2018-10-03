using OblPR.Protocol;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class Client
    {
        private string ip;
        private int port;
        private Socket socket;
        public bool match_end = false;

        public Client(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        private IPEndPoint IpEndpoint() => new IPEndPoint(IPAddress.Parse(ip), port);

        public bool Connect(ServerEndpoint serverEndpoint)
        {
            if (PortInUse(port))
            {
                Console.WriteLine("Port in use");
                return false;
            
            }
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
            IAction login = new Login();
            return login.DoAction(socket);
        }

        public bool Logout()
        {
            IAction logout = new Logout();
            return logout.DoAction(socket);
        }

        public void AddPlayer()
        {
            IAction addPlayer = new AddPlayer();
            addPlayer.DoAction(socket);
        }

        public void ActionGame(int? command)
        {
            IAction playGame = new PlayGame(command);
            playGame.DoAction(socket);
        }

        public bool JoinGame(int? command)
        {
            IAction startActiveGame = new StartActiveGame(command);
            return startActiveGame.DoAction(socket);
        }

        public void ListenServerGameResponse()
        {
            var isRunning = true;
            while (ClientConnected() && isRunning)
            {
                try
                {
                    var response = MessageHandler.RecieveMessage(socket);

                    ProtocolMessage pMessage = response.PMessage;
                    int commandResponse = response.PMessage.Command;

                    if (pMessage.Parameters[0].Name.Equals("message"))
                        Console.WriteLine(pMessage.Parameters[0].Value);

                    switch (pMessage.Command)
                    {
                        case Command.MATCH_END:
                            match_end = true;
                            break;

                        case Command.ERROR:
                            match_end = false;
                            break;
                    }
                }
                catch (SocketException)
                {
                    isRunning = false;
                }
            }
        }

        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();


            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }


            return inUse;
        }

        public bool ClientConnected()
        {
            return socket != null && socket.Connected;
        }
    }
}

