using System;
using System.Runtime.InteropServices;

namespace GTASaveData.Types
{
    /// <summary>
    /// Represents a 3D view matrix.
    /// </summary>
    /// <remarks>Code largely taken from GTA.</remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Matrix : IEquatable<Matrix>       // TODO: Serializable?
    {
        public static readonly Vector3D UnitX = new Vector3D(1, 0, 0);
        public static readonly Vector3D UnitY = new Vector3D(0, 1, 0);
        public static readonly Vector3D UnitZ = new Vector3D(0, 0, 1);
        public static readonly Matrix Identity = new Matrix() { Right = UnitX, Forward = UnitY, Up = UnitZ };

        public Vector3D Right;
        public Vector3D Forward;
        public Vector3D Up;
        public Vector3D Position;

        public Matrix(Vector3D position)
        {
            Position = position;
            Right = UnitX;
            Forward = UnitY;
            Up = UnitZ;
        }

        public Matrix(Matrix other)
        {
            Right = other.Right;
            Forward = other.Forward;
            Up = other.Up;
            Position = other.Position;
        }

        public CompressedMatrix Compress()
        {
            return new CompressedMatrix
            {
                Position = Position,
                RightX = (byte) (127 * Right.X),
                RightY = (byte) (127 * Right.Y),
                RightZ = (byte) (127 * Right.Z),
                ForwardX = (byte) (127 * Forward.X),
                ForwardY = (byte) (127 * Forward.Y),
                ForwardZ = (byte) (127 * Forward.Z)
            };
        }

        public static Matrix RotateX(Matrix m, float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            m.Right.X = 1;
            m.Right.Y = 0;
            m.Right.Z = 0;

            m.Forward.X = 0;
            m.Forward.Y = cos;
            m.Forward.Z = sin;

            m.Up.X = 0;
            m.Up.Y = -sin;
            m.Up.Z = cos;

            return m;
        }

        public static Matrix RotateY(Matrix m, float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            m.Right.X = cos;
            m.Right.Y = 0;
            m.Right.Z = -sin;

            m.Forward.X = 0;
            m.Forward.Y = 1;
            m.Forward.Z = 0;

            m.Up.X = sin;
            m.Up.Y = 0;
            m.Up.Z = cos;

            return m;
        }

        public static Matrix RotateZ(Matrix m, float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            m.Right.X = cos;
            m.Right.Y = sin;
            m.Right.Z = 0;

            m.Forward.X = -sin;
            m.Forward.Y = cos;
            m.Forward.Z = 0;

            m.Up.X = 0;
            m.Up.Y = 0;
            m.Up.Z = 1;

            return m;
        }

        public static Matrix Rotate(Matrix m, float xAngle, float yAngle, float zAngle)
        {
            float sinX = (float) Math.Sin(xAngle);
            float cosX = (float) Math.Cos(xAngle);
            float sinY = (float) Math.Sin(yAngle);
            float cosY = (float) Math.Cos(yAngle);
            float sinZ = (float) Math.Sin(zAngle);
            float cosZ = (float) Math.Cos(zAngle);

            m.Right.X = (cosZ * cosY) - ((sinZ * sinX) * sinY);
            m.Right.Y = ((cosZ * sinX) * sinY) + (sinZ * cosY);
            m.Right.Z = -cosX * sinY;

            m.Forward.X = -sinZ * cosX;
            m.Forward.Y = cosZ * cosX;
            m.Forward.Z = sinX;

            m.Up.X = (sinZ * sinX) * cosY + (cosZ * sinY);
            m.Up.Y = (sinZ * sinY) - (cosZ * sinX) * cosY;
            m.Up.Z = cosX * cosY;

            return m;
        }

        public static Matrix Orthogonalize(Matrix m)
        {
            Vector3D r = m.Right;
            Vector3D f = m.Forward;
            Vector3D u = m.Up;

            m.Up = Vector3D.Normalize(Vector3D.Cross(r, f));
            m.Right = Vector3D.Normalize(Vector3D.Cross(f, u));
            m.Forward = Vector3D.Normalize(Vector3D.Cross(u, r));

            return m;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash += 23 * Right.GetHashCode();
            hash += 23 * Forward.GetHashCode();
            hash += 23 * Up.GetHashCode();
            hash += 23 * Position.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Matrix))
            {
                return false;
            }

            return Equals((Matrix) obj);
        }

        public bool Equals(Matrix other)
        {
            return Right.Equals(other.Right)
                && Forward.Equals(other.Forward)
                && Up.Equals(other.Up)
                && Position.Equals(other.Position);
        }

        public static bool operator ==(Matrix m1, Matrix m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !m1.Equals(m2);
        }

    }
}
