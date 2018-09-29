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
        static void Main(string[] args)
        {
            //var client = new Socket(AddressFamily.InterNetwork, // IPv4
            //    SocketType.Stream,
            //    ProtocolType.Tcp);


            //client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000)); // Me conecto con el server

            //Console.WriteLine("Conected to server");
            //Console.ReadLine();

            //var message = new ProtocolMessage();
            //message.Command = "login";
            //var parameter = new ProtocolParameter("name", "Dario");
            //message.Parameters.Add(parameter);
            //var payload = new Message(message);
            //MessageHandler.SendMessage(client, payload);



            Console.WriteLine("*** Welcome to the Game Slash ***");
            Console.WriteLine("*** Connecting to server... ***");

            var SERVER_IP = "127.0.0.1";
            var SERVER_PORT = 4000;
            var CLIENT_IP = "127.0.0.1";
            var CLIENT_PORT = 6000;

            var client = new Client(CLIENT_IP, CLIENT_PORT);
            client.Connect(new ServerEndpoint(SERVER_IP, SERVER_PORT));

            int? selectedOption = null;

            while (selectedOption != Command.EXIT)
            {
                PrintMainMenu();

                selectedOption = HandleMenuInput();

                if (selectedOption == Command.EXIT)
                    break;

                var selectedHandler = HandleEvent(selectedOption);

                client.AcceptHandler(selectedHandler);
            }

        }

        private static void PrintMainMenu()
        {
            Console.WriteLine("\n*********************************");
            Console.WriteLine("*           MAIN MENU          *");
            Console.WriteLine("*********************************");
            Console.WriteLine("1 - Login");
            Console.WriteLine("7 - Logout");
            Console.WriteLine("0 - Exit\n");
        }

        private static int HandleMenuInput()
        {
            var isOptionValid = false;
            while (!isOptionValid)
            {
                try
                {
                    var selectedOption = int.Parse(Console.ReadLine());
                    if (selectedOption < 0 || selectedOption > 7)
                    {
                        isOptionValid = false;
                        Console.WriteLine("Please input a number between 0 and 7");
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
    }
}
