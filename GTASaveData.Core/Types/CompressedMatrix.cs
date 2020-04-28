using System;

namespace GTASaveData.Types
{
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

        public Matrix Decompress()
        {
            Matrix m = new Matrix(Position);
            m.Right.X = RightX / 127.0f;
            m.Right.Y = RightY / 127.0f;
            m.Right.Z = RightZ / 127.0f;
            m.Forward.X = ForwardX / 127.0f;
            m.Forward.Y = ForwardY / 127.0f;
            m.Forward.Z = ForwardZ / 127.0f;
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
            buf.Skip(2);

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
            buf.Skip(2);

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
