using System;

namespace GTASaveData.Types
{
    /// <summary>
    /// A 3-dimensional vector.
    /// </summary>
    public class Vector3D : SaveDataObject,
        IEquatable<Vector3D>, IDeepClonable<Vector3D>
    {
        private const int Size = 12;

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

        public Vector3D(Vector3D other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;
        }

        public float GetHeading()
        {
            return (float) Math.Atan2(-X, Y);
        }

        public float GetMagnitude()
        {
            return (float) Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }

        public float GetMagnitudeSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z);
        }

        public Vector2D Get2DComponent()
        {
            return new Vector2D(X, Y);
        }

        public Vector3D Normalize()
        {
            return Normalize(1.0f);
        }

        public Vector3D Normalize(float norm)
        {
            float magSq = GetMagnitudeSquared();
            if (magSq > 0)
            {
                float invSq = (float) (norm / Math.Sqrt(magSq));
                X *= invSq;
                Y *= invSq;
                Z *= invSq;
            }

            return this;
        }

        public static float Dot(Vector3D v1, Vector3D v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        public static Vector3D Cross(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(
                (v1.Y * v2.Z) - (v1.Z * v2.Y),
                (v1.Z * v2.X) - (v1.X * v2.Z),
                (v1.X * v2.Y) - (v1.Y * v2.X));
        }

        public static float Distance(Vector3D v1, Vector3D v2)
        {
            return (v2 - v1).GetMagnitude();
        }

        public static Vector3D operator -(Vector3D v)
        {
            return new Vector3D(-v.X, -v.Y, -v.Z);
        }

        public static Vector3D operator +(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Vector3D operator -(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector3D operator *(Vector3D left, float right)
        {
            return new Vector3D(left.X * right, left.Y * right, left.Z * right);
        }

        public static Vector3D operator *(float left, Vector3D right)
        {
            return new Vector3D(left * right.X, left * right.Y, left * right.Z);
        }

        public static Vector3D operator /(Vector3D left, float right)
        {
            return new Vector3D(left.X / right, left.Y / right, left.Z / right);
        }

        public static bool operator ==(Vector3D left, Vector3D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector3D left, Vector3D right)
        {
            return !left.Equals(right);
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            X = buf.ReadFloat();
            Y = buf.ReadFloat();
            Z = buf.ReadFloat();
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(X);
            buf.Write(Y);
            buf.Write(Z);
        }

        protected override int GetSize(FileFormat fmt)
        {
            return Size;
        }

        public override string ToString()
        {
            return $"{X:0.###},{Y:0.###},{Z:0.###}";
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash += 23 * X.GetHashCode();
            hash += 23 * Y.GetHashCode();
            hash += 23 * Z.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3D))
            {
                return false;
            }

            return Equals((Vector3D) obj);
        }

        public bool Equals(Vector3D other)
        {
            return X.Equals(other.X)
                && Y.Equals(other.Y)
                && Z.Equals(other.Z);
        }

        public Vector3D DeepClone()
        {
            return new Vector3D(this);
        }
    }
}
