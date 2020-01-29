using GTASaveData.Serialization;
using Newtonsoft.Json;
using System;

namespace GTASaveData.Common
{
    public sealed class Rect3d : SaveDataObject,
        IEquatable<Rect3d>
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

        [JsonIgnore]
        public Vector3d MinCoord
        {
            get
            { 
                return new Vector3d() { X = m_xMin, Y = m_yMin, Z = m_zMin };
            }
            
            set
            {
                if (value == null)
                {
                    value = new Vector3d();
                }

                m_xMin = value.X;
                m_yMin = value.Y;
                m_zMin = value.Z;
            }
        }

        [JsonIgnore]
        public Vector3d MaxCoord
        {
            get
            {
                return new Vector3d() { X = m_xMax, Y = m_yMax, Z = m_zMax };
            }

            set
            {
                if (value == null)
                {
                    value = new Vector3d();
                }

                m_xMax = value.X;
                m_yMax = value.Y;
                m_zMax = value.Z;
            }
        }


        public Rect3d()
        { }

        private Rect3d(SaveDataSerializer serializer, FileFormat format)
        {
            m_xMin = serializer.ReadSingle();
            m_xMax = serializer.ReadSingle();
            m_yMin = serializer.ReadSingle();
            m_yMax = serializer.ReadSingle();
            m_zMin = serializer.ReadSingle();
            m_zMax = serializer.ReadSingle();
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.Write(m_xMin);
            serializer.Write(m_xMax);
            serializer.Write(m_yMin);
            serializer.Write(m_yMax);
            serializer.Write(m_zMin);
            serializer.Write(m_zMax);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
