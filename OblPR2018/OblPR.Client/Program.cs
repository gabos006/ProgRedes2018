using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    class Program
    {
        public static Client clientConnected;
        public static bool connected = false;

        static void Main(string[] args)
        {
            int? selectedOption = null;

            while (selectedOption != ClientCommand.DISCONNECT)
            {
                PrintConnectionMenu();
                selectedOption = HandleMenuInput(2);

                if (selectedOption == ClientCommand.CONNECT)
                {
                    ConnectToServer();

                    if (connected)
                    {
                        selectedOption = null;

                        while (selectedOption != ClientCommand.DISCONNECT)
                        {
                            PrintMainMenu();
                            selectedOption = HandleMenuInput(3);
                            if (selectedOption != ClientCommand.DISCONNECT)
                            {
                                if (selectedOption == ClientCommand.LOGIN)
                                {
                                    var logged = clientConnected.Login();

                                    if (logged)
                                    {
                                        selectedOption = null;

                                        while (selectedOption != ClientCommand.DISCONNECT)
                                        {
                                            PrintLoggedMenu();
                                            selectedOption = HandleMenuInput(3);
                                            if (selectedOption != ClientCommand.DISCONNECT)
                                            {
                                                //ACCIONES PARA COMENZAR PARTIDA
                                            }
                                            else
                                                DisconnectFromServer();
                                        }
                                    }
                                }
                                else
                                {
                                    if (selectedOption == ClientCommand.ADD_PLAYER)
                                    {
                                        // LLAMAR LOGICA DEL ADD PLAYER
                                    }
                                }
                            }
                            else
                                DisconnectFromServer(); 
                        }
                    }
                    else
                        DisconnectFromServer();
                }
            }
        }

        private static void PrintMainMenu()
        {
            Console.WriteLine("\n*********************************");
            Console.WriteLine("*          MAIN MENU         *");
            Console.WriteLine("*********************************");
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Add Player");
            Console.WriteLine("0 - Exit\n");
        }

        private static void PrintLoggedMenu()
        {
            Console.WriteLine("\n*********************************");
            Console.WriteLine("*           ACTIVE GAME          *");
            Console.WriteLine("*********************************");
            Console.WriteLine("1 - Connect like Monster");
            Console.WriteLine("2 - Connect like Survivor");
            Console.WriteLine("0 - Exit\n");
        }

        private static void PrintConnectionMenu()
        {
            Console.WriteLine("\n*********************************************");
            Console.WriteLine("* WELCOME TO SLASHER SIMULATION SYSTEM *");
            Console.WriteLine("*********************************************");
            Console.WriteLine("1 - Connect to server");
            Console.WriteLine("0 - Disconnect from server\n");
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

        private static void ConnectToServer()
        {
            //Console.Write("Insert server ip: ");
            //var SERVER_IP = Console.ReadLine().Trim();

            //Console.Write("Insert server port: ");
            //var SERVER_PORT = int.Parse(Console.ReadLine().Trim());

            //var CLIENT_IP = "192.168.1.3";

            //Console.Write("Insert client port: ");
            //var CLIENT_PORT = int.Parse(Console.ReadLine().Trim());

            var CLIENT_IP = "192.168.1.3";
            var SERVER_IP = "192.168.1.110";
            var CLIENT_PORT = 5000;
            var SERVER_PORT = 4000;

            clientConnected = new Client(CLIENT_IP, CLIENT_PORT);

            connected = clientConnected.Connect(new ServerEndpoint(SERVER_IP, SERVER_PORT));
        }

        private static void DisconnectFromServer()
        {
            clientConnected.Disconnect();
        }
    }
}
