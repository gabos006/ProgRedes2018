using OblPR.Protocol;
using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class Logout : IAction
    {
        public bool DoAction(Socket socket)
        {
            try
            {
                var message = new ProtocolMessage();
                message.Command = Command.LOGOUT;
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
                response.RecieveResponse(socket);
                socket.Close();
                return false;
            }
            catch(SocketException)
            {
                Console.WriteLine("The server is down!!");
                return false;
            }
        }
    }
}
