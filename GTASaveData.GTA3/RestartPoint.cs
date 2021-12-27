using System;
using System.Diagnostics;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class RestartPoint : SaveDataObject,
        IEquatable<RestartPoint>, IDeepClonable<RestartPoint>
    {
        private Vector3 m_position;
        private float m_angle;

        public Vector3 Position
        { 
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public float Angle
        { 
            get { return m_angle; }
            set { m_angle = value; OnPropertyChanged(); }
        }

        public RestartPoint()
        {
            Position = new Vector3();
        }

        public RestartPoint(RestartPoint other)
        {
            Position = other.Position;
            Angle = other.Angle;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            Position = buf.ReadStruct<Vector3>();
            Angle = buf.ReadFloat();

            Debug.Assert(buf.Offset == SizeOf<RestartPoint>());
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            buf.Write(Position);
            buf.Write(Angle);

            Debug.Assert(buf.Offset == SizeOf<RestartPoint>());
        }

        protected override int GetSize(SerializationParams prm)
        {
            return 16;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RestartPoint);
        }

        public bool Equals(RestartPoint other)
        {
            if (other == null)
            {
                return false;
            }

            return Position.Equals(other.Position)
                && Angle.Equals(other.Angle);
        }

        public RestartPoint DeepClone()
        {
            return new RestartPoint(this);
        }
    }
}
