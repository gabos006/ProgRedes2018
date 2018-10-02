using System;
using System.Threading;

namespace OblPR.Client
{
    public class StartClient
    {
        public string serverIp;
        public int serverPort;
        public string clientIp;
        public int clientPort;
        public Client clientConnected;
         
        public StartClient(string ipServer, int portServer, string ipClient, int portClient)
        {
            serverIp = ipServer;
            serverPort = portServer;
            clientIp = ipClient;
            clientPort = portClient;
        }

        public void Start()
        {
            int? selectedOption = null;
            while (selectedOption != ClientCommand.DISCONNECT)
            {
                PrintConnectionMenu();
                selectedOption = HandleMenuInput(2);
                switch (selectedOption)
                {
                    case ClientCommand.CONNECT:
                        var connected = ConnectToServer();
                        if (connected)
                        {
                            selectedOption = null;
                            while (clientConnected.ClientConnected() && selectedOption != ClientCommand.DISCONNECT)
                            {
                                PrintMainMenu();
                                selectedOption = HandleMenuInput(3);
                                switch (selectedOption)
                                {
                                    case ClientCommand.LOGIN:
                                        var logged = clientConnected.Login();

                                        if (logged)
                                        {
                                            selectedOption = null;
                                            while (clientConnected.ClientConnected() &&
                                                   selectedOption != ClientCommand.DISCONNECT &&
                                                   logged)
                                            {
                                                PrintLoggedMenu();
                                                selectedOption = HandleMenuInput(3);
                                                switch (selectedOption)
                                                {
                                                    case ClientCommand.DISCONNECT:
                                                        DisconnectFromServer();
                                                        break;

                                                    case ClientCommand.LOGOUT:
                                                        logged = clientConnected.Logout();
                                                        break;


                                                    default: //Options like monster or survivor are the same
                                                        var joined = clientConnected.JoinGame(selectedOption);
                                                        if (joined)
                                                        {
                                                            //Open to listen server game response
                                                            Thread thread = new Thread(ListenServerResponse);
                                                            thread.Start();

                                                            selectedOption = null;
                                                            while (clientConnected.ClientConnected() &&
                                                                   selectedOption != ClientCommand.EXIT_GAME &&
                                                                   selectedOption != ClientCommand.DISCONNECT &&
                                                                   !clientConnected.match_end)
                                                            {
                                                                PrintActiveGameMenu();
                                                                selectedOption = HandleMenuInput(3);
                                                                switch (selectedOption)
                                                                {
                                                                    case ClientCommand.DISCONNECT:
                                                                        DisconnectFromServer();
                                                                        break;

                                                                    default:
                                                                        clientConnected.ActionGame(selectedOption);
                                                                        break;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                        break;

                                    case ClientCommand.ADD_PLAYER:
                                        clientConnected.AddPlayer();
                                        break;

                                    case ClientCommand.DISCONNECT:
                                        DisconnectFromServer();
                                        break;
                                }
                            }
                        }
                        break;

                    case ClientCommand.DISCONNECT:

                        DisconnectFromServer();
                        break;
                }
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

        private void PrintConnectionMenu()
        {
            Console.WriteLine("\n*********************************************");
            Console.WriteLine("*   WELCOME TO SLASHER SIMULATION SYSTEM    *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("1 - Connect to server");
            Console.WriteLine("0 - Disconnect from server\n");
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

        private int? HandleMenuInput(int options)
        {
            var isOptionValid = false;
            while (!isOptionValid)
            {
                try
                {
                    var selectedOption = int.Parse(Console.ReadLine());
                    if (selectedOption < 0 || selectedOption > options)
                    {
                        isOptionValid = false;
                        Console.WriteLine("Please input a number between 0 and " + options);
                    }
                    else
                    {
                        return selectedOption;
                    }
                }
                catch
                {
                    isOptionValid = false;
                    Console.WriteLine("Input has to be a number");
                }
            }
            return 0;
        }

        private void ListenServerResponse()
        {
            clientConnected.ListenServerGameResponse();
        }

        private bool ConnectToServer()
        {
            clientConnected = new Client(clientIp, clientPort);
            return clientConnected.Connect(new ServerEndpoint(serverIp, serverPort));
        }

        private void DisconnectFromServer()
        {
            clientConnected.Disconnect();
        }
    }
}
