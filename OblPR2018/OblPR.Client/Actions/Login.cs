using OblPR.Protocol;
using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class Login
    {
        public bool DoLogin(Socket socket)
        {

            Console.Write("Please, insert your nickname: ");
            var nickname = Console.ReadLine().Trim();

            var message = new ProtocolMessage();
            message.Command = Command.LOGIN;
            var parameter = new ProtocolParameter("name", nickname);
            message.Parameters.Add(parameter);
            var payload = new Message(message);
            MessageHandler.SendMessage(socket, payload);

            //Response
            ServerResponse response = new ServerResponse();

            return response.RecieveResponse(socket);
        }
    }
}
