using System;
using System.Net.Sockets;
using System.Threading;
using OblPR.Data.Entities;
using OblPR.Game;
using OblPR.Data.Services;
using OblPR.Protocol;

namespace OblPR.Game
{
    internal class ClientHandler
    {
        private readonly ILoginManager _loginManager;
        private readonly IPlayerManager _playerManager;
        private readonly IGameServer _gameServer;
        private ICharacterHandler _characterHandler;

        private readonly Socket _socket;

        private Player _player;

        public ClientHandler(ILoginManager loginManager, IPlayerManager playerManager, IGameServer gameServer, Socket socket)
        {
            this._loginManager = loginManager;
            this._playerManager = playerManager;
            this._gameServer = gameServer;
            this._socket = socket;
        }

        public void Connect()
        {
            HandleClientLogin();
            HandleClientJoinGame();
            var requestThread = new Thread(ListenClientRequests);
            requestThread.Start();

        }

        private void HandleClientJoinGame()
        {
            while (ClientConnected() && _characterHandler == null)
            {
                
            }
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
                    if (pmessage.Command.Equals(Command.LOGIN));
                    {
                        try
                        {
                            _player = _loginManager.Login(pmessage.Parameters[0].Value);

                            var param = new ProtocolParameter("message", "LoggedIn");
                            var protoMessage = new ProtocolMessage();
                            protoMessage.Command = Command.OK;
                            protoMessage.Parameters.Add(param);
                            MessageHandler.SendMessage(_socket, new Message(protoMessage));
                        }
                        catch (PlayerNotFoundException)
                        {
                            var param = new ProtocolParameter("message", "PlayerNotFound");
                            var protoMessage = new ProtocolMessage();
                            protoMessage.Command = Command.ERROR;
                            protoMessage.Parameters.Add(param);
                            MessageHandler.SendMessage(_socket, new Message(protoMessage));

                        }
                        catch (PlayerInUseException)
                        {
                            var param = new ProtocolParameter("message", "PlayerInUse");
                            var protoMessage = new ProtocolMessage();
                            protoMessage.Command = Command.ERROR;
                            protoMessage.Parameters.Add(param);
                            MessageHandler.SendMessage(_socket, new Message(protoMessage));
                        }
                    }
                }
                catch (SocketException)
                {
                    Disconnect();
                }

            }
            ListenClientRequests();
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