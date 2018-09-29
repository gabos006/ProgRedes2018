using System;
using System.Runtime.Serialization;

namespace OblPR.Data.Services
{
    [Serializable]
    public class PlayerExistsException : Exception
    {
        public PlayerExistsException()
        {
        }

        public PlayerExistsException(string message) : base(message)
        {
        }

        public PlayerExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PlayerExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}