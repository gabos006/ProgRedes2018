using System;
using System.Runtime.Serialization;

namespace OblPR.Data.Services
{
    [Serializable]
    public class PlayerInUseException : Exception
    {
        public PlayerInUseException()
        {
        }

        public PlayerInUseException(string message) : base(message)
        {
        }

        public PlayerInUseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PlayerInUseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}