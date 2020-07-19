using GTASaveData.Types.Interfaces;
using System;
using System.Runtime.InteropServices;

namespace GTASaveData.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    public struct Quaternion : ISaveDataObject, IEquatable<Quaternion>
    {
        private const int Size = 8;

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Quaternion(Quaternion other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;
            W = other.W;
        }

        public static bool operator ==(Quaternion q1, Quaternion q2)
        {
            return q1.Equals(q2);
        }

        public static bool operator !=(Quaternion q1, Quaternion q2)
        {
            return !q1.Equals(q2);
        }

        int ISaveDataObject.ReadData(StreamBuffer buf, FileFormat fmt)
        {
            X = buf.ReadFloat();
            Y = buf.ReadFloat();
            Z = buf.ReadFloat();
            W = buf.ReadFloat();

            return Size;
        }

        int ISaveDataObject.WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(X);
            buf.Write(Y);
            buf.Write(Z);
            buf.Write(W);

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
            hash += 23 * Z.GetHashCode();
            hash += 23 * W.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Quaternion))
            {
                return false;
            }

            return Equals((Quaternion) obj);
        }

        public bool Equals(Quaternion other)
        {
            return X.Equals(other.X)
                && Y.Equals(other.Y)
                && Z.Equals(other.Z)
                && W.Equals(other.W);
        }
    }
}
