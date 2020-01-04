using System;

namespace GTASaveData.Serialization
{
    /// <summary>
    /// Represents an error that occurs during object serialization.
    /// </summary>
    public class SaveDataSerializationException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="SaveDataSerializationException"/> instance.
        /// </summary>
        public SaveDataSerializationException()
        { }

        /// <summary>
        /// Creates a new <see cref="SaveDataSerializationException"/> instance
        /// with the specified message.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public SaveDataSerializationException(string message)
            : base(message)
        { }

        /// <summary>
        /// Creates a new <see cref="SaveDataSerializationException"/> instance
        /// with the specified message.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="innerException">The <see cref="Exception"/> that caused the error.</param>
        public SaveDataSerializationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
