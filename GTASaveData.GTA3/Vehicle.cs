using GTASaveData.Serialization;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GTASaveData.GTA3
{
    public sealed class Vehicle : SaveDataObject,
        IEquatable<Vehicle>
    {
        public static class Limits
        {
            public const int UnknownArray0Size = 52;
            public const int UnknownArray0SizePS2 = 48;
            public const int UnknownArray1Size = 1384;
            public const int UnknownArray1SizePS2 = 1556;
        }

        private int m_unknown0;
        private int m_modelId;  // TODO: enum
        private int m_unknown1;
        private ObservableCollection<byte> m_unknownArray0;
        private Vector3d m_position;
        private ObservableCollection<byte> m_unknownArray1;

        public int Unknown0
        {
            get { return m_unknown0; }
            set { m_unknown0 = value; OnPropertyChanged(); }
        }

        public int ModelId
        {
            get { return m_modelId; }
            set { m_modelId = value; OnPropertyChanged(); }
        }

        public int Unknown1
        {
            get { return m_unknown1; }
            set { m_unknown1 = value; OnPropertyChanged(); }
        }

        public ObservableCollection<byte> UnknownArray0
        {
            get { return UnknownArray0; }
            set { UnknownArray0 = value; OnPropertyChanged(); }
        }

        public Vector3d Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

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
            int unknownArray0Size = (format.IsPS2)
                ? Limits.UnknownArray0SizePS2
                : Limits.UnknownArray0Size;
            int unknownArray1Size = (format.IsPS2)
                ? Limits.UnknownArray1SizePS2
                : Limits.UnknownArray1Size;

            m_unknown0 = serializer.ReadInt32();
            m_modelId = serializer.ReadInt16();
            m_unknown1 = serializer.ReadInt32();
            m_unknownArray0 = new ObservableCollection<byte>(serializer.ReadBytes(unknownArray0Size));
            m_position = serializer.ReadObject<Vector3d>();
            m_unknownArray1 = new ObservableCollection<byte>(serializer.ReadBytes(unknownArray1Size));
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            int unknownArray0Size = (format.IsPS2)
                ? Limits.UnknownArray0SizePS2
                : Limits.UnknownArray0Size;
            int unknownArray1Size = (format.IsPS2)
                ? Limits.UnknownArray1SizePS2
                : Limits.UnknownArray1Size;

            serializer.Write(m_unknown0);
            serializer.Write((short) m_modelId);
            serializer.Write(m_unknown1);
            serializer.WriteArray(m_unknownArray0.ToArray(), unknownArray0Size);
            serializer.WriteObject(m_position);
            serializer.WriteArray(m_unknownArray1.ToArray(), unknownArray1Size);
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

            return m_unknown0.Equals(other.m_unknown0)
                && m_modelId.Equals(other.m_modelId)
                && m_unknown1.Equals(other.m_unknown1)
                && m_unknownArray0.SequenceEqual(other.m_unknownArray0)
                && m_position.Equals(other.m_position)
                && m_unknownArray1.SequenceEqual(other.m_unknownArray1);
        }
    }
}
