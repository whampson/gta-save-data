using System;

namespace GTASaveData.Serialization
{
    /// <summary>
    /// Represents an error that occurs during save data serialization.
    /// </summary>
    public class SerializationException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="SerializationException"/> instance.
        /// </summary>
        public SerializationException()
        { }

        /// <summary>
        /// Creates a new <see cref="SerializationException"/> instance
        /// with the specified message.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public SerializationException(string message)
            : base(message)
        { }

        /// <summary>
        /// Creates a new <see cref="SerializationException"/> instance
        /// with the specified message.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="innerException">The <see cref="Exception"/> that caused the error.</param>
        public SerializationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
