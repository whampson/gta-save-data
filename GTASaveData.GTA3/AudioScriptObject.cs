using System;
using System.Diagnostics;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class AudioScriptObject : SaveDataObject,
        IEquatable<AudioScriptObject>, IDeepClonable<AudioScriptObject>
    {
        private int m_index;
        private short m_audioId;
        private Vector3 m_position;
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

        public Vector3 Position
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
            Position = new Vector3();
        }

        public AudioScriptObject(AudioScriptObject other)
        {
            Index = other.Index;
            AudioId = other.AudioId;
            Position = other.Position;
            AudioEntity = other.AudioEntity;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            Index = buf.ReadInt32();
            AudioId = buf.ReadInt16();
            buf.ReadInt16();
            Position = buf.ReadStruct<Vector3>();
            AudioEntity = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOfType<AudioScriptObject>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(Index);
            buf.Write(AudioId);
            buf.Write((short) 0);
            buf.Write(Position);
            buf.Write(AudioEntity);

            Debug.Assert(buf.Offset == SizeOfType<AudioScriptObject>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 24;
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

        public AudioScriptObject DeepClone()
        {
            return new AudioScriptObject(this);
        }
    }
}
