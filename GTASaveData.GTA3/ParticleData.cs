using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class ParticleData : SaveDataObject,
        IEquatable<ParticleData>, IDeepClonable<ParticleData>,
        IEnumerable<ParticleObject>
    {
        private ObservableArray<ParticleObject> m_objects;

        public ObservableArray<ParticleObject> Objects
        {
            get { return m_objects; }
            set { m_objects = value; OnPropertyChanged(); }
        }

        public ParticleData()
        {
            Objects = new ObservableArray<ParticleObject>();
        }

        public ParticleData(ParticleData other)
        {
            Objects = ArrayHelper.DeepClone(other.Objects);
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            int numObjects = buf.ReadInt32();
            Objects = buf.ReadArray<ParticleObject>(numObjects, prm);
            buf.Skip(SizeOf<ParticleObject>(prm));

            Debug.Assert(buf.Offset == SizeOf(this, prm));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            int numObjects = Objects.Count;
            buf.Write(numObjects);
            buf.Write(Objects, prm, numObjects);
            buf.Skip(SizeOf<ParticleObject>(prm));  // game writes extra ParticleObject for some reason

            Debug.Assert(buf.Offset == SizeOf(this, prm));
        }

        protected override int GetSize(SerializationParams prm)
        {
            return (SizeOf<ParticleObject>(prm) * (Objects.Count + 1)) + sizeof(int);
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
