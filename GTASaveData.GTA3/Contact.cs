using System;

namespace GTASaveData.GTA3
{
    public class Contact : SaveDataObject, IEquatable<Contact>
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

        protected override void ReadData(StreamBuffer buf, SaveDataFormat fmt)
        {
            OnAMissionFlag = buf.ReadInt32();
            BaseBriefId = buf.ReadInt32();
        }

        protected override void WriteData(StreamBuffer buf, SaveDataFormat fmt)
        {
            buf.Write(OnAMissionFlag);
            buf.Write(BaseBriefId);
        }

        protected override int GetSize(SaveDataFormat fmt)
        {
            return 8;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Contact);
        }

        public bool Equals(Contact other)
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
