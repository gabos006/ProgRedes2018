using OblPR.Protocol;
using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class AddPlayer : IHandler
    {
        public void OnHandle(Socket socket)
        {
            Console.Write("Insert nickname: ");
            var nickname = Console.ReadLine().Trim();
            Console.Write("Insert image path: ");
            var imagePath = Console.ReadLine().Trim();

            //var message = new ProtocolMessage();
            //message.Command = "addPlayer";
            //var paramNickname = new ProtocolParameter("name", nickname);
            //message.Parameters.Add(paramNickname);
            //var paramImage = new ProtocolParameter("image", imagePath);
            //message.Parameters.Add(paramImage);
            //var payload = new Message(message);
            //MessageHandler.SendMessage(socket, payload);


            //Response
        }
    }
}
