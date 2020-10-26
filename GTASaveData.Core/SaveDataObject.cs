using GTASaveData.Interfaces;
using Newtonsoft.Json;
using System;
using WpfEssentials;

namespace GTASaveData
{
    /// <summary>
    /// Represents a data structure found in a savedata file.
    /// </summary>
    public abstract class SaveDataObject : ObservableObject, ISaveDataObject
    {
        /// <summary>
        /// Deserializes a JSON string into an object.
        /// </summary>
        /// <typeparam name="T">The resulting data type.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static T FromJsonString<T>(string json) where T : SaveDataObject
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Serializes this object into a JSON string.
        /// </summary>
        /// <returns>The object as a JSON string.</returns>
        public string ToJsonString()
        {
            return ToJsonString(Formatting.Indented);
        }

        /// <summary>
        /// Serializes this object into a JSON string with the
        /// specified JSON formatting.
        /// </summary>
        /// <param name="formatting">The JSON formatting.</param>
        /// <returns>The object as a JSON string.</returns>
        public string ToJsonString(Formatting formatting)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        /// <summary>
        /// Event handler executed before <see cref="ReadData(DataBuffer, FileFormat)"/> is called.
        /// </summary>
        protected virtual void OnReading(FileFormat fmt) { }

        /// <summary>
        /// Event handler executed after <see cref="ReadData(DataBuffer, FileFormat)"/> is called.
        /// </summary>
        protected virtual void OnRead(FileFormat fmt) { }

        /// <summary>
        /// Event handler executed before <see cref="WriteData(DataBuffer, FileFormat)"/> is called.
        /// </summary>
        protected virtual void OnWriting(FileFormat fmt) { }

        /// <summary>
        /// Event handler executed after <see cref="WriteData(DataBuffer, FileFormat)"/> is called.
        /// </summary>
        protected virtual void OnWrite(FileFormat fmt) { }

        /// <summary>
        /// Reads this object's data in from the stream buffer using the
        /// specified file format to define the manner in which data is read.
        /// </summary>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="fmt">The data format.</param>
        protected abstract void ReadData(DataBuffer buf, FileFormat fmt);

        /// <summary>
        /// Writes this object's data out to the stream buffer using the
        /// specified file format to define the manner in which data is written.
        /// </summary>
        /// <param name="buf">The buffer to write to.</param>
        /// <param name="fmt">The data format.</param>
        protected abstract void WriteData(DataBuffer buf, FileFormat fmt);

        /// <summary>
        /// Gets the size of this object's serialized data.
        /// </summary>
        /// <param name="fmt">The data format.</param>
        /// <returns>The size of the object in bytes.</returns>
        protected abstract int GetSize(FileFormat fmt);

        int ISaveDataObject.ReadData(DataBuffer buf, FileFormat fmt)
        {
            int oldMark, start, len;

            oldMark = buf.MarkedPosition;
            buf.Mark();
            start = buf.Position;

            OnReading(fmt);
            ReadData(buf, fmt);
            OnRead(fmt);

            len = buf.Position - start;
            buf.MarkedPosition = oldMark;

            return len;
        }

        int ISaveDataObject.WriteData(DataBuffer buf, FileFormat fmt)
        {
            int oldMark, start, len;

            oldMark = buf.MarkedPosition;
            buf.Mark();
            start = buf.Position;

            OnWriting(fmt);
            WriteData(buf, fmt);
            OnWrite(fmt);

            len = buf.Position - start;
            buf.MarkedPosition = oldMark;

            return len;
        }

        int ISaveDataObject.GetSize(FileFormat fmt)
        {
            return GetSize(fmt);
        }

        /// <summary>
        /// Gets the size of a type.
        /// </summary>
        /// <typeparam name="T">The tyoe to get the size of.</typeparam>
        /// <returns>The size in bytes of the type.</returns>
        protected static int SizeOfType<T>() where T : new()
        {
            return Serializer.SizeOfType<T>();
        }

        /// <summary>
        /// Gets the size of a type when serialized using the
        /// specified file format.
        /// </summary>
        /// <typeparam name="T">The type to get the size of.</typeparam>
        /// <param name="fmt">The file format to use.</param>
        /// <returns>The size in bytes of the type.</returns>
        protected static int SizeOfType<T>(FileFormat fmt) where T : new()
        {
            return Serializer.SizeOfType<T>(fmt);
        }

        /// <summary>
        /// Gets the size of an object when serialized.
        /// </summary>
        /// <typeparam name="T">The type to get the size of.</typeparam>
        /// <param name="obj">The object to get the size of.</param>
        /// <returns>The size in bytes of the object.</returns>
        protected static int SizeOfObject<T>(T obj)
        {
            return Serializer.SizeOfObject(obj);
        }

        /// <summary>
        /// Gets the size of an object when serialized using the
        /// specified file format.
        /// </summary>
        /// <typeparam name="T">The type to get the size of.</typeparam>
        /// <param name="obj">The object to get the size of.</param>
        /// <param name="fmt">The file format to use.</param>
        /// <returns>The size in bytes of the object.</returns>
        protected static int SizeOfObject<T>(T obj, FileFormat fmt)
        {
            return Serializer.SizeOfObject(obj, fmt);
        }

        /// <summary>
        /// Returns an exception for scenarios when there is no
        /// defined size for the specified file format.
        /// </summary>
        /// <param name="fmt">The file format to use.</param>
        /// <returns>An <see cref="InvalidOperationException"/>.</returns>
        protected InvalidOperationException SizeNotDefined(FileFormat fmt)
        {
            return new InvalidOperationException(string.Format(Strings.Error_InvalidOperation_SizeNotDefined, fmt.Name));
        }
    }
}
