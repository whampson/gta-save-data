using Newtonsoft.Json;
using System;

namespace GTASaveData.Types
{
    /// <summary>
    /// Represents a 2-dimensional vector.
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
            get { return (X * X) + (Y * Y); }
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

        public Vector2D()
            : this(0, 0)
        { }

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void Normalize()
        {
            float magSquared = MagnitudeSquared;
            if (magSquared > 0)
            {
                float invSqrt = (float) (1.0 / Math.Sqrt(magSquared));
                X *= invSqrt;
                Y *= invSqrt;
            }
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            X = buf.ReadSingle();
            Y = buf.ReadSingle();
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
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

        public static float Distance(Vector2D v1, Vector2D v2)
        {
            return (v2 - v1).Magnitude;
        }

        public static float DotProduct(Vector2D v1, Vector2D v2)
        {
            return (v1.X * v2.X)
                 + (v1.Y * v2.Y);
        }

        public static float CrossProduct(Vector2D v1, Vector2D v2)
        {
            return (v1.X * v2.Y) - (v1.Y * v2.X);
        }

        public static Vector2D operator -(Vector2D v)
        {
            return new Vector2D(-v.X, -v.Y);
        }

        public static Vector2D operator +(Vector2D left, Vector2D right)
        {
            return new Vector2D(
                left.X + right.X,
                left.Y + right.Y);
        }

        public static Vector2D operator -(Vector2D left, Vector2D right)
        {
            return new Vector2D(
                left.X - right.X,
                left.Y - right.Y);
        }

        public static Vector2D operator *(Vector2D left, float right)
        {
            return new Vector2D(
                left.X * right,
                left.Y * right);
        }

        public static Vector2D operator *(float left, Vector2D right)
        {
            return new Vector2D(
                left * right.X,
                left * right.Y);
        }

        public static Vector2D operator /(Vector2D left, float right)
        {
            return new Vector2D(
                left.X / right,
                left.Y / right);
        }
    }
}
