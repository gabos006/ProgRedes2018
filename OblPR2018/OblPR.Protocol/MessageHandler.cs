using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace OblPR.Protocol
{
    public static class MessageHandler
    {
        public static void SendMessage(Socket socket, Message message)
        {
            SendPayloadSize(socket, message);
            SendPayload(socket, message);
        }

        public static Message RecieveMessage(Socket socket)
        {
            var bufferSize = RecievePayloadSize(socket);
            var message = RecievePayload(socket, bufferSize);
            return message;

        }

        private static Message RecievePayload(Socket socket, int bufferSize)
        {
            var buffer = new byte[bufferSize];
            SocketTransfer(buffer, socket.Receive);
            return GetMessage(buffer);
        }

        private static Message GetMessage(byte[] buffer)
        {

            var message = new Message(buffer);
            return message;
        }

        private static int RecievePayloadSize(Socket socket)
        {
            var sizeBuf = new byte[sizeof(int)];
            SocketTransfer(sizeBuf, socket.Receive);
            return BitConverter.ToInt32(sizeBuf, 0);
        }

        private static void SendPayload(Socket socket, Message message)
        {
            var data = new List<byte>();
            if (message.Size > 0)
                data.AddRange(message.Payload);

            SocketTransfer(data.ToArray(), socket.Send);
        }

        private static void SendPayloadSize(Socket socket, Message message)
        {
            var sizePackage = BitConverter.GetBytes(message.Size);
            SocketTransfer(sizePackage, socket.Send);
        }

        private static void SocketTransfer(byte[] sizePackage, Func<byte[], int, int, SocketFlags, int> send)
        {
            var head = 0;
            var current = 0;

            while (head < sizePackage.Length)
            {
                current += send(sizePackage, 0, sizePackage.Length - head, 0);
                if (current == 0)
                    throw new SocketException();
                head += current;
            }
        }
    }
}
