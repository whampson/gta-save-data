using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    public class Camera : GTAObject,
        IEquatable<Camera>
    {
        private Vector m_position;
        private float m_carZoomIndicator;
        private float m_pedZoomIndicator;

        public Camera()
        {
            m_position = new Vector();
        }

        public Vector Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public float CarZoomIndicator
        {
            get { return m_carZoomIndicator; }
            set { m_carZoomIndicator = value; OnPropertyChanged(); }
        }

        public float PedZoomIndicator
        {
            get { return m_pedZoomIndicator; }
            set { m_pedZoomIndicator = value; OnPropertyChanged(); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Camera);
        }

        public bool Equals(Camera other)
        {
            if (other == null)
            {
                return false;
            }

            return m_position.Equals(other.m_position)
                && m_carZoomIndicator.Equals(other.m_carZoomIndicator)
                && m_pedZoomIndicator.Equals(other.m_pedZoomIndicator);
        }
    }
}
