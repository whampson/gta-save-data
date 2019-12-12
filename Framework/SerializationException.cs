using System;

namespace GTASaveData.Common
{
    /// <summary>
    /// Thrown when something goes wrong during object serialization or deserialization.
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
    }
}
