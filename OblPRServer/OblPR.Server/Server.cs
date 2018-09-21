using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OblPR.Server
{
    public class Server : IServer
    {
        private bool _isRuning;
        private readonly int _port;
        private readonly string _ipAddr;
        private Socket _server;
        private List<User> activeUsers;

        public Server(string ipAddr, int port)
        {
            this._ipAddr = ipAddr;
            this._port = port;
            this.activeUsers = new List<User>();
        }


        public void Start()
        {
            var listener = new Thread(() =>
            {
                try
                {
                    if (!_isRuning)
                    {
                        _isRuning = true;
                        _server = new Socket(
                            AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp
                        );
                        IPEndPoint endpoint = new IPEndPoint(
                            IPAddress.Parse(_ipAddr),
                            _port);

                        Console.WriteLine("Listening on : {0}", endpoint.ToString());
                        _server.Bind(endpoint);
                        _server.Listen(0);

                        while (_isRuning)
                        {
                            _server.Listen(0);
                            var socket = _server.Accept();
                            var client = new Thread(() =>
                            {
                                handleClient(socket);
                            });
                            client.Start();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to start server");
                }

            });
            listener.Start();
        }

        private void handleClient(Socket socket)
        {
            sendLoginRequest(socket);
            User user = handleClientLogin(socket);
            ClientHandler client = new ClientHandler(user, socket, (User logged) =>
            {
                detachActiveUser(logged);
                return new Object();
            });

        }

        private void detachActiveUser(User logged)
        {
            this.activeUsers.Remove(logged);
        }

        private User handleClientLogin(Socket socket)
        {
            throw new NotImplementedException();
        }

        private void sendLoginRequest(Socket socket)
        {
            sendMessage(socket, MessageFactory.CreateMessage(Headers.OK, Commands.EMPTY, 0));
        }

        private void sendMessage(Socket socket, byte[] message)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            if (_isRuning)
            {
                this._server.Close();
                this._isRuning = false;
            }
        }
    }
}
