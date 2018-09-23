using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace OblPR.Server
{
    internal class ClientHandler
    {
        private Socket _socket;

        public ClientHandler(Socket socket)
        {
            this._socket = socket;
        }

        public void Start()
        {


        }
    }
}