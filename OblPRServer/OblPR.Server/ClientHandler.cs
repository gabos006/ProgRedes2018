using System;
using System.Net.Sockets;

namespace OblPR.Server
{
    internal class ClientHandler
    {
        private Func<User, object> p;
        private Socket socket;
        private User user;

        public ClientHandler(User user, Socket socket, Func<User, object> p)
        {
            this.user = user;
            this.socket = socket;
            this.p = p;
        }
    }
}