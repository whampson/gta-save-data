using GTASaveData.Types.Interfaces;
using System;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GTASaveData.Types
{
    /// <summary>
    /// A compressed form of the <see cref="ViewMatrix"/> data structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 20)]
    public struct CompressedViewMatrix : ISaveDataObject, IEquatable<CompressedViewMatrix>
    {
        private const int Size = 20;

        public Vector3D Position { get; set; }
        public byte RightX { get; set; }
        public byte RightY { get; set; }
        public byte RightZ { get; set; }
        public byte ForwardX { get; set; }
        public byte ForwardY { get; set; }
        public byte ForwardZ { get; set; }

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
                Position = Position
            };
        }

        int ISaveDataObject.ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Position = buf.Read<Vector3D>();
            RightX = buf.ReadByte();
            RightY = buf.ReadByte();
            RightZ = buf.ReadByte();
            ForwardX = buf.ReadByte();
            ForwardY = buf.ReadByte();
            ForwardZ = buf.ReadByte();
            buf.ReadInt16();

            return Size;
        }

        int ISaveDataObject.WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(Position);
            buf.Write(RightX);
            buf.Write(RightY);
            buf.Write(RightZ);
            buf.Write(ForwardX);
            buf.Write(ForwardY);
            buf.Write(ForwardZ);
            buf.Write((short) 0);

            return Size;
        }

        int ISaveDataObject.GetSize(FileFormat fmt)
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
