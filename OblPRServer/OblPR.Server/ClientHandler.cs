using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace OblPR.Server
{
    internal class ClientHandler
    {
        private Socket _socket;
        private UserHandler _userManager;

        public ClientHandler(Socket socket, UserHandler userManager)
        {
            this._userManager = userManager;
            this._socket = socket;
        }

        public void Start()
        {


        }
    }
}