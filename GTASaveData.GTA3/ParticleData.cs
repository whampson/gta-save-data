using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class ParticleData : SaveDataObject, IEquatable<ParticleData>
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

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int numObjects = buf.ReadInt32();
            ParticleObjects = buf.Read<ParticleObject>(numObjects, fmt);
            buf.Skip(SizeOf<ParticleObject>(fmt));

            Debug.Assert(buf.Offset == SizeOf(this, fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int numObjects = ParticleObjects.Count;
            buf.Write(numObjects);
            buf.Write(ParticleObjects.ToArray(), fmt, numObjects);
            buf.Skip(SizeOf<ParticleObject>(fmt));  // game writes extra ParticleObject for some reason

            Debug.Assert(buf.Offset == SizeOf(this, fmt));
        }

        protected override int GetSize(DataFormat fmt)
        {
            return SizeOf<ParticleObject>(fmt) * (ParticleObjects.Count + 1) + 4;
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
    }
}
