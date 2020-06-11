using GTASaveData.Types.Interfaces;
using System;

namespace GTASaveData.Types
{
    /// <summary>
    /// A 2-dimensional vector.
    /// </summary>
    public struct Vector2D : ISaveDataObject, IEquatable<Vector2D>
    {
        private const int Size = 8;

        public float X;
        public float Y;

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

        public static Vector2D Normalize(Vector2D v)
        {
            return Normalize(v);
        }

        public static Vector2D Normalize(Vector2D v, float norm)
        {
            float mag = v.GetMagnitude();
            if (mag > 0)
            {
                float invSq = norm / mag;
                v.X *= invSq;
                v.Y *= invSq;
            }

            return v;
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

        int ISaveDataObject.ReadData(StreamBuffer buf, FileFormat fmt)
        {
            X = buf.ReadFloat();
            Y = buf.ReadFloat();

            return Size;
        }

        int ISaveDataObject.WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(X);
            buf.Write(Y);

            return Size;
        }

        int ISaveDataObject.GetSize(FileFormat fmt)
        {
            return Size;
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

        public override string ToString()
        {
            return $"{X:0.###},{Y:0.###}";
        }
    }
}
