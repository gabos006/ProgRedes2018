using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using OblPR.Data.Services;

namespace OblPR.Server
{
    public class ClientListener
    {
        private bool _isRuning;
        private Socket _server;
        private readonly IUserManager _userManager;
        private readonly ILoginManager _loginManager;

        public ClientListener(IUserManager userManager, ILoginManager loginManager)
        {
            _userManager = userManager;
            _loginManager = loginManager;
        }

        public void StartListening(string ip, int port)
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
                        IPAddress.Parse(ip),
                        port);

                    Console.WriteLine("Listening on : {0}", endpoint);
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
                    Console.WriteLine(e.Message);
                }

            });
            listener.Start();
        }

        private void HandleClient(Socket socket)
        {
            
            var client = new ClientHandler(_loginManager, _userManager, socket);
            client.Connect();
        }

        public void Stop()
        {
            if (!_isRuning) return;
            _server.Close();
            _isRuning = false;
        }
    }
}
