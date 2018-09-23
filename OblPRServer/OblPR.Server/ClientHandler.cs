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
        private readonly MessageHandler _messageHandler;
        private readonly Socket _socket;

        private User _user;

        public ClientHandler(ILoginManager loginManager, IUserManager userManager, Socket socket)
        {
            _messageHandler = new MessageHandler();
            _loginManager = loginManager;
            _userManager = userManager;
            _socket = socket;
        }

        public void Connect()
        {
            RequestClientLogin();
            var requestThread = new Thread(ListenClientRequests);
            requestThread.Start();

        }

        public void Disconnect()
        {

        }

        private void ListenClientRequests() { 
        
            throw new NotImplementedException();
        }

        private void RequestClientLogin()
        {
            var message = new Message(Command.REQUEST_LOGIN, null);
            try
            {
                _messageHandler.Send(_socket, message);
                HandleLoginResponse();
            }
            catch (SocketException e)
            {
                Disconnect();
            }
        }

        private void HandleLoginResponse()
        {
            throw new NotImplementedException();
        }


    }
}