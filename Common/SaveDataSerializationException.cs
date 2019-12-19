using System;

namespace GTASaveData
{
    /// <summary>
    /// Thrown when an error occurs object serialization.
    /// </summary>
    public class SaveDataSerializationException : Exception
    {
        public SaveDataSerializationException()
        { }

        public SaveDataSerializationException(string message)
            : base(message)
        { }

        public SaveDataSerializationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
