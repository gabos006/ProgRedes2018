using OblPR.Protocol;
using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class PlayGame : IAction
    {
        public int? command;

        public PlayGame(int? comm)
        {
            command = comm;
        }

        public bool DoAction(Socket socket)
        {
            try
            {
                var message = new ProtocolMessage();

                switch (command)
                {
                    case ClientCommand.MOVE:

                        Console.Write("Insert coordinate x: ");
                        var x = Console.ReadLine().Trim();
                        Console.Write("Insert coordinate y: ");
                        var y = Console.ReadLine().Trim();
                        message.Command = Command.MOVE;
                        var coordinateX = new ProtocolParameter("x", x);
                        message.Parameters.Add(coordinateX);
                        var coordinateY = new ProtocolParameter("y", y);
                        message.Parameters.Add(coordinateY);

                        break;

                    case ClientCommand.ATTACK:

                        message.Command = Command.ATTACK;
                        break;
                }

                var payload = new Message(message);
                MessageHandler.SendMessage(socket, payload);
                return true;
            }
            catch (SocketException)
            {
                Console.WriteLine("The server is down!!");
                return false;
            }
        }
    }
}
