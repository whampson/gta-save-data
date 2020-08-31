using System;
using WpfEssentials;

namespace GTASaveData.Types
{
    /// <summary>
    /// Represents a 3D view matrix.
    /// </summary>
    /// <remarks>Code largely taken from GTA.</remarks>
    public class ViewMatrix : ObservableObject,     // TODO: SaveDataObject?
        IEquatable<ViewMatrix>, IDeepClonable<ViewMatrix>
    {
        public static Vector3D UnitX => new Vector3D(1, 0, 0);
        public static Vector3D UnitY => new Vector3D(0, 1, 0);
        public static Vector3D UnitZ => new Vector3D(0, 0, 1);
        public static ViewMatrix Identity => new ViewMatrix() { Right = UnitX, Forward = UnitY, Up = UnitZ };

        private Vector3D m_right;
        private Vector3D m_forward;
        private Vector3D m_up;
        private Vector3D m_position;

        public Vector3D Right
        {
            get { return m_right; }
            set { m_right = value; OnPropertyChanged(); }
        }

        public Vector3D Forward
        {
            get { return m_forward; }
            set { m_forward = value; OnPropertyChanged(); }
        }

        public Vector3D Up
        {
            get { return m_up; }
            set { m_up = value; OnPropertyChanged(); }
        }

        public Vector3D Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public ViewMatrix()
            : this(new Vector3D())
        { }

        public ViewMatrix(Vector3D position)
        {
            Position = new Vector3D(position);
            Right = UnitX;
            Forward = UnitY;
            Up = UnitZ;
        }

        public ViewMatrix(ViewMatrix other)
        {
            Right = new Vector3D(other.Right);
            Forward = new Vector3D(other.Forward);
            Up = new Vector3D(other.Up);
            Position = new Vector3D(other.Position);
        }

        public CompressedViewMatrix Compress()
        {
            return new CompressedViewMatrix
            {
                Position = Position.DeepClone(),
                RightX = (byte) (127 * Right.X),
                RightY = (byte) (127 * Right.Y),
                RightZ = (byte) (127 * Right.Z),
                ForwardX = (byte) (127 * Forward.X),
                ForwardY = (byte) (127 * Forward.Y),
                ForwardZ = (byte) (127 * Forward.Z)
            };
        }

        public ViewMatrix RotateX(float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            Right = new Vector3D { X = 1, Y = 0, Z = 0 };
            Forward = new Vector3D { X = 0, Y = cos, Z = sin };
            Up = new Vector3D { X = 0, Y = -sin, Z = cos };

            return this;
        }

        public ViewMatrix RotateY(float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            Right = new Vector3D { X = cos, Y = 0, Z = -sin };
            Forward = new Vector3D { X = 0, Y = 1, Z = 0 };
            Up = new Vector3D { X = sin, Y = 0, Z = cos };

            return this;
        }

        public ViewMatrix RotateZ(float angle)
        {
            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            Right = new Vector3D { X = cos, Y = sin, Z = 0 };
            Forward = new Vector3D { X = -sin, Y = cos, Z = 0 };
            Up = new Vector3D { X = 0, Y = 0, Z = 1 };

            return this;
        }

        public ViewMatrix Rotate(float xAngle, float yAngle, float zAngle)
        {
            float sinX = (float) Math.Sin(xAngle);
            float cosX = (float) Math.Cos(xAngle);
            float sinY = (float) Math.Sin(yAngle);
            float cosY = (float) Math.Cos(yAngle);
            float sinZ = (float) Math.Sin(zAngle);
            float cosZ = (float) Math.Cos(zAngle);

            Right = new Vector3D
            {
                X = (cosZ * cosY) - ((sinZ * sinX) * sinY),
                Y = ((cosZ * sinX) * sinY) + (sinZ * cosY),
                Z = -cosX * sinY
            };
            Forward = new Vector3D
            {
                X = -sinZ * cosX,
                Y = cosZ * cosX,
                Z = sinX
            };
            Up = new Vector3D
            {
                X = (sinZ * sinX) * cosY + (cosZ * sinY),
                Y = (sinZ * sinY) - (cosZ * sinX) * cosY,
                Z = cosX * cosY
            };

            return this;
        }

        public ViewMatrix Orthogonalize()
        {
            Vector3D r = Right.DeepClone();
            Vector3D f = Forward.DeepClone();
            Vector3D u = Up.DeepClone();

            Up = Vector3D.Cross(r, f).Normalize();
            Right = Vector3D.Cross(f, u).Normalize();
            Forward = Vector3D.Cross(u, r).Normalize();

            return this;
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

        public ViewMatrix DeepClone()
        {
            return new ViewMatrix(this);
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
