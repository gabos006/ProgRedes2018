using OblPR.Protocol;
using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class ServerResponse
    { 

        public bool RecieveResponse(Socket socket)
        {
            bool haveResponse = false;
            try
            {
                var response = MessageHandler.RecieveMessage(socket);

                ProtocolMessage pMessage = response.PMessage;
                int commandResponse = response.PMessage.Command;

                if (pMessage.Parameters[0].Name.Equals("message"))
                {
                    Console.WriteLine(pMessage.Parameters[0].Value);
                    haveResponse = true;
                }

            }
            catch (SocketException)
            {
                Console.WriteLine("The server is down!!");
                haveResponse = false;
            }

            return haveResponse;
        }
    }
}
