using System;

namespace GTASaveData
{
    /// <summary>
    /// Represents an error that can occur during the serialization of save data.
    /// </summary>
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
