using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using OblPR.Protocol;

namespace OblPR.Client
{
    public class Client
    {
        private readonly string _serverIp;
        private readonly int _serverPort;
        private readonly string _clientIp;
        private readonly int _clientPort;

        private bool _running;
        private bool _loggedIn;
        private bool _playing;


        private bool _bound;
        private bool _multi;

        private Socket _socket;

        public Client(string ipServer, int portServer, string ipClient, int portClient, bool multi)
        {
            _serverIp = ipServer;
            _serverPort = portServer;
            _clientIp = ipClient;
            _clientPort = portClient;
            _multi = multi;
        }

        public void Start()
        {
            _running = true;
            while (_running)
            {
                while (_running && !Connected())
                {
                    PrintConnectionMenu();
                    ReadConnectionMenu();
                }

                while (_running && Connected() && !_loggedIn)
                {
                    PrintMainMenu();
                    ReadMainMenu();

                }
                while (_running && Connected() && _loggedIn && !_playing)
                {
                    PrintLoggedMenu();
                    ReadLoggedMenu();
                }

                while (_running && Connected() && _loggedIn && _playing)
                {
                    PrintActiveGameMenu();
                    ReadActiveGameMenu();
                }
            }
        }


        private void PrintConnectionMenu()
        {
            Console.WriteLine("\n*********************************************");
            Console.WriteLine("*   WELCOME TO SLASHER SIMULATION SYSTEM    *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("1 - Connect to server");
            Console.WriteLine("0 - Disconnect from server\n");
        }


        private void ReadConnectionMenu()
        {
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ConnectToServer();
                    break;
                case "0":
                    Disconnect();
                    break;
            }
        }


        private void PrintMainMenu()
        {
            Console.WriteLine("\n*********************************************");
            Console.WriteLine("*                 MAIN MENU                 *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Add Player");
            Console.WriteLine("0 - Disconnect\n");
        }

        private void ReadMainMenu()
        {
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    Disconnect();
                    break;
                case "1":
                    Login();
                    break;
                case "2":
                    AddPlayer();
                    break;
            }
        }

