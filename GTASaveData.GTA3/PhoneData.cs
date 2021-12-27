using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class PhoneData : SaveDataObject,
        IEquatable<PhoneData>, IDeepClonable<PhoneData>,
        IEnumerable<Phone>
    {
        public const int MaxNumPhones = 50;

        private int m_numPhones;
        private int m_numActivePhones;
        private ObservableArray<Phone> m_phones;

        public int NumPhones
        {
            get { return m_numPhones; }
            set { m_numPhones = value; OnPropertyChanged(); }
        }

        public int NumActivePhones
        {
            get { return m_numActivePhones; }
            set { m_numActivePhones = value; OnPropertyChanged(); }
        }

        public ObservableArray<Phone> Phones
        {
            get { return m_phones; }
            set { m_phones = value; OnPropertyChanged(); }
        }

        public Phone this[int i]
        {
            get { return Phones[i]; }
            set { Phones[i] = value; OnPropertyChanged(); }
        }

        public PhoneData()
        {
            Phones = ArrayHelper.CreateArray<Phone>(MaxNumPhones);
        }

        public PhoneData(PhoneData other)
        {
            NumPhones = other.NumPhones;
            NumActivePhones = other.NumActivePhones;
            Phones = ArrayHelper.DeepClone(other.Phones);
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            NumPhones = buf.ReadInt32();
            NumActivePhones = buf.ReadInt32();
            Phones = buf.ReadArray<Phone>(MaxNumPhones);

            Debug.Assert(buf.Offset == SizeOf<PhoneData>());
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            buf.Write(NumPhones);
            buf.Write(NumActivePhones);
            buf.Write(Phones, MaxNumPhones);

            Debug.Assert(buf.Offset == SizeOf<PhoneData>());
        }

        protected override int GetSize(SerializationParams prm)
        {
            return 0xA30;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PhoneData);
        }

        public bool Equals(PhoneData other)
        {
            if (other == null)
            {
                return false;
            }

            return NumPhones.Equals(other.NumPhones)
                && NumActivePhones.Equals(other.NumActivePhones)
                && Phones.SequenceEqual(other.Phones);
        }

        public PhoneData DeepClone()
        {
            return new PhoneData(this);
        }

        public IEnumerator<Phone> GetEnumerator()
        {
            return Phones.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
