using System;
using System.Net.Sockets;
using System.Threading;
using OblPR.Data.Entities;
using OblPR.Game;
using OblPR.Data.Services;
using OblPR.Protocol;

namespace OblPR.Game
{
    internal class ClientHandler : IClientHandler
    {
        private readonly ILoginManager _loginManager;
        private readonly IPlayerManager _playerManager;
        private readonly IControlsProvider _controlsProvider;
        private ICharacterHandler _characterHandler;

        private readonly Socket _socket;
        private Thread _requestThread;

        private Player _player;

        public ClientHandler(ILoginManager loginManager, IPlayerManager playerManager, IControlsProvider controlsProvider, Socket socket)
        {
            this._loginManager = loginManager;
            this._playerManager = playerManager;
            this._controlsProvider = controlsProvider;
            this._socket = socket;
        }

        public void Connect()
        {
            HandleClientLogin();
            HandleGameStart();

        }

        private void HandleGameStart()
        {


            HandleCharacterSelection();
            if (ClientConnected() && HasGameControls())
            {
                _requestThread = new Thread(HandleGameCommands);
                _requestThread.Start();
            }



        }

        private bool HasGameControls()
        {
            return _characterHandler != null;
        }

        private void HandleCharacterSelection()
        {
            while (ClientConnected() && !HasGameControls())
            {
                try
                {
                    var recieved = MessageHandler.RecieveMessage(_socket);
                    var pmessage = recieved.PMessage;
                    if (pmessage.Command.Equals(Command.JOIN_GAME))
                    {
                        try
                        {
                            var role = (Role)int.Parse(pmessage.Parameters[0].Value);
                            var character = new Character(_player, role);
                            _characterHandler = _controlsProvider.JoinGame(this, character);

                            var param = new ProtocolParameter("message", "Joined Successfully");
                            var ok = new ProtocolMessage { Command = Command.OK };
                            ok.Parameters.Add(param);
                            MessageHandler.SendMessage(_socket, new Message(ok));
                        }
                        catch (GameException e)
                        {
                            var param = new ProtocolParameter("message", e.Message);
                            var error = new ProtocolMessage { Command = Command.ERROR };
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
            if (HasGameControls())
                _characterHandler.ExitMatch();
            if (LoggedIn())
                _loginManager.Logout(_player.Nick);
            if (ClientConnected())
                _socket.Close();
        }

        private void HandleGameCommands()
        {
            while (ClientConnected() && HasGameControls())
            {
                try
                {
                    var message = MessageHandler.RecieveMessage(_socket);
                    var pmessage = message.PMessage;

                    if (pmessage.Command.Equals(Command.MOVE))
                    {
                        try
                        {
                            var x = int.Parse(pmessage.Parameters[0].Value);
                            var y = int.Parse(pmessage.Parameters[1].Value);
                            _characterHandler.Move(new Point(x, y));

                            var param = new ProtocolParameter("message", "Moved OK");
                            var protoMessage = new ProtocolMessage { Command = Command.OK };
                            protoMessage.Parameters.Add(param);
                            MessageHandler.SendMessage(_socket, new Message(protoMessage));
                        }
                        catch (GameException e)
                        {
                            var param = new ProtocolParameter("message", e.Message);
                            var protoMessage = new ProtocolMessage { Command = Command.ERROR };
                            protoMessage.Parameters.Add(param);
                            MessageHandler.SendMessage(_socket, new Message(protoMessage));
                        }

                    }


                    if (pmessage.Command.Equals(Command.ATTACK))
                    {
                        try
                        {

                            _characterHandler.Attack();

                            var param = new ProtocolParameter("message", "Attack OK");
                            var protoMessage = new ProtocolMessage { Command = Command.OK };
                            protoMessage.Parameters.Add(param);
                            MessageHandler.SendMessage(_socket, new Message(protoMessage));
                        }
                        catch (GameException e)
                        {
                            var param = new ProtocolParameter("message", e.Message);
                            var protoMessage = new ProtocolMessage { Command = Command.ERROR };
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

        private void HandleClientLogin()
        {
            while (!LoggedIn() && ClientConnected())
            {
                try
                {
                    var recieved = MessageHandler.RecieveMessage(_socket);
                    var pmessage = recieved.PMessage;
                    if (pmessage.Command.Equals(Command.LOGIN)) ;
                    {
                        try
                        {
                            _player = _loginManager.Login(pmessage.Parameters[0].Value);

                            var param = new ProtocolParameter("message", "LoggedIn");
                            var protoMessage = new ProtocolMessage { Command = Command.OK };
                            protoMessage.Parameters.Add(param);
                            MessageHandler.SendMessage(_socket, new Message(protoMessage));
                        }
                        catch (PlayerNotFoundException e)
                        {
                            var param = new ProtocolParameter("message", e.Message);
                            var protoMessage = new ProtocolMessage { Command = Command.ERROR };
                            protoMessage.Parameters.Add(param);
                            MessageHandler.SendMessage(_socket, new Message(protoMessage));

                        }
                        catch (PlayerInUseException e)
                        {
                            var param = new ProtocolParameter("message", e.Message);
                            var protoMessage = new ProtocolMessage { Command = Command.ERROR };
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

        public void NotifyPlayerNear(string result)
        {
            if (ClientConnected() && HasGameControls())
            {
                try
                {
                    var param = new ProtocolParameter("message", "Player near!");
                    var protoMessage = new ProtocolMessage { Command = Command.PLAYER_NOTIFICATION };
                    protoMessage.Parameters.Add(param);
                    MessageHandler.SendMessage(_socket, new Message(protoMessage));
                }
                catch (SocketException e)
                {
                    Disconnect();
                }
            }

        }

        public void NotifyMatchEnd(string result)
        {
            _characterHandler = null;

            if (ClientConnected())
            {
                try
                {
                    var param = new ProtocolParameter("message", result);
                    var protoMessage = new ProtocolMessage { Command = Command.MATCH_END };
                    protoMessage.Parameters.Add(param);
                    MessageHandler.SendMessage(_socket, new Message(protoMessage));

                }
                catch (SocketException e)
                {
                    Disconnect();
                }
            }


        }
    }
}