using OblPR.Protocol;
using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class ServerResponse
    {
        private Socket socket;

        public ServerResponse(Socket serverSocket)
        {
            socket = serverSocket;
        }

        public void RecieveResponse()
        {
            var response = MessageHandler.RecieveMessage(socket);

            ProtocolMessage pMessage = response.PMessage;
            int commandResponse = response.PMessage.Command;

            if (pMessage.Parameters[0].Name.Equals("message"))
            {
                Console.WriteLine(pMessage.Parameters[0].Value);
            }
        }
    }
}
