using System;
using System.Net.Sockets;
using System.Threading;

namespace OblPR.Client
{
    class Program
    {
        public static Client clientConnected;

        static void Main(string[] args)
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
                                            while (clientConnected.ClientConnected() && selectedOption != ClientCommand.DISCONNECT)
                                            {
                                                PrintLoggedMenu();
                                                selectedOption = HandleMenuInput(3);
                                                switch (selectedOption)
                                                {
                                                    case ClientCommand.DISCONNECT:
                                                        DisconnectFromServer();
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

        private static void PrintMainMenu()
        {
            Console.WriteLine("\n*********************************************");
            Console.WriteLine("*                 MAIN MENU                 *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Add Player");
            Console.WriteLine("0 - Disconnect\n");
        }

        private static void PrintLoggedMenu()
        {
            Console.WriteLine("\n*********************************************");
            Console.WriteLine("*              JOIN ACTIVE GAME             *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("1 - Connect like Monster");
            Console.WriteLine("2 - Connect like Survivor");
            Console.WriteLine("0 - Disconnect\n");
        }

        private static void PrintConnectionMenu()
        {
            Console.WriteLine("\n*********************************************");
            Console.WriteLine("*   WELCOME TO SLASHER SIMULATION SYSTEM    *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("1 - Connect to server");
            Console.WriteLine("0 - Disconnect from server\n");
        }

        private static void PrintActiveGameMenu()
        {
            Console.WriteLine("\n*********************************************");
            Console.WriteLine("*                ACTIVE GAME                *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("1 - Move");
            Console.WriteLine("2 - Attack");
            Console.WriteLine("3 - Exit active game");
            Console.WriteLine("0 - Disconnect\n");
        }

        private static int? HandleMenuInput(int options)
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

        private static void ListenServerResponse()
        {
            clientConnected.ListenServerGameResponse();
        }

        private static bool ConnectToServer()
        {
            //Console.Write("Insert server ip: ");
            //var SERVER_IP = Console.ReadLine().Trim();

            //Console.Write("Insert server port: ");
            //var SERVER_PORT = int.Parse(Console.ReadLine().Trim());

            //var CLIENT_IP = "192.168.1.3";

            //Console.Write("Insert client port: ");
            //var CLIENT_PORT = int.Parse(Console.ReadLine().Trim());

            var CLIENT_IP = "192.168.43.134";
            var SERVER_IP = "192.168.43.247";
            var CLIENT_PORT = 5000;
            var SERVER_PORT = 4000;

            clientConnected = new Client(CLIENT_IP, CLIENT_PORT);

            return clientConnected.Connect(new ServerEndpoint(SERVER_IP, SERVER_PORT));
        }

        private static void DisconnectFromServer()
        {
            clientConnected.Disconnect();
        }
    }
}
