using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class ObjectPool : SaveDataObject,
        IEquatable<ObjectPool>, IDeepClonable<ObjectPool>
    {
        private Array<GameObject> m_objects;

        public Array<GameObject> Objects
        {
            get { return m_objects; }
            set { m_objects = value; OnPropertyChanged(); }
        }

        public GameObject this[int i]
        {
            get { return Objects[i]; }
            set { Objects[i] = value; OnPropertyChanged(); }
        }

        public ObjectPool()
        {
            Objects = new Array<GameObject>();
        }

        public ObjectPool(ObjectPool other)
        {
            Objects = ArrayHelper.DeepClone(other.Objects);
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            int numObjects = buf.ReadInt32();
            Objects = buf.Read<GameObject>(numObjects, fmt);

            Debug.Assert(buf.Offset == SizeOfObject(this, fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(Objects.Count);
            buf.Write(Objects, fmt);

            Debug.Assert(buf.Offset == SizeOfObject(this, fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            return (SizeOfType<GameObject>(fmt) * Objects.Count) + sizeof(int);
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

        public ObjectPool DeepClone()
        {
            return new ObjectPool(this);
        }
    }
}
