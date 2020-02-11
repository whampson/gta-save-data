using GTASaveData.Serialization;
using System;

namespace GTASaveData.Common
{
    /// <summary>
    /// Represents a rectangle in 3-space.
    /// </summary>
    public class Rect3d : SerializableObject, IEquatable<Rect3d>
    {
        private float m_xMin;
        private float m_xMax;
        private float m_yMin;
        private float m_yMax;
        private float m_zMin;
        private float m_zMax;

        public float XMin
        {
            get { return m_xMin; }
            set { m_xMin = value; OnPropertyChanged(); }
        }

        public float XMax
        {
            get { return m_xMax; }
            set { m_xMax = value; OnPropertyChanged(); }
        }

        public float YMin
        {
            get { return m_yMin; }
            set { m_yMin = value; OnPropertyChanged(); }
        }

        public float YMax
        {
            get { return m_yMax; }
            set { m_yMax = value; OnPropertyChanged(); }
        }

        public float ZMin
        {
            get { return m_zMin; }
            set { m_zMin = value; OnPropertyChanged(); }
        }

        public float ZMax
        {
            get { return m_zMax; }
            set { m_zMax = value; OnPropertyChanged(); }
        }

        public Rect3d()
        { }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_xMin = r.ReadSingle();
            m_xMax = r.ReadSingle();
            m_yMin = r.ReadSingle();
            m_yMax = r.ReadSingle();
            m_zMin = r.ReadSingle();
            m_zMax = r.ReadSingle();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_xMin);
            w.Write(m_xMax);
            w.Write(m_yMin);
            w.Write(m_yMax);
            w.Write(m_zMin);
            w.Write(m_zMax);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Rect3d);
        }

        public bool Equals(Rect3d other)
        {
            if (other == null)
            {
                return false;
            }

            return m_xMin.Equals(other.m_xMin)
                && m_xMax.Equals(other.m_xMax)
                && m_yMin.Equals(other.m_yMin)
                && m_yMax.Equals(other.m_yMax)
                && m_zMin.Equals(other.m_zMin)
                && m_zMax.Equals(other.m_zMax);
        }
    }
}
