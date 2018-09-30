using Newtonsoft.Json;
using System.Text;

namespace OblPR.Protocol
{
    public class Message
    {
        private readonly byte[] _payload;
        private readonly ProtocolMessage _pmessage;

        public Message(byte[] payload)
        {
            _payload = payload;
            _pmessage = JsonConvert.DeserializeObject<ProtocolMessage>(
                Encoding.UTF8.GetString(_payload)
                );
        }

        public Message(ProtocolMessage message)
        {
            var jsonString = JsonConvert.SerializeObject(message);
            _payload = Encoding.UTF8.GetBytes(jsonString);
        }

        public byte[] Payload => _payload;

        public int Size => _payload.Length;

        public ProtocolMessage PMessage
        {
            get { return _pmessage; }
        }
    }
}
