using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class ParticleData : SaveDataObject,
        IEquatable<ParticleData>, IDeepClonable<ParticleData>
    {
        private Array<ParticleObject> m_particleObjects;

        public Array<ParticleObject> ParticleObjects
        {
            get { return m_particleObjects; }
            set { m_particleObjects = value; OnPropertyChanged(); }
        }

        public ParticleData()
        {
            ParticleObjects = new Array<ParticleObject>();
        }

        public ParticleData(ParticleData other)
        {
            ParticleObjects = ArrayHelper.DeepClone(other.ParticleObjects);
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            int numObjects = buf.ReadInt32();
            ParticleObjects = buf.Read<ParticleObject>(numObjects, fmt);
            buf.Skip(SizeOfType<ParticleObject>(fmt));

            Debug.Assert(buf.Offset == SizeOfObject(this, fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            int numObjects = ParticleObjects.Count;
            buf.Write(numObjects);
            buf.Write(ParticleObjects.ToArray(), fmt, numObjects);
            buf.Skip(SizeOfType<ParticleObject>(fmt));  // game writes extra ParticleObject for some reason

            Debug.Assert(buf.Offset == SizeOfObject(this, fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            return (SizeOfType<ParticleObject>(fmt) * (ParticleObjects.Count + 1)) + sizeof(int);
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

            return ParticleObjects.SequenceEqual(other.ParticleObjects);
        }

        public ParticleData DeepClone()
        {
            return new ParticleData(this);
        }
    }
}
