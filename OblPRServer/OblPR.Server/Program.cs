using System;
using System.Net;
using System.Net.Sockets;

namespace OblPR.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Slasher Simulation System ");
            Console.WriteLine("Starting server...");

            Console.Write("Enter server ip: ");
            string ip = Console.ReadLine().Trim();

            Console.Write("Enter server port: ");
            string strPort = Console.ReadLine().Trim();
            int port = Int32.Parse(strPort);

            Console.Write("Enter maximum number of connections to the server: ");
            string strNumConnections = Console.ReadLine().Trim();
            int numConnections = Int32.Parse(strNumConnections);


            StartServer(ip,port,numConnections);


            Console.ReadLine();

        }

        static void StartServer(string ip, int port, int numConnections)
        {

            Console.WriteLine(ip);
            Console.WriteLine(port);
            Console.WriteLine(numConnections);

            var server = new Socket(AddressFamily.InterNetwork, // IPv4
                                    SocketType.Stream,
                                    ProtocolType.Tcp);

            server.Bind(new IPEndPoint(IPAddress.Parse(ip), port)); // Abre el socket en la ip y puerto

            server.Listen(numConnections);

            var client = server.Accept(); // Comienza a aceptar conexiones

            server.Close();

            //var msgBytes = new byte[256];

            //Thread t = new Thread(() => {
            //    while (true)
            //    {
            //        client.Receive(msgBytes);
            //        Console.WriteLine("Client: " + System.Text.Encoding.ASCII.GetString(msgBytes));
            //    }
            //});

            //t.Start();

            //while (true)
            //{
            //    Console.Write("Server: ");
            //    var msg = Console.ReadLine();

            //    var byteMsg = System.Text.Encoding.ASCII.GetBytes(msg);

            //    client.Send(byteMsg);
            //}
        }
    }
}
