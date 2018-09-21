using System;
using System.Globalization;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace OblPR.Server
{
    public class Client
    {
        public string IP { get; private set; }
        public string Port { get; private set; }
        private Socket _socket;
        private User _user;

        public Client(Socket socket, User user)
        {
            this._socket = socket;
            this._user = user;
        }

    }
}
