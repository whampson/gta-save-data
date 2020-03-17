using Newtonsoft.Json;
using System;
using WpfEssentials;

namespace GTASaveData.Serialization
{
    /// <summary>
    /// Represents an arbitrary data structure stored in a <i>Grand Theft Auto</i> save file.
    /// </summary>
    public abstract class SerializableObject : ObservableObject, ISerializable
    {
        /// <summary>
        /// Popualates this object's fields by reading data from the specified <see cref="Serializer"/>.
        /// </summary>
        /// <param name="r">The data input stream.</param>
        /// <param name="fmt">The data format for reading platform-specific data.</param>
        protected abstract void ReadObjectData(Serializer r, FileFormat fmt);

        /// <summary>
        /// Writes this object's data to the specified <see cref="Serializer"/>.
        /// </summary>
        /// <param name="w">The data output stream.</param>
        /// <param name="fmt">The data format for writing platform-specific data.</param>
        protected abstract void WriteObjectData(Serializer w, FileFormat fmt);

        void ISerializable.ReadObjectData(Serializer r, FileFormat fmt)
        {
#if DEBUG
            r.Mark();
#endif
            ReadObjectData(r, fmt);
        }

        void ISerializable.WriteObjectData(Serializer w, FileFormat fmt)
        {
#if DEBUG
            w.Mark();
#endif
            WriteObjectData(w, fmt);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static int SizeOf<T>() where T : SerializableObject
        {
            SizeAttribute sizeAttr = (SizeAttribute) Attribute.GetCustomAttribute(typeof(T), typeof(SizeAttribute));
            if (sizeAttr == null)
            {
                throw new InvalidOperationException(string.Format("Cannot use SizeOf() on type {0}.", typeof(T)));
            }

            return sizeAttr.Size;
        }
    }
}
