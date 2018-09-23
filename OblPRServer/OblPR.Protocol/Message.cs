using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Protocol
{
    public class Message
    {
        public Message(int command, byte[] payload)
        {
            Command = command;
            Payload = payload;
        }

        public int Command { get; private set; }
        public byte[] Payload { get; private set; }

        public int Size
        {
            get
            {
                return (this.Payload == null) ? 0 : Payload.Length;
            }
        }

    }
}
