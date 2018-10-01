using OblPR.Protocol;
using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class StartActiveGame : IAction
    {
        private int? command;

        public StartActiveGame(int? com)
        {
            command = com;
        }

        public bool DoAction(Socket socket)
        {
            try
            {
                ProtocolParameter parameter = null;
                var message = new ProtocolMessage();
                message.Command = Command.JOIN_GAME;

                switch (command)
                {
                    case ClientCommand.ACTIVE_GAME_MONSTER:
                        parameter = new ProtocolParameter("rol", Constant.Monster);
                        break;
                    case ClientCommand.ACTIVE_GAME_SURVIVOR:
                        parameter = new ProtocolParameter("rol", Constant.Survivor);
                        break;
                }

                message.Parameters.Add(parameter);
                var payload = new Message(message);
                MessageHandler.SendMessage(socket, payload);

            }
            catch (SocketException)
            {
                Console.WriteLine("The server is down!!");
                return false;
            }

            try
            {
                ServerResponse response = new ServerResponse();
                return response.RecieveResponse(socket);
            }
            catch (SocketException)
            {
                Console.WriteLine("The server is down!!");
                return false;
            }
        }
    }
}
