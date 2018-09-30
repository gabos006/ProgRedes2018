using OblPR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

            //Response
            ServerResponse response = new ServerResponse();
            return response.RecieveResponse(socket);
        }
    }
}
