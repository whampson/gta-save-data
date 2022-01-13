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
        /// Called immediately before <see cref="ReadData(DataBuffer, SerializationParams)"/> is executed.
        /// </summary>
        protected virtual void OnReading(SerializationParams p) { }

        /// <summary>
        /// Called immediately after <see cref="ReadData(DataBuffer, SerializationParams)"/> is executed.
        /// </summary>
        protected virtual void OnRead(SerializationParams p) { }

        /// <summary>
        /// Called immediately before <see cref="WriteData(DataBuffer, SerializationParams)"/> is executed.
        /// </summary>
        protected virtual void OnWriting(SerializationParams p) { }

        /// <summary>
        /// Called immediately after <see cref="WriteData(DataBuffer, SerializationParams)"/> is executed.
        /// </summary>
        protected virtual void OnWrite(SerializationParams p) { }

        /// <summary>
        /// Populates this object's fields by deserializing its data from a 
        /// <see cref="DataBuffer"/> using the specified <see cref="SerializationParams"/>
        /// to control how data is read.
        /// </summary>
        /// <remarks>
        /// This method is called by <see cref="ISaveDataObject.ReadData(DataBuffer, SerializationParams)"/>;
        /// it should not be called directly.
        /// </remarks>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="p">The <see cref="SerializationParams"/> controlling how data is read.</param>
        /// <returns>The number of bytes read.</returns>
        protected abstract void ReadData(DataBuffer buf, SerializationParams p);

        /// <summary>
        /// Serializes the object by writing its fields to a <see cref="DataBuffer"/>
        /// using the specified <see cref="SerializationParams"/> to control how data is written.
        /// </summary>
        /// <remarks>
        /// This method is called by <see cref="ISaveDataObject.WriteData(DataBuffer, SerializationParams)"/>;
        /// it should not be called directly.
        /// </remarks>
        /// <param name="buf">The buffer to write to.</param>
        /// <param name="p">The <see cref="SerializationParams"/> controlling how data is written.</param>
        /// <returns>The number of bytes written.</returns>
        protected abstract void WriteData(DataBuffer buf, SerializationParams p);

        /// <summary>
        /// Gets the size in bytes of this object.
        /// </summary>
        /// <param name="p">The <see cref="SerializationParams"/> controlling how data is written,
        /// which may affect the size.</param>
        /// <returns>The size in bytes of the serialized object.</returns>
        protected abstract int GetSize(SerializationParams p);

        int ISaveDataObject.ReadData(DataBuffer buf, SerializationParams p)
        {
            int oldMark, start, len;

            oldMark = buf.MarkedPosition;
            buf.Mark();
            start = buf.Position;

            OnReading(p);
            ReadData(buf, p);
            OnRead(p);

            len = buf.Position - start;
            buf.MarkedPosition = oldMark;

            return len;
        }

        int ISaveDataObject.WriteData(DataBuffer buf, SerializationParams p)
        {
            int oldMark, start, len;

            oldMark = buf.MarkedPosition;
            buf.Mark();
            start = buf.Position;

            OnWriting(p);
            WriteData(buf, p);
            OnWrite(p);

            len = buf.Position - start;
            buf.MarkedPosition = oldMark;

            return len;
        }

        int ISaveDataObject.GetSize(SerializationParams p)
        {
            return GetSize(p);
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
        /// <param name="p">A <see cref="SerializationParams"/> descriptor controlling how data is written,
        /// which may affect the size.</param>
        protected static int SizeOf<T>(SerializationParams p) where T : new()
        {
            return Serializer.SizeOf<T>(p);
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
        /// <param name="p">A <see cref="SerializationParams"/> descriptor controlling how data is written,
        /// which may affect the size.</param>
        protected static int SizeOf<T>(T obj, SerializationParams p)
        {
            return Serializer.SizeOf(obj, p);
        }

        /// <summary>
        /// Returns an exception for scenarios when there is no
        /// defined size for the specified file typr.
        /// </summary>
        protected InvalidOperationException SizeNotDefined(FileType t)
        {
            return new InvalidOperationException(string.Format(Strings.Error_InvalidOperation_SizeNotDefined, t.Id));
        }

        /// <summary>
        /// Rounds an address up to the next multiple of 4.
        /// </summary>
        protected int Align4(int addr)
        {
            return DataBuffer.Align4(addr);
        }
    }
}
