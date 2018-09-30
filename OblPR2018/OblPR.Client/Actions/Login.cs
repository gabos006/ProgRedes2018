using OblPR.Protocol;
using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class Login : IAction
    {
        public bool DoAction(Socket socket)
        {
            Console.Write("Please, insert your nickname: ");
            var nickname = Console.ReadLine().Trim();

            try
            {
                var message = new ProtocolMessage();
                message.Command = Command.LOGIN;
                var parameter = new ProtocolParameter("name", nickname);
                message.Parameters.Add(parameter);
                var payload = new Message(message);
                MessageHandler.SendMessage(socket, payload);
                return true;
            }
            catch (SocketException)
            {
                Console.WriteLine("The server is down!!");
                return false;
            }
        }
    }
}
