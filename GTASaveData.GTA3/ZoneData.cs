using System;
using System.Diagnostics;
using System.Linq;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3
{
    public class ZoneData : SaveDataObject,
        IEquatable<ZoneData>, IDeepClonable<ZoneData>
    {
        public const int MaxNumZones = 50;
        public const int MaxNumZoneInfos = 100;
        public const int MaxNumMapZones = 25;
        public const int MaxNumAudioZones = 36;

        private int m_currentZoneIndex;
        private Level m_currentLevel;
        private short m_findIndex;      // useless field
        private Array<Zone> m_zones;
        private Array<ZoneInfo> m_zoneInfo;
        private short m_numberOfZones;
        private short m_numberOfZoneInfos;
        private Array<Zone> m_mapZones;
        private Array<short> m_audioZones;
        private short m_numberOfMapZones;
        private short m_numberOfAudioZones;

        public int CurrentZoneIndex
        {
            get { return m_currentZoneIndex; }
            set { m_currentZoneIndex = value; OnPropertyChanged(); }
        }

        public Level CurrentLevel
        {
            get { return m_currentLevel; }
            set { m_currentLevel = value; OnPropertyChanged(); }
        }

        [Obsolete("Not used by the game.")]
        public short FindIndex
        {
            get { return m_findIndex; }
            set { m_findIndex = value; OnPropertyChanged(); }
        }

        public Array<Zone> Zones
        {
            get { return m_zones; }
            set { m_zones = value; OnPropertyChanged(); }
        }

        public Array<ZoneInfo> ZoneInfo
        {
            get { return m_zoneInfo; }
            set { m_zoneInfo = value; OnPropertyChanged(); }
        }

        public short NumberOfZones
        {
            get { return m_numberOfZones; }
            set { m_numberOfZones = value; OnPropertyChanged(); }
        }

        public short NumberOfZoneInfos
        {
            get { return m_numberOfZoneInfos; }
            set { m_numberOfZoneInfos = value; OnPropertyChanged(); }
        }

        public Array<Zone> MapZones
        {
            get { return m_mapZones; }
            set { m_mapZones = value; OnPropertyChanged(); }
        }

        public Array<short> AudioZones
        {
            get { return m_audioZones; }
            set { m_audioZones = value; OnPropertyChanged(); }
        }

        public short NumberOfMapZones
        {
            get { return m_numberOfMapZones; }
            set { m_numberOfMapZones = value; OnPropertyChanged(); }
        }

        public short NumberOfAudioZones
        {
            get { return m_numberOfAudioZones; }
            set { m_numberOfAudioZones = value; OnPropertyChanged(); }
        }

        public ZoneData()
        {
            Zones = ArrayHelper.CreateArray<Zone>(MaxNumZones);
            ZoneInfo = ArrayHelper.CreateArray<ZoneInfo>(MaxNumZoneInfos);
            MapZones = ArrayHelper.CreateArray<Zone>(MaxNumMapZones);
            AudioZones = ArrayHelper.CreateArray<short>(MaxNumAudioZones);
        }

        public ZoneData(ZoneData other)
        {
            CurrentZoneIndex = other.CurrentZoneIndex;
            CurrentLevel = other.CurrentLevel;
            FindIndex = other.FindIndex;
            Zones = ArrayHelper.DeepClone(other.Zones);
            ZoneInfo = ArrayHelper.DeepClone(other.ZoneInfo);
            NumberOfZones = other.NumberOfZones;
            NumberOfZoneInfos = other.NumberOfZoneInfos;
            MapZones = ArrayHelper.DeepClone(other.MapZones);
            AudioZones = ArrayHelper.DeepClone(other.AudioZones);
            NumberOfMapZones = other.NumberOfMapZones;
            NumberOfAudioZones = other.NumberOfAudioZones;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            int size = GTA3VCSave.ReadSaveHeader(buf, "ZNS");

            CurrentZoneIndex = buf.ReadInt32();
            CurrentLevel = (Level) buf.ReadInt32();
            FindIndex = buf.ReadInt16();
            buf.ReadInt16();
            Zones = buf.Read<Zone>(MaxNumZones);
            ZoneInfo = buf.Read<ZoneInfo>(MaxNumZoneInfos);
            NumberOfZones = buf.ReadInt16();
            NumberOfZoneInfos = buf.ReadInt16();
            MapZones = buf.Read<Zone>(MaxNumMapZones);
            AudioZones = buf.Read<short>(MaxNumAudioZones);
            NumberOfMapZones = buf.ReadInt16();
            NumberOfAudioZones = buf.ReadInt16();

            Debug.Assert(buf.Offset == size + GTA3VCSave.BlockHeaderSize);
            Debug.Assert(size == SizeOfType<ZoneData>() - GTA3VCSave.BlockHeaderSize);
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            GTA3VCSave.WriteSaveHeader(buf, "ZNS", SizeOfType<ZoneData>() - GTA3VCSave.BlockHeaderSize);

            buf.Write(CurrentZoneIndex);
            buf.Write((int) CurrentLevel);
            buf.Write(FindIndex);
            buf.Write((short) 0);
            buf.Write(Zones.ToArray(), MaxNumZones);
            buf.Write(ZoneInfo.ToArray(), MaxNumZoneInfos);
            buf.Write(NumberOfZones);
            buf.Write(NumberOfZoneInfos);
            buf.Write(MapZones.ToArray(), MaxNumMapZones);
            buf.Write(AudioZones.ToArray(), MaxNumAudioZones);
            buf.Write(NumberOfMapZones);
            buf.Write(NumberOfAudioZones);

            Debug.Assert(buf.Offset == SizeOfType<ZoneData>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x2774;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ZoneData);
        }

        public bool Equals(ZoneData other)
        {
            if (other == null)
            {
                return false;
            }

            return CurrentZoneIndex.Equals(other.CurrentZoneIndex)
                && CurrentLevel.Equals(other.CurrentLevel)
                && FindIndex.Equals(other.FindIndex)
                && Zones.SequenceEqual(other.Zones)
                && ZoneInfo.SequenceEqual(other.ZoneInfo)
                && NumberOfZones.Equals(other.NumberOfZones)
                && NumberOfZoneInfos.Equals(other.NumberOfZoneInfos)
                && MapZones.SequenceEqual(other.MapZones)
                && AudioZones.SequenceEqual(other.AudioZones)
                && NumberOfMapZones.Equals(other.NumberOfMapZones)
                && NumberOfAudioZones.Equals(other.NumberOfAudioZones);
        }

        public ZoneData DeepClone()
        {
            return new ZoneData(this);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
