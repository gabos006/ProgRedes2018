using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using OblPR.Data.Services;
using OblPR.Game;

namespace OblPR.Game
{
    public class ClientListener:IClientHandler
    {
        private bool _isRuning;
        private Socket _server;
        private readonly IPlayerManager _playerManager;
        private readonly ILoginManager _loginManager;
        private readonly IGameServer _gameServer;

        public ClientListener(IPlayerManager playerManager, ILoginManager loginManager, IGameServer gameServer)
        {
            _playerManager = playerManager;
            _loginManager = loginManager;
            _gameServer = gameServer;
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
            
            var client = new ClientHandler(_loginManager, _playerManager, _gameServer, socket);
            client.Connect();
        }

        public void Stop()
        {
            if (!_isRuning) return;
            _server.Close();
            _isRuning = false;
        }

        public void NotifyPlayerNear()
        {
            throw new NotImplementedException();
        }



        public void NotifyMatchEnd(string result)
        {
            throw new NotImplementedException();
        }
    }
}
