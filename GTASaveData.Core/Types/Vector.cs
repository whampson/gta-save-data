using Newtonsoft.Json;
using System;

namespace GTASaveData.Types
{
    /// <summary>
    /// Represents a 3-dimensional vector.
    /// </summary>
    [Size(12)]
    public class Vector : SaveDataObject, IEquatable<Vector>
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

        [JsonIgnore]
        public float Heading
        {
            get { return (float) Math.Atan2(-X, Y); }
        }

        [JsonIgnore]
        public float Magnitude
        {
            get { return (float) Math.Sqrt(MagnitudeSquared); }
        }

        [JsonIgnore]
        public float MagnitudeSquared
        {
            get { return (X * X) + (Y * Y) + (Z * Z); }
        }

        [JsonIgnore]
        public float Magnitude2D
        {
            get { return (float) Math.Sqrt(MagnitudeSquared2D); }
        }

        [JsonIgnore]
        public float MagnitudeSquared2D
        {
            get { return (X * X) + (Y * Y); }
        }

        public Vector()
            : this(0, 0, 0)
        { }

        public Vector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Normalize()
        {
            float magSquared = MagnitudeSquared;
            if (magSquared > 0)
            {
                float invSqrt = (float) (1.0 / Math.Sqrt(magSquared));
                X *= invSqrt;
                Y *= invSqrt;
                Z *= invSqrt;
            }
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            X = buf.ReadSingle();
            Y = buf.ReadSingle();
            Z = buf.ReadSingle();
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
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
            return Equals(obj as Vector);
        }

        public bool Equals(Vector other)
        {
            if (other == null)
            {
                return false;
            }

            return X.Equals(other.X)
                && Y.Equals(other.Y)
                && Z.Equals(other.Z);
        }

        public static float Distance(Vector v1, Vector v2)
        {
            return (v2 - v1).Magnitude;
        }

        public static float DotProduct(Vector v1, Vector v2)
        {
            return (v1.X * v2.X)
                 + (v1.Y * v2.Y)
                 + (v1.Z * v2.Z);
        }

        public static Vector CrossProduct(Vector v1, Vector v2)
        {
            return new Vector(
                (v1.Y * v2.Z) - (v1.Z * v2.Y),
                (v1.Z * v2.X) - (v1.X * v2.Z),
                (v1.X * v2.Y) - (v1.Y * v2.X));
        }

        public static Vector operator -(Vector v)
        {
            return new Vector(-v.X, -v.Y, -v.Z);
        }

        public static Vector operator +(Vector left, Vector right)
        {
            return new Vector(
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z);
        }

        public static Vector operator -(Vector left, Vector right)
        {
            return new Vector(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z);
        }

        public static Vector operator *(Vector left, float right)
        {
            return new Vector(
                left.X * right,
                left.Y * right,
                left.Z * right);
        }

        public static Vector operator *(float left, Vector right)
        {
            return new Vector(
                left * right.X,
                left * right.Y,
                left * right.Z);
        }

        public static Vector operator /(Vector left, float right)
        {
            return new Vector(
                left.X / right,
                left.Y / right,
                left.Z / right);
        }
    }
}
