using GTASaveData.Interfaces;
using System;
using System.Linq;

namespace GTASaveData
{
    /// <summary>
    /// A serializable placeholder object.
    /// </summary>
    public class Dummy : BufferedObject,
        IEquatable<Dummy>, IDeepClonable<Dummy>
    {
        public Dummy() : this(0) { }
        public Dummy(int size) : base(size) { }
        public Dummy(Dummy other) : base(other) { }

        protected override void ReadData(DataBuffer buf, SerializationParams p)
        {
            FillWorkBuffer(buf);
        }

        protected override void WriteData(DataBuffer buf, SerializationParams p)
        {
            WriteWorkBuffer(buf);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Dummy);
        }

        public bool Equals(Dummy other)
        {
            if (other == null)
            {
                return false;
            }

            return WorkBuffer.SequenceEqual(other.WorkBuffer);
        }

        public Dummy DeepClone()
        {
            return new Dummy(this);
        }
    }
}
