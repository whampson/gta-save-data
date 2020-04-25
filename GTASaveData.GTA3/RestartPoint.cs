using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(16)]
    public class RestartPoint : SaveDataObject, IEquatable<RestartPoint>
    {
        private Vector3D m_position;
        private float m_angle;

        public Vector3D Position
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
            Position = new Vector3D();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Position = buf.Read<Vector3D>();
            Angle = buf.ReadFloat();

            Debug.Assert(buf.Offset == SizeOf<RestartPoint>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Position);
            buf.Write(Angle);

            Debug.Assert(buf.Offset == SizeOf<RestartPoint>());
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
    }
}
