using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GTASaveData.Types
{
    /// <summary>
    /// Represents a 3D view matrix.
    /// </summary>
    /// <remarks>Code largely taken from GTA.</remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ViewMatrix : IEquatable<ViewMatrix>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly Vector3D UnitX = new Vector3D(1, 0, 0);
        public static readonly Vector3D UnitY = new Vector3D(0, 1, 0);
        public static readonly Vector3D UnitZ = new Vector3D(0, 0, 1);
        public static readonly ViewMatrix Identity = new ViewMatrix() { Right = UnitX, Forward = UnitY, Up = UnitZ };

        public Vector3D Right { get; set; }
        public Vector3D Forward { get; set; }
        public Vector3D Up { get; set; }
        public Vector3D Position { get; set; }
        public ViewMatrix(Vector3D position)
        {
            Position = position;
            Right = UnitX;
            Forward = UnitY;
            Up = UnitZ;
            PropertyChanged = null;
        }

        public ViewMatrix(ViewMatrix other)
        {
            Right = other.Right;
            Forward = other.Forward;
            Up = other.Up;
            Position = other.Position;
            PropertyChanged = null;
        }

        public CompressedViewMatrix Compress()
        {
            return new CompressedViewMatrix
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

        public static ViewMatrix RotateX(ViewMatrix m, float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            Vector3D r = new Vector3D
            {
                X = 1,
                Y = 0,
                Z = 0
            };
            Vector3D f = new Vector3D
            {
                X = 0,
                Y = cos,
                Z = sin
            };
            Vector3D u = new Vector3D
            {
                X = 0,
                Y = -sin,
                Z = cos
            };

            return new ViewMatrix()
            {
                Right = r,
                Forward = f,
                Up = u
            };
        }

        public static ViewMatrix RotateY(ViewMatrix m, float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            Vector3D r = new Vector3D
            {
                X = cos,
                Y = 0,
                Z = -sin
            };
            Vector3D f = new Vector3D
            {
                X = 0,
                Y = 1,
                Z = 0
            };
            Vector3D u = new Vector3D
            {
                X = sin,
                Y = 0,
                Z = cos
            };

            return new ViewMatrix()
            {
                Right = r,
                Forward = f,
                Up = u
            };
        }

        public static ViewMatrix RotateZ(ViewMatrix m, float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            Vector3D r = new Vector3D
            {
                X = cos,
                Y = sin,
                Z = 0
            };
            Vector3D f = new Vector3D
            {
                X = -sin,
                Y = cos,
                Z = 0
            };
            Vector3D u = new Vector3D
            {
                X = 0,
                Y = 0,
                Z = 1
            };

            return new ViewMatrix()
            {
                Right = r,
                Forward = f,
                Up = f
            };
        }

        public static ViewMatrix Rotate(ViewMatrix m, float xAngle, float yAngle, float zAngle)
        {
            float sinX = (float) Math.Sin(xAngle);
            float cosX = (float) Math.Cos(xAngle);
            float sinY = (float) Math.Sin(yAngle);
            float cosY = (float) Math.Cos(yAngle);
            float sinZ = (float) Math.Sin(zAngle);
            float cosZ = (float) Math.Cos(zAngle);

            Vector3D r = new Vector3D
            {
                X = (cosZ * cosY) - ((sinZ * sinX) * sinY),
                Y = ((cosZ * sinX) * sinY) + (sinZ * cosY),
                Z = -cosX * sinY
            };
            Vector3D f = new Vector3D
            {
                X = -sinZ * cosX,
                Y = cosZ * cosX,
                Z = sinX
            };
            Vector3D u = new Vector3D
            {
                X = (sinZ * sinX) * cosY + (cosZ * sinY),
                Y = (sinZ * sinY) - (cosZ * sinX) * cosY,
                Z = cosX * cosY
            };

            return new ViewMatrix()
            {
                Right = r,
                Forward = f,
                Up = u
            };
        }

        public static ViewMatrix Orthogonalize(ViewMatrix m)
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
            if (!(obj is ViewMatrix))
            {
                return false;
            }

            return Equals((ViewMatrix) obj);
        }

        public bool Equals(ViewMatrix other)
        {
            return Right.Equals(other.Right)
                && Forward.Equals(other.Forward)
                && Up.Equals(other.Up)
                && Position.Equals(other.Position);
        }

        public static bool operator ==(ViewMatrix m1, ViewMatrix m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator !=(ViewMatrix m1, ViewMatrix m2)
        {
            return !m1.Equals(m2);
        }

    }
}
