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
            var client = new Socket(AddressFamily.InterNetwork, // IPv4
                SocketType.Stream,
                ProtocolType.Tcp);


            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000)); // Me conecto con el server

            Console.WriteLine("Conected to server");
            Console.ReadLine();

            var message = new LoginPayload();
            var payload = new Message(message);
            MessageHandler.SendMessage(client, payload);

        }

    }
}
