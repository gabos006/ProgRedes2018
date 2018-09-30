using System;
using System.Runtime.Serialization;

namespace OblPR.Game
{
    [Serializable]
    internal class NoCellAvailableException : Exception
    {
        public NoCellAvailableException()
        {
        }

        public NoCellAvailableException(string message) : base(message)
        {
        }

        public NoCellAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoCellAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}