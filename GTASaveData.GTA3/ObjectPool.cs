using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class ObjectPool : SaveDataObject, IEquatable<ObjectPool>
    {
        private Array<GameObject> m_items;

        public Array<GameObject> Items
        {
            get { return m_items; }
            set { m_items = value; OnPropertyChanged(); }
        }

        public GameObject this[int i]
        {
            get { return Items[i]; }
            set { Items[i] = value; OnPropertyChanged(); }
        }

        public ObjectPool()
        {
            Items = new Array<GameObject>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int numObjects = buf.ReadInt32();
            Items = buf.Read<GameObject>(numObjects);

            Debug.Assert(buf.Offset == SizeOf(this));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Items.Count);
            buf.Write(Items.ToArray());

            Debug.Assert(buf.Offset == SizeOf(this));
        }

        protected override int GetSize(DataFormat fmt)
        {
            return (SizeOf<GameObject>(fmt) * Items.Count) + sizeof(int);
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

            return Items.SequenceEqual(other.Items);
        }
    }
}
