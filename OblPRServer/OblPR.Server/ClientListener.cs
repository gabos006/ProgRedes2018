using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OblPR.Server
{
    public class ClientListener
    {
        private bool _isRuning;
        private readonly int _port;
        private readonly string _ipAddr;

        private Socket _server;

        public ClientListener(string ipAddr, int port)
        {
            this._ipAddr = ipAddr;
            this._port = port;
        }


        public void Start()
        {
            var listener = new Thread(() =>
            {
                try
                {
                    if (_isRuning) return;
                    _isRuning = true;
                    _server = new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp
                    );
                    var endpoint = new IPEndPoint(
                        IPAddress.Parse(_ipAddr),
                        _port);

                    Console.WriteLine("Listening on : {0}", endpoint.ToString());
                    _server.Bind(endpoint);
                    _server.Listen(0);

                    while (_isRuning)
                    {
                        _server.Listen(0);
                        var socket = _server.Accept();
                        var clientThread = new Thread(() =>
                        {
                            HandleClient(socket);
                        });
                        clientThread.Start();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to start server");
                }

            });
            listener.Start();
        }

        private void HandleClient(Socket socket)
        {

            var client = new ClientHandler(socket);
            client.Start();
        }

        public void Stop()
        {
            if (!_isRuning) return;
            this._server.Close();
            this._isRuning = false;
        }
    }
}
