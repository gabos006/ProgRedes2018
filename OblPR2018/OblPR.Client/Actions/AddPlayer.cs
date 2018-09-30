using OblPR.Protocol;
using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class AddPlayer : IAction
    {
        public bool DoAction(Socket socket)
        {
            Console.Write("Insert nickname: ");
            var nickname = Console.ReadLine().Trim();
            Console.Write("Insert image path: ");
            var imagePath = Console.ReadLine().Trim();
            try
            {
                var message = new ProtocolMessage();
                message.Command = Command.ADD_PLAYER;
                var paramNickname = new ProtocolParameter("name", nickname);
                message.Parameters.Add(paramNickname);
                var paramImage = new ProtocolParameter("image", imagePath);
                message.Parameters.Add(paramImage);
                var payload = new Message(message);
                MessageHandler.SendMessage(socket, payload);
            }
            catch (SocketException)
            {
                Console.WriteLine("The server is down!!");
                return false;
            }

            try
            {
                ServerResponse response = new ServerResponse();
                return response.RecieveResponse(socket);
            }
            catch (SocketException)
            {
                Console.WriteLine("The server is down!!");
                return false;
            }
        }
    }
}
