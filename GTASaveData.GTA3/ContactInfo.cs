using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    [Size(8)]
    public class ContactInfo : GTAObject,
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

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            m_onAMissionFlag = buf.ReadInt32();
            m_baseBriefId = buf.ReadInt32();
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(m_onAMissionFlag);
            buf.Write(m_baseBriefId);
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
