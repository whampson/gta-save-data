using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class ObjectPool : SaveDataObject, IEquatable<ObjectPool>
    {
        private Array<GameObject> m_objects;

        public Array<GameObject> Objects
        {
            get { return m_objects; }
            set { m_objects = value; OnPropertyChanged(); }
        }

        public ObjectPool()
        {
            Objects = new Array<GameObject>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int numObjects = buf.ReadInt32();
            Objects = buf.Read<GameObject>(numObjects);

            Debug.Assert(buf.Offset == SizeOf(this));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Objects.Count);
            buf.Write(Objects.ToArray());

            Debug.Assert(buf.Offset == SizeOf(this));
        }

        protected override int GetSize(DataFormat fmt)
        {
            return (SizeOf<GameObject>(fmt) * Objects.Count) + sizeof(int);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ObjectPool);
        }

        public bool Equals(ObjectPool other)
        {
            if (other == null)
            {
                return false;
            }

            return Objects.SequenceEqual(other.Objects);
        }
    }
}
