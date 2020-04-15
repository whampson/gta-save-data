using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(24)]
    public class AudioScriptObject : SaveDataObject, IEquatable<AudioScriptObject>
    {
        private int m_index;
        private short m_audioId;
        private Vector m_position;
        private int m_audioEntity;

        public int Index
        {
            get { return m_index; }
            set { m_index = value; OnPropertyChanged(); }
        }

        public short AudioId
        {
            get { return m_audioId; }
            set { m_audioId = value; OnPropertyChanged(); }
        }

        public Vector Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public int AudioEntity
        {
            get { return m_audioEntity; }
            set { m_audioEntity = value; OnPropertyChanged(); }
        }


        public AudioScriptObject()
        {
            Position = new Vector();
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            Index = buf.ReadInt32();
            AudioId = buf.ReadInt16();
            buf.Align4Bytes();
            Position = buf.Read<Vector>();
            AudioEntity = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<AudioScriptObject>());
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(Index);
            buf.Write(AudioId);
            buf.Align4Bytes();
            buf.Write(Position);
            buf.Write(AudioEntity);

            Debug.Assert(buf.Offset == SizeOf<AudioScriptObject>());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AudioScriptObject);
        }

        public bool Equals(AudioScriptObject other)
        {
            if (other == null)
            {
                return false;
            }

            return Index.Equals(other.Index)
                && AudioId.Equals(other.AudioId)
                && Position.Equals(other.Position)
                && AudioEntity.Equals(other.AudioEntity);
        }
    }
}
