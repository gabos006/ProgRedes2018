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
                        message.Command = Command.MOVE;
                        Console.Write("Insert coordinate x: ");
                        var coordinateX = new ProtocolParameter("x", HandleInput());
                        Console.Write("Insert coordinate y: ");
                        var coordinateY = new ProtocolParameter("y", HandleInput());

                        message.Parameters.Add(coordinateX);
                        message.Parameters.Add(coordinateY);
                        break;

                    case ClientCommand.ATTACK:

                        message.Command = Command.ATTACK;
                        break;

                    case ClientCommand.EXIT_GAME:

                        message.Command = Command.MATCH_END;
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

        private string HandleInput()
        {
            var isOptionValid = false;
            while (!isOptionValid)
            {
                try
                {
                    var input = int.Parse(Console.ReadLine().Trim());
                    return input.ToString();
                }
                catch
                {
                    isOptionValid = false;
                    Console.WriteLine("Input has to be a number");
                }
            }
            return null;
        }
    }
}
