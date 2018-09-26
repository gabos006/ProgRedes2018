using System;
using System.Net.Sockets;
using System.Threading;
using OblPR.Data.Entities;
using OblPR.Data.Services;
using OblPR.Protocol;

namespace OblPR.Server
{
    internal class ClientHandler
    {
        private readonly ILoginManager _loginManager;
        private readonly IPlayerManager _playerManager;
        private readonly Socket _socket;

        private Player _player;


        public ClientHandler(ILoginManager loginManager, IPlayerManager playerManager, Socket socket)
        {
            _loginManager = loginManager;
            _playerManager = playerManager;
            _socket = socket;
        }

        public void Connect()
        {
            HandleClientLogin();
            var requestThread = new Thread(ListenClientRequests);
            requestThread.Start();

        }

        private void Disconnect()
        {
            if (LoggedIn())
                _loginManager.Logout(_player.Nick);
            if (ClientConnected())
                _socket.Close();
        }

        private void ListenClientRequests()
        {
            while (ClientConnected())
            {
                try
                {
                    var message = MessageHandler.RecieveMessage(_socket);
                }
                catch (SocketException)
                {
                    Disconnect();
                }
            }

        }


        private void HandleClientLogin()
        {
            while (!LoggedIn() && ClientConnected())
            {
                try
                {
                    var recieved = MessageHandler.RecieveMessage(_socket);
                    var pmessage = recieved.PMessage;
                    if (pmessage.Command.Equals("login"))
                    {
                        try
                        {
                            _player = _loginManager.Login(pmessage.Parameters[0].Value);
                            Console.WriteLine("hola");
                        }
                        catch (PlayerNotFoundException)
                        {
                            //cosas
                        }
                        catch (PlayerInUseException)
                        {
                            //otras cosas
                        }
                    }
                }
                catch (SocketException)
                {
                    Disconnect();
                }

            }
        }

        private bool LoggedIn()
        {
            return _player != null;
        }

        private bool ClientConnected()
        {
            return _socket != null && _socket.Connected;
        }
    }
}