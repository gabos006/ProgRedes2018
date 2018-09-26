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
            var done = false;
            while (!done)
            {
                var message = MessageHandler.RecieveMessage(_socket);
                try
                {
                    ParseMessage(message);

                }
                catch (SocketException e)
                {
                    done = true;
                }
            }

        }



        private void HandleClientLogin()
        {
            while (!LoggedIn())
            {
                var message = MessageHandler.RecieveMessage(_socket);
                var objectMessage = JsonConvert.DeserializeObject<ProtocolMessage>(message.JSONString);
                try
                {
                    
                }
            }
        }

        private bool LoggedIn()
        {
            return (this._user != null);
        }
    }
}