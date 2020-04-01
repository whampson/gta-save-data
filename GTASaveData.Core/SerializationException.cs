using System;

namespace GTASaveData
{
    public class SerializationException : Exception
    {
        public SerializationException()
        { }

        public SerializationException(string message)
            : base(message)
        { }

        public SerializationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public SerializationException(string msgFmt, params object[] args)
            : base (string.Format(msgFmt, args))
        { }
    }
}
