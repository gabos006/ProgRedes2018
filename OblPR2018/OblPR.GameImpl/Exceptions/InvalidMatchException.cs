using System;
using System.Runtime.Serialization;

namespace OblPR.GameImpl
{
    [Serializable]
    internal class InvalidMatchException : Exception
    {
        public InvalidMatchException()
        {
        }

        public InvalidMatchException(string message) : base(message)
        {
        }

        public InvalidMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}