using GTASaveData.Serialization;
using Newtonsoft.Json;
using System;

namespace GTASaveData
{
    /// <summary>
    /// Represents a 2-dimensional vector.
    /// </summary>
    [Size(8)]
    public class Vector2D : SerializableObject,
        IEquatable<Vector2D>
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
            get { return (m_x * m_x) + (m_y * m_y); }
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

        public Vector2D()
            : this(0, 0)
        { }

        public Vector2D(float x, float y)
        {
            m_x = x;
            m_y = y;
        }

        public void Normalize()
        {
            float magSquared = MagnitudeSquared;
            if (magSquared > 0)
            {
                float invSqrt = (float) (1.0 / Math.Sqrt(magSquared));
                m_x *= invSqrt;
                m_y *= invSqrt;
            }
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_x = r.ReadSingle();
            m_y = r.ReadSingle();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_x);
            w.Write(m_y);
        }

        public override string ToString()
        {
            return string.Format("<{0:0.###},{1:0.###}>", m_x, m_y);
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

            return m_x.Equals(other.m_x)
                && m_y.Equals(other.m_y);
        }

        public static float Distance(Vector2D v1, Vector2D v2)
        {
            return (v2 - v1).Magnitude;
        }

        public static float DotProduct(Vector2D v1, Vector2D v2)
        {
            return (v1.m_x * v2.m_x)
                 + (v1.m_y * v2.m_y);
        }

        public static float CrossProduct(Vector2D v1, Vector2D v2)
        {
            return (v1.m_x * v2.m_y) - (v1.m_y * v2.m_x);
        }

        public static Vector2D operator -(Vector2D v)
        {
            return new Vector2D(-v.m_x, -v.m_y);
        }

        public static Vector2D operator +(Vector2D left, Vector2D right)
        {
            return new Vector2D(
                left.m_x + right.m_x,
                left.m_y + right.m_y);
        }

        public static Vector2D operator -(Vector2D left, Vector2D right)
        {
            return new Vector2D(
                left.m_x - right.m_x,
                left.m_y - right.m_y);
        }

        public static Vector2D operator *(Vector2D left, float right)
        {
            return new Vector2D(
                left.m_x * right,
                left.m_y * right);
        }

        public static Vector2D operator *(float left, Vector2D right)
        {
            return new Vector2D(
                left * right.m_x,
                left * right.m_y);
        }

        public static Vector2D operator /(Vector2D left, float right)
        {
            return new Vector2D(
                left.m_x / right,
                left.m_y / right);
        }
    }
}
