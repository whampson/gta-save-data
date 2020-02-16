using GTASaveData.Common;
using GTASaveData.Serialization;
using Newtonsoft.Json;

namespace GTASaveData.GTA3
{
    public abstract class Vehicle : SerializableObject
    {
        protected Array<byte> m_unknownArray0;
        protected Vector3d m_position;
        protected Array<byte> m_unknownArray1;

        [JsonIgnore]
        public Array<byte> UnknownArray0
        {
            get { return m_unknownArray0; }
            set { m_unknownArray0 = value; OnPropertyChanged(); }
        }

        public Vector3d Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public Array<byte> UnknownArray1
        {
            get { return m_unknownArray1; }
            set { m_unknownArray1 = value; OnPropertyChanged(); }
        }

        public Vehicle()
        {
            m_unknownArray0 = new Array<byte>();
            m_unknownArray1 = new Array<byte>();
            m_position = new Vector3d();
        }
    }
}
