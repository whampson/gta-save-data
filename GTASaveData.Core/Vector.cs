using GTASaveData.Serialization;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace GTASaveData
{
    /// <summary>
    /// Represents a 3-dimensional vector.
    /// </summary>
    [Size(12)]
    public class Vector : SerializableObject,
        IEquatable<Vector>
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
            get { return (float) Math.Atan2(-m_x, m_y); }
        }

        [JsonIgnore]
        public float Magnitude
        {
            get { return (float) Math.Sqrt(MagnitudeSquared); }
        }

        [JsonIgnore]
        public float MagnitudeSquared
        {
            get { return (m_x * m_x) + (m_y * m_y) + (m_z * m_z); }
        }

        [JsonIgnore]
        public float Magnitude2D
        {
            get { return (float) Math.Sqrt(MagnitudeSquared2D); }
        }

        [JsonIgnore]
        public float MagnitudeSquared2D
        {
            get { return (m_x * m_x) + (m_y * m_y); }
        }

        public Vector()
            : this(0, 0, 0)
        { }

        public Vector(float x, float y, float z)
        {
            m_x = x;
            m_y = y;
            m_z = z;
        }

        public void Normalize()
        {
            float magSquared = MagnitudeSquared;
            if (magSquared > 0)
            {
                float invSqrt = (float) (1.0 / Math.Sqrt(magSquared));
                m_x *= invSqrt;
                m_y *= invSqrt;
                m_z *= invSqrt;
            }
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_x = r.ReadSingle();
            m_y = r.ReadSingle();
            m_z = r.ReadSingle();

            Debug.Assert(r.Position() - r.Marked() == SizeOf<Vector>());
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_x);
            w.Write(m_y);
            w.Write(m_z);

            Debug.Assert(w.Position() - w.Marked() == SizeOf<Vector>());
        }

        public override string ToString()
        {
            return string.Format("<{0:0.###},{1:0.###},{2:0.###}>", m_x, m_y, m_z);
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

            return m_x.Equals(other.m_x)
                && m_y.Equals(other.m_y)
                && m_z.Equals(other.m_z);
        }

        public static float Distance(Vector v1, Vector v2)
        {
            return (v2 - v1).Magnitude;
        }

        public static float DotProduct(Vector v1, Vector v2)
        {
            return (v1.m_x * v2.m_x)
                 + (v1.m_y * v2.m_y)
                 + (v1.m_z * v2.m_z);
        }

        public static Vector CrossProduct(Vector v1, Vector v2)
        {
            return new Vector(
                (v1.m_y * v2.m_z) - (v1.m_z * v2.m_y),
                (v1.m_z * v2.m_x) - (v1.m_x * v2.m_z),
                (v1.m_x * v2.m_y) - (v1.m_y * v2.m_x));
        }

        public static Vector operator -(Vector v)
        {
            return new Vector(-v.m_x, -v.m_y, -v.m_z);
        }

        public static Vector operator +(Vector left, Vector right)
        {
            return new Vector(
                left.m_x + right.m_x,
                left.m_y + right.m_y,
                left.m_z + right.m_z);
        }

        public static Vector operator -(Vector left, Vector right)
        {
            return new Vector(
                left.m_x - right.m_x,
                left.m_y - right.m_y,
                left.m_z - right.m_z);
        }

        public static Vector operator *(Vector left, float right)
        {
            return new Vector(
                left.m_x * right,
                left.m_y * right,
                left.m_z * right);
        }

        public static Vector operator *(float left, Vector right)
        {
            return new Vector(
                left * right.m_x,
                left * right.m_y,
                left * right.m_z);
        }

        public static Vector operator /(Vector left, float right)
        {
            return new Vector(
                left.m_x / right,
                left.m_y / right,
                left.m_z / right);
        }
    }
}
