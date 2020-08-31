using System;

namespace GTASaveData.Types
{
    /// <summary>
    /// A compressed form of the <see cref="ViewMatrix"/> data structure.
    /// </summary>
    public class CompressedViewMatrix : SaveDataObject,
        IEquatable<CompressedViewMatrix>, IDeepClonable<CompressedViewMatrix>
    {
        private const int Size = 20;

        private Vector3D m_position;
        private byte m_rightX;
        private byte m_rightY;
        private byte m_rightZ;
        private byte m_forwardX;
        private byte m_forwardY;
        private byte m_forwardZ;

        public Vector3D Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public byte RightX
        {
            get { return m_rightX; }
            set { m_rightX = value; OnPropertyChanged(); }
        }

        public byte RightY
        {
            get { return m_rightY; }
            set { m_rightY = value; OnPropertyChanged(); }
        }

        public byte RightZ
        {
            get { return m_rightZ; }
            set { m_rightZ = value; OnPropertyChanged(); }
        }

        public byte ForwardX
        {
            get { return m_forwardX; }
            set { m_forwardX = value; OnPropertyChanged(); }
        }

        public byte ForwardY
        {
            get { return m_forwardY; }
            set { m_forwardY = value; OnPropertyChanged(); }
        }

        public byte ForwardZ
        {
            get { return m_forwardZ; }
            set { m_forwardZ = value; OnPropertyChanged(); }
        }

        public CompressedViewMatrix()
        {
            Position = new Vector3D();
            RightX = 127;
            RightY = 0;
            RightZ = 0;
            ForwardX = 0;
            ForwardY = 127;
            ForwardZ = 0;
        }

        public CompressedViewMatrix(CompressedViewMatrix other)
        {
            Position = new Vector3D(other.Position);
            RightX = other.RightX;
            RightY = other.RightY;
            RightZ = other.RightZ;
            ForwardX = other.ForwardX;
            ForwardY = other.ForwardY;
            ForwardZ = other.ForwardZ;
        }

        public ViewMatrix Decompress()
        {
            Vector3D r = new Vector3D
            {
                X = RightX / 127.0f,
                Y = RightY / 127.0f,
                Z = RightZ / 127.0f
            };
            Vector3D f = new Vector3D
            {
                X = ForwardX / 127.0f,
                Y = ForwardY / 127.0f,
                Z = ForwardZ / 127.0f
            };
            Vector3D u = Vector3D.Cross(r, f);

            return new ViewMatrix()
            {
                Right = r,
                Forward = f,
                Up = u,
                Position = Position.DeepClone()
            };
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Position = buf.Read<Vector3D>();
            RightX = buf.ReadByte();
            RightY = buf.ReadByte();
            RightZ = buf.ReadByte();
            ForwardX = buf.ReadByte();
            ForwardY = buf.ReadByte();
            ForwardZ = buf.ReadByte();
            buf.ReadInt16();
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(Position);
            buf.Write(RightX);
            buf.Write(RightY);
            buf.Write(RightZ);
            buf.Write(ForwardX);
            buf.Write(ForwardY);
            buf.Write(ForwardZ);
            buf.Write((short) 0);
        }

        protected override int GetSize(FileFormat fmt)
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
            if (!(obj is CompressedViewMatrix))
            {
                return false;
            }

            return Equals((CompressedViewMatrix) obj);
        }

        public bool Equals(CompressedViewMatrix other)
        {
            return Position.Equals(other.Position)
                && RightX.Equals(other.RightX)
                && RightY.Equals(other.RightY)
                && RightZ.Equals(other.RightZ)
                && ForwardX.Equals(other.ForwardX)
                && ForwardY.Equals(other.ForwardY)
                && ForwardZ.Equals(other.ForwardZ);
        }

        public CompressedViewMatrix DeepClone()
        {
            return new CompressedViewMatrix(this);
        }

        public static bool operator ==(CompressedViewMatrix m1, CompressedViewMatrix m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator !=(CompressedViewMatrix m1, CompressedViewMatrix m2)
        {
            return !m1.Equals(m2);
        }
    }
}
