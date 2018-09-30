using System;
using System.Runtime.Serialization;

namespace OblPR.Game
{
    [Serializable]
    internal class InvalidMoveException : GameException
    {
        public InvalidMoveException()
        {
        }

        public InvalidMoveException(string message) : base(message)
        {
        }

        public InvalidMoveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidMoveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}