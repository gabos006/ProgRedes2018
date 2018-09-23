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
        private ILoginManager _loginManager;
        private IUserManager _userManager;
        private Socket _socket;
        private User _user;

        public ClientHandler(ILoginManager loginManager, IUserManager userManager, Socket socket)
        {
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
                MessageHandler.SendMessage(_socket, message);
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