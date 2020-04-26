using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// A 3D view matrix.
    /// </summary>
    /// <remarks>Code largely taken from GTA.</remarks>
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

        // TODO: other math funcs?

        public static Matrix Orthogonalize(Matrix m)
        {
            m.Up = Vector3D.Normalize(Vector3D.Cross(m.Right, m.Forward));
            m.Right = Vector3D.Normalize(Vector3D.Cross(m.Forward, m.Up));
            m.Forward = Vector3D.Normalize(Vector3D.Cross(m.Up, m.Right));

            return m;
        }

        public static bool operator ==(Matrix m1, Matrix m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !m1.Equals(m2);
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
    }

    public struct CompressedMatrix : ISerializable, IEquatable<CompressedMatrix>
    {
        private const int Size = 20;

        public Vector3D Position;
        public byte RightX;
        public byte RightY;
        public byte RightZ;
        public byte ForwardX;
        public byte ForwardY;
        public byte ForwardZ;

        public Matrix Decompress(CompressedMatrix cm)
        {
            Matrix m = new Matrix(cm.Position);
            m.Right.X = cm.RightX / 127.0f;
            m.Right.Y = cm.RightY / 127.0f;
            m.Right.Z = cm.RightZ / 127.0f;
            m.Forward.X = cm.ForwardX / 127.0f;
            m.Forward.Y = cm.ForwardY / 127.0f;
            m.Forward.Z = cm.ForwardZ / 127.0f;
            m.Up = Vector3D.Cross(m.Right, m.Forward);

            return Matrix.Orthogonalize(m);
        }

        int ISerializable.ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Position = buf.Read<Vector3D>();
            RightX = buf.ReadByte();
            RightY = buf.ReadByte();
            RightZ = buf.ReadByte();
            ForwardX = buf.ReadByte();
            ForwardY = buf.ReadByte();
            ForwardZ = buf.ReadByte();

            return Size;
        }

        int ISerializable.WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Position);
            buf.Write(RightX);
            buf.Write(RightY);
            buf.Write(RightZ);
            buf.Write(ForwardX);
            buf.Write(ForwardY);
            buf.Write(ForwardZ);
            buf.Align4Bytes();

            return Size;
        }

        int ISerializable.GetSize(DataFormat fmt)
        {
            return Size;
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
