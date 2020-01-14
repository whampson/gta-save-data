using GTASaveData.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class Vehicle : SaveDataObject,
        IEquatable<Vehicle>
    {
        public static class Limits
        {
            public static int GetUnknownArray0Size(FileFormat fmt)
            {
                return (fmt.IsPS2) ? 48 : 52;
            }

            public static int GetUnknownArray1Size(FileFormat fmt)
            {
                int size = 0;
                if (fmt.IsPS2)
                {
                    size = 1556;
                }
                else if (fmt.IsPC || fmt.IsXbox)
                {
                    size = 1384;
                }
                else if (fmt.IsMobile)
                {
                    size = 1388;
                }

                return size;
            }
        }

        protected ObservableCollection<byte> m_unknownArray0;
        protected Vector3d m_position;
        protected ObservableCollection<byte> m_unknownArray1;

        [JsonIgnore]
        public ObservableCollection<byte> UnknownArray0
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
        public ObservableCollection<byte> UnknownArray1
        {
            get { return m_unknownArray1; }
            set { m_unknownArray1 = value; OnPropertyChanged(); }
        }

        public Vehicle()
        {
            m_unknownArray0 = new ObservableCollection<byte>();
            m_unknownArray1 = new ObservableCollection<byte>();
            m_position = new Vector3d();
        }

        private Vehicle(SaveDataSerializer serializer, FileFormat format)
        {
            m_unknownArray0 = new ObservableCollection<byte>(serializer.ReadBytes(Limits.GetUnknownArray0Size(format)));
            m_position = serializer.ReadObject<Vector3d>();
            m_unknownArray1 = new ObservableCollection<byte>(serializer.ReadBytes(Limits.GetUnknownArray1Size(format)));
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.WriteArray(m_unknownArray0.ToArray(), Limits.GetUnknownArray0Size(format));
            serializer.WriteObject(m_position);
            serializer.WriteArray(m_unknownArray1.ToArray(), Limits.GetUnknownArray1Size(format));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vehicle);
        }

        public bool Equals(Vehicle other)
        {
            if (other == null)
            {
                return false;
            }

            return m_unknownArray0.SequenceEqual(other.m_unknownArray0)
                && m_position.Equals(other.m_position)
                && m_unknownArray1.SequenceEqual(other.m_unknownArray1);
        }
    }
}
