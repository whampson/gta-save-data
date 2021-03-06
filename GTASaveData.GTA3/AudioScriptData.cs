﻿using System;
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
        private Array<AudioScriptObject> m_audioScriptObjects;

        public Array<AudioScriptObject> AudioScriptObjects
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
            AudioScriptObjects = new Array<AudioScriptObject>();
        }

        public AudioScriptData(AudioScriptData other)
        {
            AudioScriptObjects = ArrayHelper.DeepClone(other.AudioScriptObjects);
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            int size = SaveFileGTA3VC.ReadBlockHeader(buf, "AUD");

            int count = buf.ReadInt32();
            AudioScriptObjects = buf.ReadArray<AudioScriptObject>(count);

            Debug.Assert(buf.Offset == size + SaveFileGTA3VC.BlockHeaderSize);
            Debug.Assert(size == SizeOfObject(this) - SaveFileGTA3VC.BlockHeaderSize);
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            SaveFileGTA3VC.WriteBlockHeader(buf, "AUD", SizeOfObject(this) - SaveFileGTA3VC.BlockHeaderSize);

            buf.Write(AudioScriptObjects.Count);
            buf.Write(AudioScriptObjects);

            Debug.Assert(buf.Offset == SizeOfObject(this));
        }

        protected override int GetSize(FileFormat fmt)
        {
            return SizeOfType<AudioScriptObject>() * AudioScriptObjects.Count
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
