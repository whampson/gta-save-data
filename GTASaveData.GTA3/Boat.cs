using GTASaveData.Serialization;
using System;
using System.Linq;

namespace GTASaveData.GTA3
{
    public sealed class Boat : SaveDataObject,
        IEquatable<Boat>
    {
        private int m_unknown0;
        private int m_modelId;  // TODO: enum
        private int m_unknown1;
        private byte[] m_unknownArray0;
        private Vector3d m_position;
        private byte[] m_unknownArray1;

        public int ModelId
        {
            get { return m_modelId; }
            set { m_modelId = value; OnPropertyChanged(); }
        }

        public Vector3d Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public Boat()
        {
            m_unknownArray0 = null;  // Lazy creation below
            m_unknownArray1 = null;  // Lazy creation below
            m_position = new Vector3d();
        }

        private Boat(SaveDataSerializer serializer, FileFormat format)
        {
            if (m_unknownArray0 == null || m_unknownArray1 == null)
            {
                CreateUnknowns(format);
            }

            m_unknown0 = serializer.ReadInt32();
            m_modelId = serializer.ReadInt16();
            m_unknown1 = serializer.ReadInt32();
            m_unknownArray0 = serializer.ReadBytes(m_unknownArray0.Length);
            m_position = serializer.ReadObject<Vector3d>();
            m_unknownArray1 = serializer.ReadBytes(m_unknownArray1.Length);
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            if (m_unknownArray0 == null || m_unknownArray1 == null)
            {
                CreateUnknowns(format);
            }

            serializer.Write(m_unknown0);
            serializer.Write((short) m_modelId);
            serializer.Write(m_unknown1);
            serializer.Write(m_unknownArray0);
            serializer.WriteObject(m_position);
            serializer.Write(m_unknownArray1);
        }

        private void CreateUnknowns(FileFormat fmt)
        {
            if (fmt.IsPS2)
            {
                m_unknownArray0 = new byte[48];
                m_unknownArray1 = new byte[1140];
            }
            else
            {
                m_unknownArray0 = new byte[52];
                m_unknownArray1 = new byte[1092];
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Boat);
        }

        public bool Equals(Boat other)
        {
            if (other == null)
            {
                return false;
            }

            return m_unknown0.Equals(other.m_unknown0)
                && m_modelId.Equals(other.m_modelId)
                && m_unknown1.Equals(other.m_unknown1)
                && m_unknownArray0.SequenceEqual(other.m_unknownArray0)
                && m_position.Equals(other.m_position)
                && m_unknownArray1.SequenceEqual(other.m_unknownArray1);
        }
    }
}
