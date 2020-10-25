using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace GTASaveData
{
    /// <summary>
    /// A compressed form of the <see cref="Matrix"/> data structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 20)]
    public struct CompressedMatrix : IEquatable<CompressedMatrix>
    {
        public static CompressedMatrix Identity => new CompressedMatrix() { RightX = 127, ForwardY = 127 };

        public Vector3 Position;
        public byte RightX;
        public byte RightY;
        public byte RightZ;
        public byte ForwardX;
        public byte ForwardY;
        public byte ForwardZ;

        public CompressedMatrix(Vector3 position)
        {
            Position = position;
            RightX = Identity.RightX;
            RightY = Identity.RightY;
            RightZ = Identity.RightZ;
            ForwardX = Identity.ForwardX;
            ForwardY = Identity.ForwardY;
            ForwardZ = Identity.ForwardZ;
        }

        public CompressedMatrix(CompressedMatrix other)
        {
            Position = other.Position;
            RightX = other.RightX;
            RightY = other.RightY;
            RightZ = other.RightZ;
            ForwardX = other.ForwardX;
            ForwardY = other.ForwardY;
            ForwardZ = other.ForwardZ;
        }

        public Matrix Decompress()
        {
            Vector3 r = new Vector3
            {
                X = RightX / 127.0f,
                Y = RightY / 127.0f,
                Z = RightZ / 127.0f
            };
            Vector3 f = new Vector3
            {
                X = ForwardX / 127.0f,
                Y = ForwardY / 127.0f,
                Z = ForwardZ / 127.0f
            };
            Vector3 u = Vector3.Cross(r, f);

            return new Matrix()
            {
                Right = r,
                Forward = f,
                Up = u,
                Position = Position
            };
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash += 23 * Position.GetHashCode();
            hash += 23 * RightX;
            hash += 23 * RightY;
            hash += 23 * RightZ;
            hash += 23 * ForwardX;
            hash += 23 * ForwardY;
            hash += 23 * ForwardZ;

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CompressedMatrix))
            {
                return false;
            }

            return Equals((CompressedMatrix) obj);
        }

        public bool Equals(CompressedMatrix other)
        {
            return Position.Equals(other.Position)
                && RightX.Equals(other.RightX)
                && RightY.Equals(other.RightY)
                && RightZ.Equals(other.RightZ)
                && ForwardX.Equals(other.ForwardX)
                && ForwardY.Equals(other.ForwardY)
                && ForwardZ.Equals(other.ForwardZ);
        }

        public static bool operator ==(CompressedMatrix m1, CompressedMatrix m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator !=(CompressedMatrix m1, CompressedMatrix m2)
        {
            return !m1.Equals(m2);
        }
    }
}
