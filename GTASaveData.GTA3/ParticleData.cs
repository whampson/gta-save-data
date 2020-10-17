using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class ParticleData : SaveDataObject,
        IEquatable<ParticleData>, IDeepClonable<ParticleData>,
        IEnumerable<ParticleObject>
    {
        private Array<ParticleObject> m_objects;

        public Array<ParticleObject> Objects
        {
            get { return m_objects; }
            set { m_objects = value; OnPropertyChanged(); }
        }

        public ParticleObject this[int i]
        {
            get { return Objects[i]; }
            set { Objects[i] = value; OnPropertyChanged(); }
        }

        public ParticleData()
        {
            Objects = new Array<ParticleObject>();
        }

        public ParticleData(ParticleData other)
        {
            Objects = ArrayHelper.DeepClone(other.Objects);
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            int numObjects = buf.ReadInt32();
            Objects = buf.Read<ParticleObject>(numObjects, fmt);
            buf.Skip(SizeOfType<ParticleObject>(fmt));

            Debug.Assert(buf.Offset == SizeOfObject(this, fmt));
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            int numObjects = Objects.Count;
            buf.Write(numObjects);
            buf.Write(Objects, fmt, numObjects);
            buf.Skip(SizeOfType<ParticleObject>(fmt));  // game writes extra ParticleObject for some reason

            Debug.Assert(buf.Offset == SizeOfObject(this, fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            return (SizeOfType<ParticleObject>(fmt) * (Objects.Count + 1)) + sizeof(int);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ParticleData);
        }

        public bool Equals(ParticleData other)
        {
            if (other == null)
            {
                return false;
            }

            return Objects.SequenceEqual(other.Objects);
        }

        public ParticleData DeepClone()
        {
            return new ParticleData(this);
        }

        public IEnumerator<ParticleObject> GetEnumerator()
        {
            return Objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
