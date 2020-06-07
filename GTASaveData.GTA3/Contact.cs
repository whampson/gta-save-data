using System;

namespace GTASaveData.GTA3
{
    public class Contact : SaveDataObject,
        IEquatable<Contact>, IDeepClonable<Contact>
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

        public Contact()
        { }

        public Contact(Contact other)
        {
            OnAMissionFlag = other.OnAMissionFlag;
            BaseBriefId = other.BaseBriefId;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            OnAMissionFlag = buf.ReadInt32();
            BaseBriefId = buf.ReadInt32();
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(OnAMissionFlag);
            buf.Write(BaseBriefId);
        }

        protected override int GetSize(FileFormat fmt)
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

        public Contact DeepClone()
        {
            return new Contact(this);
        }
    }
}
