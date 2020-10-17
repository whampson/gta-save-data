using System;

namespace GTASaveData.Types
{
    public class Quaternion : SaveDataObject,
        IEquatable<Quaternion>, IDeepClonable<Quaternion>
    {
        private const int Size = 16;

        private float m_x;
        private float m_y;
        private float m_z;
        private float m_w;

        public float X
        {
            get { return m_x; }
            set { m_x = value; OnPropertyChanged(); }
        }

        public float Y
        {
            get { return m_y; }
            set { m_y = value; OnPropertyChanged(); }
        }

        public float Z
        {
            get { return m_z; }
            set { m_z = value; OnPropertyChanged(); }
        }

        public float W
        {
            get { return m_w; }
            set { m_w = value; OnPropertyChanged(); }
        }
        public Quaternion()
            : this(0, 0, 0, 0)
        { }

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Quaternion(Quaternion other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;
            W = other.W;
        }

        public static bool operator ==(Quaternion q1, Quaternion q2)
        {
            return q1.Equals(q2);
        }

        public static bool operator !=(Quaternion q1, Quaternion q2)
        {
            return !q1.Equals(q2);
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            X = buf.ReadFloat();
            Y = buf.ReadFloat();
            Z = buf.ReadFloat();
            W = buf.ReadFloat();
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(X);
            buf.Write(Y);
            buf.Write(Z);
            buf.Write(W);
        }

        protected override int GetSize(FileFormat fmt)
        {
            return Size;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash += 23 * X.GetHashCode();
            hash += 23 * Y.GetHashCode();
            hash += 23 * Z.GetHashCode();
            hash += 23 * W.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Quaternion))
            {
                return false;
            }

            return Equals((Quaternion) obj);
        }

        public bool Equals(Quaternion other)
        {
            return X.Equals(other.X)
                && Y.Equals(other.Y)
                && Z.Equals(other.Z)
                && W.Equals(other.W);
        }

        public Quaternion DeepClone()
        {
            return new Quaternion(this);
        }
    }
}
