using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class AudioScriptData : SaveDataObject, IEquatable<AudioScriptData>
    {
        private Array<AudioScriptObject> m_audioScriptObjects;
        public Array<AudioScriptObject> AudioScriptObjects
        {
            get { return m_audioScriptObjects; }
            set { m_audioScriptObjects = value; OnPropertyChanged(); }
        }

        public AudioScriptData()
        {
            AudioScriptObjects = new Array<AudioScriptObject>();
        }

        protected override void ReadData(StreamBuffer buf, SaveDataFormat fmt)
        {
            int size = GTA3Save.ReadSaveHeader(buf, "AUD");

            int count = buf.ReadInt32();
            AudioScriptObjects = buf.Read<AudioScriptObject>(count);

            Debug.Assert(buf.Offset == size + GTA3Save.SaveHeaderSize);
            Debug.Assert(size == SizeOf(this) - GTA3Save.SaveHeaderSize);
        }

        protected override void WriteData(StreamBuffer buf, SaveDataFormat fmt)
        {
            GTA3Save.WriteSaveHeader(buf, "AUD", SizeOf(this) - GTA3Save.SaveHeaderSize);

            buf.Write(AudioScriptObjects.Count);
            buf.Write(AudioScriptObjects.ToArray());

            Debug.Assert(buf.Offset == SizeOf(this));
        }

        protected override int GetSize(SaveDataFormat fmt)
        {
            return SizeOf<AudioScriptObject>() * AudioScriptObjects.Count
                + GTA3Save.SaveHeaderSize
                + sizeof(int);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AudioScriptData);
        }

        public bool Equals(AudioScriptData other)
        {
            if (other == null)
            {
                return false;
            }

            return AudioScriptObjects.SequenceEqual(other.AudioScriptObjects);
        }
    }
}
