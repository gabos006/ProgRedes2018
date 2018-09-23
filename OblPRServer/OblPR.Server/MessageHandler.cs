using System;
using System.Collections.Generic;
using System.Net.Sockets;
using OblPR.Protocol;

namespace OblPR.Server
{
    public  class MessageHandler
    {
        public  void Send(Socket socket, Message message)
        {
            SendPayloadSize(socket, message);
            SendMessage(socket, message);
        }

        private  void SendMessage(Socket socket, Message message)
        {
            var head = 0;
            var current = 0;

            var data = new List<byte>();

            data.AddRange(BitConverter.GetBytes(message.Command));

            if (message.Size > 0)
            {
                data.AddRange(message.Payload);
            }

            while (head < data.Count)
            {
                current += socket.Send(data.ToArray(), head, data.Count - head, SocketFlags.None);
                if (current == 0)
                    throw new SocketException();
                head += current;
            }
        }

        private  void SendPayloadSize(Socket socket, Message message)
        {
            var head = 0;
            var current = 0;

            var sizePackage = BitConverter.GetBytes(message.Size);

            while (head < sizePackage.Length)
            {
                current += socket.Send(sizePackage, head, sizePackage.Length - head, SocketFlags.None);
                if (current == 0)
                    throw new SocketException();
                head += current;
            }
        }
    }
}
