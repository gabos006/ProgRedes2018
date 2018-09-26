using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OblPR.Protocol
{
    public class Message
    {
        private byte[] _payload;
        private ProtocolMessage _pmessage;

        public Message(byte[] payload)
        {
            _payload = payload;
            _pmessage = JsonConvert.DeserializeObject<ProtocolMessage>(
                Convert.ToBase64String(_payload)
                );
        }

        public Message(ProtocolMessage message)
        {
            var jsonString = JsonConvert.SerializeObject(message);
            _payload = Convert.FromBase64String(jsonString);
        }

        public byte[] Payload => _payload;

        public int Size => _payload.Length;
    }
}
