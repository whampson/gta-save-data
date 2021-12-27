using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class ObjectPool : SaveDataObject,
        IEquatable<ObjectPool>, IDeepClonable<ObjectPool>,
        IEnumerable<PhysicalObject>
    {
        private ObservableArray<PhysicalObject> m_objects;

        public ObservableArray<PhysicalObject> Objects
        {
            get { return m_objects; }
            set { m_objects = value; OnPropertyChanged(); }
        }

        public PhysicalObject this[int i]
        {
            get { return Objects[i]; }
            set { Objects[i] = value; OnPropertyChanged(); }
        }

        public ObjectPool()
        {
            Objects = new ObservableArray<PhysicalObject>();
        }

        public ObjectPool(ObjectPool other)
        {
            Objects = ArrayHelper.DeepClone(other.Objects);
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            int numObjects = buf.ReadInt32();
            Objects = buf.ReadArray<PhysicalObject>(numObjects, prm);

            Debug.Assert(buf.Offset == SizeOf(this, prm));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            buf.Write(Objects.Count);
            buf.Write(Objects, prm);

            Debug.Assert(buf.Offset == SizeOf(this, prm));
        }

        protected override int GetSize(SerializationParams prm)
        {
            return (SizeOf<PhysicalObject>(prm) * Objects.Count) + sizeof(int);
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

        public IEnumerator<PhysicalObject> GetEnumerator()
        {
            return Objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
