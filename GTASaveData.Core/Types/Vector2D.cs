using System;

namespace GTASaveData.Types
{
    /// <summary>
    /// A 2-dimensional vector.
    /// </summary>
    public class Vector2D : SaveDataObject,
        IEquatable<Vector2D>, IDeepClonable<Vector2D>
    {
        private const int Size = 8;

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

        public Vector2D(Vector2D other)
        {
            X = other.X;
            Y = other.Y;
        }

        public float GetHeading()
        {
            return (float) Math.Atan2(-X, Y);
        }

        public float GetMagnitude()
        {
            return (float) Math.Sqrt((X * X) + (Y * Y));
        }

        public float GetMagnitudeSquared()
        {
            return (X * X) + (Y * Y);
        }

        public Vector2D Normalize()
        {
            return Normalize(1.0f);
        }

        public Vector2D Normalize(float norm)
        {
            float mag = GetMagnitude();
            if (mag > 0)
            {
                float invSq = norm / mag;
                X *= invSq;
                Y *= invSq;
            }

            return this;
        }

        public static float Dot(Vector2D v1, Vector2D v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y);
        }

        public static float Distance(Vector2D v1, Vector2D v2)
        {
            return (v2 - v1).GetMagnitude();
        }

        public static Vector2D operator -(Vector2D v)
        {
            return new Vector2D(-v.X, -v.Y);
        }

        public static Vector2D operator +(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.X + right.X, left.Y + right.Y);
        }

        public static Vector2D operator -(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.X - right.X, left.Y - right.Y);
        }

        public static Vector2D operator *(Vector2D left, float right)
        {
            return new Vector2D(left.X * right, left.Y * right);
        }

        public static Vector2D operator *(float left, Vector2D right)
        {
            return new Vector2D(left * right.X, left * right.Y);
        }

        public static Vector2D operator /(Vector2D left, float right)
        {
            return new Vector2D(left.X / right, left.Y / right);
        }

        public static bool operator ==(Vector2D v1, Vector2D v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector2D v1, Vector2D v2)
        {
            return !v1.Equals(v2);
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            X = buf.ReadFloat();
            Y = buf.ReadFloat();
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(X);
            buf.Write(Y);
        }

        protected override int GetSize(FileFormat fmt)
        {
            return Size;
        }

        public override string ToString()
        {
            return $"{X:0.###},{Y:0.###}";
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash += 23 * X.GetHashCode();
            hash += 23 * Y.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2D))
            {
                return false;
            }

            return Equals((Vector2D) obj);
        }

        public bool Equals(Vector2D other)
        {
            return X.Equals(other.X)
                && Y.Equals(other.Y);
        }

        public Vector2D DeepClone()
        {
            return new Vector2D(this);
        }
    }
}
