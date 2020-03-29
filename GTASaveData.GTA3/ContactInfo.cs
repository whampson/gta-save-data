using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    [Size(8)]
    public class ContactInfo : SaveDataObject, IEquatable<ContactInfo>
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

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            OnAMissionFlag = buf.ReadInt32();
            BaseBriefId = buf.ReadInt32();
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(OnAMissionFlag);
            buf.Write(BaseBriefId);
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

            return OnAMissionFlag.Equals(other.OnAMissionFlag)
                && BaseBriefId.Equals(other.BaseBriefId);
        }
    }
}
