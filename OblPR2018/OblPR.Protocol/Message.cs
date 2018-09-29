using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OblPR.Protocol
{
    public class Message
    {


        private readonly byte[] _payload;
        private readonly AbstractPayload _pmessage;

        public Message(byte[] payload)
        {
            _payload = payload;

            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(_payload, 0, _payload.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                _pmessage = (AbstractPayload) binForm.Deserialize(memStream);
            }
        }

        public Message(AbstractPayload message)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, message);
                _payload = ms.ToArray();
            }
        }

        public byte[] Payload => _payload;

        public int Size => _payload.Length;

        public AbstractPayload PMessage
        {
            get { return _pmessage; }
        }
    }
}
