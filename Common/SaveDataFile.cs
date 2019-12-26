using Newtonsoft.Json;
using System;
using System.IO;

namespace GTASaveData
{
    public abstract class SaveDataFile : SaveDataObject
    {
        private SystemType m_targetSystem;  // TODO: keep this field in base class?
        private PaddingMode m_paddingMode;
        private byte[] m_paddingSequence;

        [JsonIgnore]
        public PaddingMode FilePaddingMode
        {
            get { return m_paddingMode; }
            set { m_paddingMode = value; }
        }

        [JsonIgnore]
        public byte[] FilePaddingSequence
        {
            get { return m_paddingSequence; }
            set { m_paddingSequence = value; }
        }

        [JsonIgnore]
        protected SystemType TargetSystem
        {
            get { return m_targetSystem; }
            set { m_targetSystem = value; }
        }

        protected SaveDataFile()
        {
            m_paddingMode = PaddingMode.Zeros;
            m_paddingSequence = null;
            m_targetSystem = SystemType.Unspecified;
        }

        public void Store(string path, SystemType system)
        {
            byte[] data = Serialize(this, system);
            File.WriteAllBytes(path, data);
        }

        protected SaveDataSerializer CreateSerializer(Stream baseStream)
        {
            return new SaveDataSerializer(baseStream)
            {
                PaddingMode = m_paddingMode,
                PaddingSequence = m_paddingSequence
            };
        }

        protected T Deserialize<T>(byte[] buffer,
            SystemType system = SystemType.Unspecified,
            int length = 0,
            bool unicode = false)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            using (SaveDataSerializer s = CreateSerializer(new MemoryStream(buffer)))
            {
                return s.GenericRead<T>(system, length, unicode);
            }
        }

        protected byte[] Serialize<T>(T obj,
            SystemType system = SystemType.Unspecified,
            int length = 0,
            bool unicode = false)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            using (MemoryStream m = new MemoryStream())
            {
                using (SaveDataSerializer s = CreateSerializer(m))
                {
                    s.GenericWrite(obj, system, length, unicode);
                }

                return m.ToArray();
            }
        }
    }
}
