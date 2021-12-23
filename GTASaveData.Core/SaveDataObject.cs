using GTASaveData.Interfaces;
using Newtonsoft.Json;
using System;
using WpfEssentials;

namespace GTASaveData
{
    /// <summary>
    /// A generic object found in a <i>Grand Theft Auto</i> save data file.
    /// </summary>
    public abstract class SaveDataObject : ObservableObject, ISaveDataObject
    {
        /// <summary>
        /// Deserializes a JSON string into a <see cref="SaveDataObject"/>.
        /// </summary>
        /// <typeparam name="T">The type to deserialize.</typeparam>
        /// <param name="json">The JSON string to parse.</param>
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
        /// Serializes this object into a JSON string.
        /// </summary>
        /// <param name="formatting">The JSON formatting.</param>
        /// <returns>The object as a JSON string.</returns>
        public string ToJsonString(Formatting formatting)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        /// <summary>
        /// Called immediately before <see cref="ReadData(DataBuffer, FileType)"/> is executed.
        /// </summary>
        protected virtual void OnReading(FileType fmt) { }

        /// <summary>
        /// Called immediately after <see cref="ReadData(DataBuffer, FileType)"/> is executed.
        /// </summary>
        protected virtual void OnRead(FileType fmt) { }

        /// <summary>
        /// Called immediately before <see cref="WriteData(DataBuffer, FileType)"/> is executed.
        /// </summary>
        protected virtual void OnWriting(FileType fmt) { }

        /// <summary>
        /// Called immediately after <see cref="WriteData(DataBuffer, FileType)"/> is executed.
        /// </summary>
        protected virtual void OnWrite(FileType fmt) { }

        /// <summary>
        /// Populates this object's fields by deserializing its data from a 
        /// <see cref="DataBuffer"/> using the specified <see cref="FileType"/>
        /// to control how data is read.
        /// </summary>
        /// <remarks>
        /// This method is called by <see cref="ISaveDataObject.ReadData(DataBuffer, FileType)"/>;
        /// it should not be called directly.
        /// </remarks>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="fmt">The <see cref="FileType"/> controlling how data is read.</param>
        /// <returns>The number of bytes read.</returns>
        protected abstract void ReadData(DataBuffer buf, FileType fmt);

        /// <summary>
        /// Serializes the object by writing its fields to a <see cref="DataBuffer"/>
        /// using the specified <see cref="FileType"/> to control how data is written.
        /// </summary>
        /// <remarks>
        /// This method is called by <see cref="ISaveDataObject.WriteData(DataBuffer, FileType)"/>;
        /// it should not be called directly.
        /// </remarks>
        /// <param name="buf">The buffer to write to.</param>
        /// <param name="fmt">The <see cref="FileType"/> controlling how data is written.</param>
        /// <returns>The number of bytes written.</returns>
        protected abstract void WriteData(DataBuffer buf, FileType fmt);

        /// <summary>
        /// Gets the size in bytes of this object.
        /// </summary>
        /// <param name="fmt">The <see cref="FileType"/> controlling how data is written, which may affect the size.</param>
        /// <returns>The size in bytes of the serialized object.</returns>
        protected abstract int GetSize(FileType fmt);

        int ISaveDataObject.ReadData(DataBuffer buf, FileType fmt)
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

        int ISaveDataObject.WriteData(DataBuffer buf, FileType fmt)
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

        int ISaveDataObject.GetSize(FileType fmt)
        {
            return GetSize(fmt);
        }

        /// <summary>
        /// Gets the size of a type when serialized.
        /// </summary>
        /// <typeparam name="T">The type to get the size of.</typeparam>
        protected static int SizeOf<T>() where T : new()
        {
            return Serializer.SizeOf<T>();
        }

        /// <summary>
        /// Gets the size of a type when serialized.
        /// </summary>
        /// <typeparam name="T">The type to get the size of.</typeparam>
        /// <param name="fmt">A <see cref="FileType"/> descriptor controlling how data is written, which may affect the size.</param>
        protected static int SizeOf<T>(FileType fmt) where T : new()
        {
            return Serializer.SizeOf<T>(fmt);
        }

        /// <summary>
        /// Gets the size of an object when serialized.
        /// </summary>
        /// <param name="obj">The object to get the size of.</param>
        protected static int SizeOf<T>(T obj)
        {
            return Serializer.SizeOf(obj);
        }

        /// <summary>
        /// Gets the size of an object when serialized.
        /// </summary>
        /// <param name="obj">The object to get the size of.</param>
        /// <param name="fmt">A <see cref="FileType"/> descriptor controlling how data is written, which may affect the size.</param>
        protected static int SizeOf<T>(T obj, FileType fmt)
        {
            return Serializer.SizeOf(obj, fmt);
        }

        /// <summary>
        /// Returns an exception for scenarios when there is no
        /// defined size for the specified file format.
        /// </summary>
        /// <param name="fmt">The file format to use.</param>
        /// <returns>An <see cref="InvalidOperationException"/>.</returns>
        protected InvalidOperationException SizeNotDefined(FileType fmt)
        {
            return new InvalidOperationException(string.Format(Strings.Error_InvalidOperation_SizeNotDefined, fmt.DisplayName));
        }

        /// <summary>
        /// Rounds an address up to the next multiple of 4.
        /// </summary>
        public int Align4(int addr)
        {
            return DataBuffer.Align4(addr);
        }
    }
}