        private void AddPlayer()
        {
            Console.Write("Insert nickname: ");
            var nickname = Console.ReadLine()?.Trim();

            Console.Write("Insert image path: ");
            var imagePath = Console.ReadLine()?.Trim();
            try
            {
                var message = new ProtocolMessage { Command = Command.ADD_PLAYER };

                var paramNickname = new ProtocolParameter("name", nickname);
                message.Parameters.Add(paramNickname);

                var paramImage = new ProtocolParameter("image", ReadImageFromFile(imagePath));
                message.Parameters.Add(paramImage);

                var payload = new Message(message);
                MessageHandler.SendMessage(_socket, payload);


                ServerAck();
            }
            catch (SocketException)
            {
                Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Login()
        {
            Console.Write("Please, insert your nickname: ");
            var nickname = Console.ReadLine()?.Trim();

            try
            {
                var message = new ProtocolMessage { Command = Command.LOGIN };
                var parameter = new ProtocolParameter("name", nickname);
                message.Parameters.Add(parameter);
                var payload = new Message(message);
                MessageHandler.SendMessage(_socket, payload);

                _loggedIn = ServerAck();

            }
            catch (SocketException)
            {
                Disconnect();
            }
        }

        private void PrintLoggedMenu()
        {
            Console.WriteLine("\n*********************************************");
            Console.WriteLine("*              JOIN ACTIVE GAME             *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("1 - Connect like Monster");
            Console.WriteLine("2 - Connect like Survivor");
            Console.WriteLine("3 - Logout");
            Console.WriteLine("0 - Disconnect\n");
        }


        private void ReadLoggedMenu()
        {
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    PlayAsMonster();
                    break;
                case "2":
                    PlayAsSurvivor();
                    break;
                case "3":
                    Logout();
                    break;
                case "0":
                    Disconnect();
                    break;
            }
        }

        private void Logout()
        {
            try
            {
                var message = new ProtocolMessage { Command = Command.LOGOUT };
                var payload = new Message(message);
                MessageHandler.SendMessage(_socket, payload);

                ServerAck();
                _loggedIn = false;
            }
            catch (SocketException)
            {
                Disconnect();
            }

        }

        private void PlayAsSurvivor()
        {
            try
            {
                var message = new ProtocolMessage { Command = Command.JOIN_GAME };
                var parameter = new ProtocolParameter("role", Constant.Survivor);


                message.Parameters.Add(parameter);
                var payload = new Message(message);
                MessageHandler.SendMessage(_socket, payload);

                _playing = ServerAck();
                if (!_playing) return;
                var listener = new Thread(ListenServerGameResponse);
                listener.Start();
            }
            catch (SocketException)
            {
                Disconnect();
            }

        }

        private void PlayAsMonster()
        {
            try
            {
                var message = new ProtocolMessage { Command = Command.JOIN_GAME };
                var parameter = new ProtocolParameter("role", Constant.Monster);

                message.Parameters.Add(parameter);
                var payload = new Message(message);
                MessageHandler.SendMessage(_socket, payload);

                _playing = ServerAck();
                if (!_playing) return;
                var listener = new Thread(ListenServerGameResponse);
                listener.Start();
            }
            catch (SocketException)
            {
                Disconnect();
            }
        }


        private void PrintActiveGameMenu()
        {
            Console.WriteLine("\n*********************************************");
            Console.WriteLine("*                ACTIVE GAME                *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("1 - Move");
            Console.WriteLine("2 - Attack");
            Console.WriteLine("3 - Exit active game");
            Console.WriteLine("0 - Disconnect\n");
        }

        private void ReadActiveGameMenu()
        {
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    MovePlayer();
                    break;
                case "2":
                    AttackPlayer();
                    break;
                case "3":
                    ExitGame();
                    break;
                case "0":
                    Disconnect();
                    break;
            }
        }

        private void ExitGame()
        {
            try
            {
                var message = new ProtocolMessage { Command = Command.MATCH_END };
                var payload = new Message(message);

                if (!_playing)
                    return;

                MessageHandler.SendMessage(_socket, payload);
                _playing = false;
            }
            catch (SocketException)
            {
                Disconnect();
            }

        }

        private void AttackPlayer()
        {
            try
            {
                var message = new ProtocolMessage { Command = Command.ATTACK };
                var payload = new Message(message);

                if (!_playing)
                    return;

                MessageHandler.SendMessage(_socket, payload);
            }
            catch (SocketException)
            {
                Disconnect();
            }
        }

        private void MovePlayer()
        {
            try
            {
                var message = new ProtocolMessage { Command = Command.MOVE };
                Console.Write("Insert coordinate x: ");
                var coordinateX = new ProtocolParameter("x", HandleInput());

                Console.Write("Insert coordinate y: ");
                var coordinateY = new ProtocolParameter("y", HandleInput());

                message.Parameters.Add(coordinateX);
                message.Parameters.Add(coordinateY);
                var payload = new Message(message);

                if (!_playing)
                    return;

                MessageHandler.SendMessage(_socket, payload);
            }
            catch (SocketException)
            {
                Disconnect();
            }
        }

        private string HandleInput()
        {
            while (true)
            {
                try
                {
                    var input = int.Parse(Console.ReadLine().Trim());
                    return input.ToString();
                }
                catch
                {
                    Console.WriteLine("Input has to be a number");
                }
            }
        }

        private void ConnectToServer()
        {
            try
            {
                var srvEndPoint = new IPEndPoint(
                    IPAddress.Parse(_serverIp),
                    _serverPort);

                var clientEndPoint = new IPEndPoint(
                    IPAddress.Parse(_clientIp),
                    _clientPort);

                var client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);

                if (!_multi)
                {
                    if (!_bound)
                    {
                        client.Bind(clientEndPoint);
                        _bound = true;
                    }
                }


                client.Connect(srvEndPoint);
                _socket = client;
                Console.WriteLine("Connected");
            }
            catch (SocketException)
            {
                Disconnect();
            }
        }


        private bool ServerAck()
        {
            var response = MessageHandler.RecieveMessage(_socket);
            var pMessage = response.PMessage;
            if (pMessage.Parameters[0].Name.Equals("message"))
            {
                Console.WriteLine(pMessage.Parameters[0].Value);
            }

            return pMessage.Command == Command.OK;
        }

        private void ListenServerGameResponse()
        {
            while (_running && Connected() && _loggedIn && _playing)
            {
                try
                {
                    var response = MessageHandler.RecieveMessage(_socket);
                    var pMessage = response.PMessage;

                    if (pMessage.Parameters[0].Name.Equals("message"))
                        Console.WriteLine(pMessage.Parameters[0].Value);

                    if (pMessage.Command == Command.MATCH_END)
                    {
                        var message = new ProtocolMessage { Command = Command.OK };
                        var payload =new Message(message);
                        MessageHandler.SendMessage(_socket, payload);
                        _playing = false;
                    }

                }
                catch (SocketException)
                {
                    Disconnect();
                }
            }
        }

        private string ReadImageFromFile(string path)
        {
            byte[] image = File.ReadAllBytes(path);
            var image64 = Convert.ToBase64String(image);
            return image64;
        }

        private bool Connected()
        {
            return _socket != null && _socket.Connected;
        }
        private void Disconnect()
        {
            _running = false;
            _loggedIn = false;

            if (Connected())
                _socket.Close();
        }

    }
}
