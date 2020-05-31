using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class PlayerPed : SaveDataObject, IEquatable<PlayerPed>
    {
        public static class Limits
        {
            public const int NumWeapons = 13;
            public const int MaxModelNameLength = 24;
            public const int NumTargetableObjects = 4;
        }

        private PedTypeId m_type;
        private short m_modelIndex;
        private int m_handle;
        private Vector3D m_position;
        private CharCreatedBy m_createdBy;
        private float m_health;
        private float m_armor;
        private Array<Weapon> m_weapons;
        private byte m_maxWeaponTypeAllowed;
        private float m_maxStamina;
        private Array<int> m_targetableObjects;
        private int m_maxWantedLevel;
        private int m_maxChaosLevel;
        private string m_modelName;

        public PedTypeId Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public short ModelIndex
        {
            get { return m_modelIndex; }
            set { m_modelIndex = value; OnPropertyChanged(); }
        }

        public int Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        public Vector3D Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public CharCreatedBy CreatedBy
        {
            get { return m_createdBy; }
            set { m_createdBy = value; OnPropertyChanged(); }
        }

        public float Health
        {
            get { return m_health; }
            set { m_health = value; OnPropertyChanged(); }
        }

        public float Armor
        {
            get { return m_armor; }
            set { m_armor = value; OnPropertyChanged(); }
        }

        public Array<Weapon> Weapons
        {
            get { return m_weapons; }
            set { m_weapons = value; OnPropertyChanged(); }
        }

        public byte MaxWeaponTypeAllowed
        {
            get { return m_maxWeaponTypeAllowed; }
            set { m_maxWeaponTypeAllowed = value; OnPropertyChanged(); }
        }

        public float MaxStamina
        {
            get { return m_maxStamina; }
            set { m_maxStamina = value; OnPropertyChanged(); }
        }

        public Array<int> TargetableObjects
        {
            get { return m_targetableObjects; }
            set { m_targetableObjects = value; OnPropertyChanged(); }
        }


        public int MaxWantedLevel
        {
            get { return m_maxWantedLevel; }
            set { m_maxWantedLevel = value; OnPropertyChanged(); }
        }

        public int MaxChaosLevel
        {
            get { return m_maxChaosLevel; }
            set { m_maxChaosLevel = value; OnPropertyChanged(); }
        }

        public string ModelName
        {
            get { return m_modelName; }
            set { m_modelName = value; OnPropertyChanged(); }
        }

        public PlayerPed()
            : this(0, 257)
        { }

        public PlayerPed(short model, int handle)
        {
            ModelIndex = model;
            Handle = handle;
            Type = PedTypeId.Player1;
            CreatedBy = CharCreatedBy.Mission;
            ModelName = "player";
            Health = 100;
            MaxStamina = 150;
            Weapons = new Array<Weapon>();
            TargetableObjects = new Array<int>();
        }

        public PlayerPed(PlayerPed other)
        {
            Type = other.Type;
            ModelIndex = other.ModelIndex;
            Handle = other.Handle;
            Position = other.Position;
            CreatedBy = other.CreatedBy;
            Health = other.Health;
            Armor = other.Armor;
            Weapons = new Array<Weapon>(other.Weapons);
            MaxWeaponTypeAllowed = other.MaxWeaponTypeAllowed;
            MaxStamina = other.MaxStamina;
            TargetableObjects = new Array<int>(other.TargetableObjects);
            MaxWantedLevel = other.MaxWantedLevel;
            MaxChaosLevel = other.MaxChaosLevel;
            ModelName = other.ModelName;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Skip(52);
            Position = buf.Read<Vector3D>();
            buf.Skip(288);
            CreatedBy = (CharCreatedBy) buf.ReadByte();
            buf.Skip(351);
            Health = buf.ReadFloat();
            Armor = buf.ReadFloat();
            buf.Skip(148);
            Weapons = buf.Read<Weapon>(Limits.NumWeapons, fmt);
            buf.Skip(5);
            MaxWeaponTypeAllowed = buf.ReadByte();
            buf.Skip(178);
            MaxStamina = buf.ReadFloat();
            buf.Skip(28);
            TargetableObjects = buf.Read<int>(Limits.NumTargetableObjects);
            buf.Skip(116);

            Debug.Assert(buf.Offset == SizeOfType<PlayerPed>(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Skip(52);
            buf.Write(Position);
            buf.Skip(288);
            buf.Write((byte) CreatedBy);
            buf.Skip(351);
            buf.Write(Health);
            buf.Write(Armor);
            buf.Skip(148);
            buf.Write(Weapons.ToArray(), Limits.NumWeapons);
            buf.Skip(5);
            buf.Write(MaxWeaponTypeAllowed);
            buf.Skip(178);
            buf.Write(MaxStamina);
            buf.Skip(28);
            buf.Write(TargetableObjects.ToArray(), Limits.NumTargetableObjects);
            buf.Skip(116);

            Debug.Assert(buf.Offset == SizeOfType<PlayerPed>(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsPC)
            {
                return 0x5F0;
            }

            throw SizeNotDefined(fmt);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PlayerPed);
        }

        public bool Equals(PlayerPed other)
        {
            if (other == null)
            {
                return false;
            }

            return Type.Equals(other.Type)
                && ModelIndex.Equals(other.ModelIndex)
                && Handle.Equals(other.Handle)
                && Position.Equals(other.Position)
                && CreatedBy.Equals(other.CreatedBy)
                && Health.Equals(other.Health)
                && Armor.Equals(other.Armor)
                && Weapons.SequenceEqual(other.Weapons)
                && MaxWeaponTypeAllowed.Equals(other.MaxWeaponTypeAllowed)
                && MaxStamina.Equals(other.MaxStamina)
                && TargetableObjects.SequenceEqual(other.TargetableObjects)
                && MaxWantedLevel.Equals(other.MaxWantedLevel)
                && MaxChaosLevel.Equals(other.MaxChaosLevel)
                && ModelName.Equals(other.ModelName);
        }
    }

    public enum CharCreatedBy
    {
        Unknown,
        Random,
        Mission
    }
}