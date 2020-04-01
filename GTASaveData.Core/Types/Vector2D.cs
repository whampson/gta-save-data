using System;
using System.Numerics;

namespace GTASaveData.Types
{
    /// <summary>
    /// A 2-dimensional vector.
    /// </summary>
    [Size(8)]
    public class Vector2D : SaveDataObject, IEquatable<Vector2D>
    {
        private float m_x;
        private float m_y;

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
        public Vector2D()
            : this(0, 0)
        { }

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            X = buf.ReadSingle();
            Y = buf.ReadSingle();
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(X);
            buf.Write(Y);
        }

        public override string ToString()
        {
            return string.Format("<{0:0.###},{1:0.###}>", X, Y);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vector2D);
        }

        public bool Equals(Vector2D other)
        {
            if (other == null)
            {
                return false;
            }

            return X.Equals(other.X)
                && Y.Equals(other.Y);
        }

        public static implicit operator Vector2(Vector2D v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static implicit operator Vector2D(Vector2 v)
        {
            return new Vector2D(v.X, v.Y);
        }
    }
}
