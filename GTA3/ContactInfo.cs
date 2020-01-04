using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    public sealed class ContactInfo : SaveDataObject,
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

        private ContactInfo(SaveDataSerializer serializer, FileFormat format)
        {
            m_onAMissionFlag = serializer.ReadInt32();
            m_baseBriefId = serializer.ReadInt32();
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.Write(m_onAMissionFlag);
            serializer.Write(m_baseBriefId);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
