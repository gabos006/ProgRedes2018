using System;
using System.ComponentModel.Design;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using OblPR.Data.Entities;
using OblPR.Data.Services;
using OblPR.Protocol;

namespace OblPR.Server
{
    internal class ClientHandler
    {
        private readonly ILoginManager _loginManager;
        private readonly IUserManager _userManager;
        private readonly Socket _socket;

        private User _user;


        public ClientHandler(ILoginManager loginManager, IUserManager userManager, Socket socket)
        {
            _loginManager = loginManager;
            _userManager = userManager;
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
            _loginManager.Logout(_user.Nick);
            //cosas
        }

        private void ListenClientRequests()
        {
            while (ClientConnected())
            {
                try
                {
                    var message = MessageHandler.RecieveMessage(_socket);
                }
                catch (SocketException e)
                {
                    Disconnect();
                }
            }

        }


        private void HandleClientLogin()
        {
            while (!LoggedIn())
            {
                try
                {
                    var recieved = MessageHandler.RecieveMessage(_socket);
                    var pmessage = recieved.PMessage;
                    if (pmessage.Command.Equals("login"))
                    {
                        try
                        {
                            _loginManager.Login(pmessage.Parameters[0].Value);

                        }
                        catch (PlayerNotFoundException e)
                        {

                        }
                        catch (PlayerInUseException e)
                        {

                        }
                    }
                }
                catch (SocketException e)
                {
                    Disconnect();
                }

            }
        }

        private bool LoggedIn()
        {
            return (this._user != null);
        }

        private bool ClientConnected()
        {
            return _socket != null && _socket.Connected;
        }
    }
}