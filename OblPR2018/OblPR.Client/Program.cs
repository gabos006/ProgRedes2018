using OblPR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Client
{
    class Program
    {
        public static Client clientConnected;

        static void Main(string[] args)
        {
            int? selectedOption = null;
            bool connected = false;

            while (selectedOption != Command.EXIT)
            {
                if (!connected)
                {
                    PrintConnectionMenu();
                    selectedOption = HandleMenuInput(2);
                }
                else
                {
                    PrintMainMenu();
                    selectedOption = HandleMenuInput(7);
                }


                if (selectedOption == Command.EXIT)
                    break;
                else
                {
                    if (selectedOption == Command.CONNECT)
                    {
                        clientConnected = ConnectToServer();
                        connected = true;
                    }
                    else
                    {
                        if (selectedOption == Command.DISCONNECT)
                        {
                            DisconnectFromServer();
                            connected = false;
                        }
                        else
                        {
                            var selectedHandler = HandleEvent(selectedOption);
                            clientConnected.AcceptHandler(selectedHandler);
                        }
                    }
                }
            }

        }

        private static void PrintMainMenu()
        {
            Console.WriteLine("\n*********************************");
            Console.WriteLine("*           MAIN MENU          *");
            Console.WriteLine("*********************************");
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Add Player");
            Console.WriteLine("7 - Logout");
            Console.WriteLine("0 - Exit\n");
        }

        private static void PrintConnectionMenu()
        {
            Console.WriteLine("\n**************************************************");
            Console.WriteLine("* WELCOME TO SLASHER SIMULATION SYSTEM *");
            Console.WriteLine("**************************************************");
            Console.WriteLine("1 - Connect to server");
            Console.WriteLine("2 - Disconnect from server");
            Console.WriteLine("0 - Exit\n");
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

        private static IHandler HandleEvent(int? selectedOption)
        {
            return HandlerFactory.Handler(selectedOption);
        }

        private static Client ConnectToServer()
        {
            Console.Write("Insert server ip: ");
            var SERVER_IP = Console.ReadLine().Trim();

            Console.Write("Insert server port: ");
            var SERVER_PORT = int.Parse(Console.ReadLine().Trim());

            var CLIENT_IP = "127.0.0.1";

            Console.Write("Insert client port: ");
            var CLIENT_PORT = int.Parse(Console.ReadLine().Trim());

            clientConnected = new Client(CLIENT_IP, CLIENT_PORT);
            clientConnected.Connect(new ServerEndpoint(SERVER_IP, SERVER_PORT));

            return clientConnected;
        }

        private static void DisconnectFromServer()
        {
            clientConnected.Disconnect();
        }
    }
}
