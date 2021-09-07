using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace GTASaveData
{
    /// <summary>
    /// Represents a 3x3 view matrix.
    /// </summary>
    /// <remarks>Code derived from GTA3.</remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 64)]
    public struct Matrix : IEquatable<Matrix>
    {
        public static Matrix Identity => new Matrix() { Right = Vector3.UnitX, Forward = Vector3.UnitY, Up = Vector3.UnitZ };

        public Vector3 Right;
        private int _pad0;
        public Vector3 Forward;
        private int _pad1;
        public Vector3 Up;
        private int _pad2;
        public Vector3 Position;
        private int _pad3;

        public Matrix(Vector3 position)
        {
            Right = Identity.Right;
            Forward = Identity.Forward;
            Up = Identity.Up;
            Position = position;
            _pad0 = 0;
            _pad1 = 0;
            _pad2 = 0;
            _pad3 = 0;
        }

        public Matrix(Vector3 position, Vector3 right, Vector3 forward, Vector3 up)
        {
            Right = right;
            Forward = forward;
            Up = up;
            Position = position;
            _pad0 = 0;
            _pad1 = 0;
            _pad2 = 0;
            _pad3 = 0;
        }

        public Matrix(Matrix other)
        {
            Right = other.Right;
            Forward = other.Forward;
            Up = other.Up;
            Position = other.Position;
            _pad0 = 0;
            _pad1 = 0;
            _pad2 = 0;
            _pad3 = 0;
        }

        private static bool CloseEnough(float a, float b)
        {
            return (float.Epsilon > Math.Abs(a - b));
        }

        public Vector3 GetEulerAngles()
        {
            // Check for gimbal lock
            if (CloseEnough(Right.Z, -1.0f))
            {
                return new Vector3()
                {
                    X = 0,  // Gimbal lock, X doesn't matter
                    Y = (float) Math.PI / 2,
                    Z = (float) Math.Atan2(Forward.X, Up.X)
                };
            }
            else if (CloseEnough(Right.Z, 1.0f))
            {
                return new Vector3()
                {
                    X = 0,  // Gimbal lock, X doesn't matter
                    Y = (float) -Math.PI / 2,
                    Z = (float) Math.Atan2(-Forward.X, -Up.X)
                };
            }
            else    // Two solutions exist
            {
                float x1 = (float) -Math.Asin(Right.Z);
                float x2 = (float) Math.PI - x1;
                float y1 = (float) Math.Atan2(Forward.Z / Math.Cos(x1), Up.Z / Math.Cos(x1));
                float y2 = (float) Math.Atan2(Forward.Z / Math.Cos(x2), Up.Z / Math.Cos(x2));
                float z1 = (float) Math.Atan2(Right.Y / Math.Cos(x1), Right.X / Math.Cos(x1));
                float z2 = (float) Math.Atan2(Right.Y / Math.Cos(x2), Right.X / Math.Cos(x2));

                float a1 = Math.Abs(x1) + Math.Abs(y1) + Math.Abs(z1);
                float a2 = Math.Abs(x2) + Math.Abs(y2) + Math.Abs(z2);

                return (a1 <= a2)
                    ? new Vector3(x1, y1, z1)
                    : new Vector3(x2, y2, z2);
            }
        }

        public Matrix SetRotateX(float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            Right = new Vector3 { X = 1, Y = 0, Z = 0 };
            Forward = new Vector3 { X = 0, Y = cos, Z = sin };
            Up = new Vector3 { X = 0, Y = -sin, Z = cos };

            return this;
        }

        public Matrix SetRotateY(float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            Right = new Vector3 { X = cos, Y = 0, Z = -sin };
            Forward = new Vector3 { X = 0, Y = 1, Z = 0 };
            Up = new Vector3 { X = sin, Y = 0, Z = cos };

            return this;
        }

        public Matrix SetRotateZ(float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            Right = new Vector3 { X = cos, Y = sin, Z = 0 };
            Forward = new Vector3 { X = -sin, Y = cos, Z = 0 };
            Up = new Vector3 { X = 0, Y = 0, Z = 1 };

            return this;
        }

        public Matrix SetRotate(float xAngle, float yAngle, float zAngle)
        {
            float sinX = (float) Math.Sin(xAngle);
            float cosX = (float) Math.Cos(xAngle);
            float sinY = (float) Math.Sin(yAngle);
            float cosY = (float) Math.Cos(yAngle);
            float sinZ = (float) Math.Sin(zAngle);
            float cosZ = (float) Math.Cos(zAngle);

            Right = new Vector3
            {
                X = (cosZ * cosY) - ((sinZ * sinX) * sinY),
                Y = ((cosZ * sinX) * sinY) + (sinZ * cosY),
                Z = -cosX * sinY
            };
            Forward = new Vector3
            {
                X = -sinZ * cosX,
                Y = cosZ * cosX,
                Z = sinX
            };
            Up = new Vector3
            {
                X = (sinZ * sinX) * cosY + (cosZ * sinY),
                Y = (sinZ * sinY) - (cosZ * sinX) * cosY,
                Z = cosX * cosY
            };

            return this;
        }

        public Matrix Orthogonalize()
        {
            Vector3 r = Right;
            Vector3 f = Forward;
            Vector3 u = Up;

            Up = Vector3.Normalize(Vector3.Cross(r, f));
            Right = Vector3.Normalize(Vector3.Cross(f, u));
            Forward = Vector3.Normalize(Vector3.Cross(u, r));

            return this;
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

        // TODO: operators
    }
}
