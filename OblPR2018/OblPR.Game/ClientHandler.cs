using System;
using System.Net.Sockets;
using System.Threading;
using OblPR.Data.Entities;
using OblPR.Game;
using OblPR.Data.Services;
using OblPR.Protocol;

namespace OblPR.Game
{
    internal class ClientHandler:IClientHandler
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
            HandleCharacterSelection();
            var requestThread = new Thread(HandleGameCommands);
            requestThread.Start();

        }

        private void HandleCharacterSelection()
        {
            while (ClientConnected() && _characterHandler == null)
            {
                try
                {
                    var recieved = MessageHandler.RecieveMessage(_socket);
                    var pmessage = recieved.PMessage;
                    if (pmessage.Command.Equals(Command.JOIN_GAME))
                    {
                        try
                        {
                            var role = (Role) int.Parse(pmessage.Parameters[0].Value);
                            var character = new Character(_player, role);
                            _characterHandler = _gameServer.JoinGame(this, character);

                            var param = new ProtocolParameter("message", "Joined Successfully");
                            var ok = new ProtocolMessage {Command = Command.OK};
                            ok.Parameters.Add(param);
                            MessageHandler.SendMessage(_socket, new Message(ok));
                        }
                        catch (GameException e)
                        {
                            var param = new ProtocolParameter("message", e.Message);
                            var error = new ProtocolMessage {Command = Command.ERROR};
                            error.Parameters.Add(param);
                            MessageHandler.SendMessage(_socket, new Message(error));
                        }
                    }

                }
                catch (SocketException)
                {
                    Disconnect();
                }
            }
        }

        private void Disconnect()
        {

            if (LoggedIn())
                _loginManager.Logout(_player.Nick);
            if (ClientConnected())
                _socket.Close();
        }

        private void HandleGameCommands()
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
        }

        private bool LoggedIn()
        {
            return _player != null;
        }

        private bool ClientConnected()
        {
            return _socket != null && _socket.Connected;
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