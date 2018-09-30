using OblPR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Client
{
    public class StartActiveGame
    {
        private int? command;

        public StartActiveGame(int? com)
        {
            command = com;
        }

        public bool JoinActiveGame(Socket socket)
        {
            ProtocolParameter parameter =  null;
            var message = new ProtocolMessage();
            message.Command = Command.JOIN_GAME;

            switch (command)
            {
                case ClientCommand.ACTIVE_GAME_MONSTER:
                    parameter = new ProtocolParameter("rol", Command.MONSTER_ROL);
                    break;
                case ClientCommand.ACTIVE_GAME_SURVIVOR:
                    parameter = new ProtocolParameter("rol", Command.SURVIVOR_ROL);
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
