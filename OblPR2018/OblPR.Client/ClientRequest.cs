using OblPR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Client
{
    public class ClientRequest
    {
        public void SendRequest(Socket socket, Message payload)
        {
            try
            {
                MessageHandler.SendMessage(socket, payload);
            }
            catch (SocketException)
            {
                Console.WriteLine("The server is down!!");
            }

        }
    }
}
