﻿using GTASaveData.Common;
using GTASaveData.Serialization;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GTASaveData.GTA3
{
    public sealed class Boat : Vehicle,
        IEquatable<Boat>
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
                    size = 1140;
                }
                else if (fmt.IsPC || fmt.IsXbox)
                {
                    size = 1092;
                }
                else if (fmt.IsMobile)
                {
                    size = 1096;
                }

                return size;
            }
        }

        public Boat()
            : base()
        { }

        private Boat(SaveDataSerializer serializer, FileFormat format)
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
            return Equals(obj as Boat);
        }

        public bool Equals(Boat other)
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
