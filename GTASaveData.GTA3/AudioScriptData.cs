using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class AudioScriptData : SaveDataObject,
        IEquatable<AudioScriptData>, IDeepClonable<AudioScriptData>,
        IEnumerable<AudioScriptObject>
    {
        private ObservableArray<AudioScriptObject> m_audioScriptObjects;

        public ObservableArray<AudioScriptObject> AudioScriptObjects
        {
            get { return m_audioScriptObjects; }
            set { m_audioScriptObjects = value; OnPropertyChanged(); }
        }

        public AudioScriptObject this[int i]
        {
            get { return AudioScriptObjects[i]; }
            set { AudioScriptObjects[i] = value; OnPropertyChanged(); }
        }

        public AudioScriptData()
        {
            AudioScriptObjects = new ObservableArray<AudioScriptObject>();
        }

        public AudioScriptData(AudioScriptData other)
        {
            AudioScriptObjects = ArrayHelper.DeepClone(other.AudioScriptObjects);
        }

        protected override void ReadData(DataBuffer buf, FileType fmt)
        {
            int size = SaveFileGTA3VC.ReadBlockHeader(buf, out string tag);
            Debug.Assert(tag == "AUD");

            int count = buf.ReadInt32();
            AudioScriptObjects = buf.ReadArray<AudioScriptObject>(count);

            Debug.Assert(buf.Offset == size + SaveFileGTA3VC.BlockHeaderSize);
            Debug.Assert(size == SizeOf(this) - SaveFileGTA3VC.BlockHeaderSize);
        }

        protected override void WriteData(DataBuffer buf, FileType fmt)
        {
            SaveFileGTA3VC.WriteBlockHeader(buf, "AUD", SizeOf(this) - SaveFileGTA3VC.BlockHeaderSize);

            buf.Write(AudioScriptObjects.Count);
            buf.Write(AudioScriptObjects);

            Debug.Assert(buf.Offset == SizeOf(this));
        }

        protected override int GetSize(FileType fmt)
        {
            return SizeOf<AudioScriptObject>() * AudioScriptObjects.Count
                + SaveFileGTA3VC.BlockHeaderSize
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

        public AudioScriptData DeepClone()
        {
            return new AudioScriptData(this);
        }

        public IEnumerator<AudioScriptObject> GetEnumerator()
        {
            return AudioScriptObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
