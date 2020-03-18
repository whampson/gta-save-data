using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    [Size(8)]
    public class ContactInfo : SerializableObject,
        IEquatable<ContactInfo>
    {
        private int m_onAMissionFlag;
        private int m_baseBriefId;

        public int OnAMissionFlag
        {
            get { return m_onAMissionFlag; }
            set { m_onAMissionFlag = value; OnPropertyChanged(); }
        }

        public int BaseBriefId
        {
            get { return m_baseBriefId; }
            set { m_baseBriefId = value; OnPropertyChanged(); }
        }

        public ContactInfo()
        { }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_onAMissionFlag = r.ReadInt32();
            m_baseBriefId = r.ReadInt32();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_onAMissionFlag);
            w.Write(m_baseBriefId);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ContactInfo);
        }

        public bool Equals(ContactInfo other)
        {
            if (other == null)
            {
                return false;
            }

            return m_onAMissionFlag.Equals(other.m_onAMissionFlag)
                && m_baseBriefId.Equals(other.m_baseBriefId);
        }
    }
}
