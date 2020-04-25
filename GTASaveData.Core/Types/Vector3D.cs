using System;
using System.Numerics;

namespace GTASaveData.Types
{
    /// <summary>
    /// A 3-dimensional vector.
    /// </summary>
    [Size(12)]
    public class Vector3D : SaveDataObject, IEquatable<Vector3D>
    {
        private float m_x;
        private float m_y;
        private float m_z;

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

        public Vector3D()
            : this(0, 0, 0)
        { }

        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            X = buf.ReadFloat();
            Y = buf.ReadFloat();
            Z = buf.ReadFloat();
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(X);
            buf.Write(Y);
            buf.Write(Z);
        }

        public override string ToString()
        {
            return string.Format("<{0:0.###},{1:0.###},{2:0.###}>", X, Y, Z);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vector3D);
        }

        public bool Equals(Vector3D other)
        {
            if (other == null)
            {
                return false;
            }

            return X.Equals(other.X)
                && Y.Equals(other.Y)
                && Z.Equals(other.Z);
        }

        public static implicit operator Vector3(Vector3D v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static implicit operator Vector3D(Vector3 v)
        {
            return new Vector3D(v.X, v.Y, v.Z);
        }
    }
}
