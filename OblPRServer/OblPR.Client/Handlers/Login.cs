using OblPR.Protocol;
using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class Login : IHandler
    {
        public void OnHandle(Socket socket)
        {

            Console.Write("Please, insert your nickname: ");
            var nickname = Console.ReadLine().Trim();

            var message = new ProtocolMessage();
            message.Command = "login";
            var parameter = new ProtocolParameter("name", nickname);
            message.Parameters.Add(parameter);
            var payload = new Message(message);
            MessageHandler.SendMessage(socket, payload);

            Console.WriteLine("Login Successfully");
        }
    }
}
