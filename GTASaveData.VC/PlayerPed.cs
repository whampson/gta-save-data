using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.VC
{
    public class PlayerPed : SaveDataObject,
        IEquatable<PlayerPed>, IDeepClonable<PlayerPed>
    {
        public const int NumWeapons = 10;
        public const int MaxModelNameLength = 22;
        public const int NumTargetableObjects = 4;
        public const int NumTargetableObjectsMobile = 26;

        private PedTypeId m_type;
        private short m_modelIndex;
        private int m_handle;
        private Vector3 m_position;
        private CharCreatedBy m_createdBy;
        private float m_health;
        private float m_armor;
        private ObservableArray<Weapon> m_weapons;
        private float m_maxStamina;
        private ObservableArray<int> m_targetableObjects;
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

        public Vector3 Position
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

        public ObservableArray<Weapon> Weapons
        {
            get { return m_weapons; }
            set { m_weapons = value; OnPropertyChanged(); }
        }

        public float MaxStamina
        {
            get { return m_maxStamina; }
            set { m_maxStamina = value; OnPropertyChanged(); }
        }

        public ObservableArray<int> TargetableObjects
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
            : this(0, (1 << 8) & 1)
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
            Weapons = ArrayHelper.CreateArray<Weapon>(NumWeapons);
            TargetableObjects = ArrayHelper.CreateArray<int>(NumTargetableObjects);
            Position = new Vector3();
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
            Weapons = ArrayHelper.DeepClone(other.Weapons);
            MaxStamina = other.MaxStamina;
            TargetableObjects = ArrayHelper.DeepClone(other.TargetableObjects);
            MaxWantedLevel = other.MaxWantedLevel;
            MaxChaosLevel = other.MaxChaosLevel;
            ModelName = other.ModelName;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            // PC

            // CPed
            buf.Skip(52);
            Position = buf.ReadStruct<Vector3>();
            if (fmt.IsiOS) buf.Skip(283);
            else buf.Skip(288);
            CreatedBy = (CharCreatedBy) buf.ReadByte();
            if (fmt.IsiOS) buf.Skip(496);
            else buf.Skip(499);
            Health = buf.ReadFloat();
            Armor = buf.ReadFloat();
            buf.Skip(172);
            Weapons = buf.ReadArray<Weapon>(NumWeapons, fmt);
            buf.Skip(252);

            // CPlayerPed
            buf.Skip(16);
            MaxStamina = buf.ReadFloat();
            buf.Skip(28);
            int numTargetableObjects = (fmt.IsMobile) ? NumTargetableObjectsMobile : NumTargetableObjects;
            TargetableObjects = buf.ReadArray<int>(numTargetableObjects);
            buf.Skip(164);

            Debug.Assert(buf.Offset == SizeOf<PlayerPed>(fmt));
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            // CPed
            buf.Skip(52);
            buf.Write(Position);
            if (fmt.IsiOS) buf.Skip(283);
            else buf.Skip(288);
            buf.Write((byte) CreatedBy);
            if (fmt.IsiOS) buf.Skip(496);
            else buf.Skip(499);
            buf.Write(Health);
            buf.Write(Armor);
            buf.Skip(172);
            buf.Write(Weapons, NumWeapons);
            buf.Skip(252);

            // CPlayerPed
            buf.Skip(16);
            buf.Write(MaxStamina);
            buf.Skip(28);
            int numTargetableObjects = (fmt.IsMobile) ? NumTargetableObjectsMobile : NumTargetableObjects;
            buf.Write(TargetableObjects, numTargetableObjects);
            buf.Skip(164);

            Debug.Assert(buf.Offset == SizeOf<PlayerPed>(fmt));
        }

        protected override int GetSize(FileFormat fmt)

        {
            if (fmt.IsAndroid) return 0x730;
            if (fmt.IsiOS) return 0x728;
            if (fmt.IsPC) return 0x6D8;
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
                && MaxStamina.Equals(other.MaxStamina)
                && TargetableObjects.SequenceEqual(other.TargetableObjects)
                && MaxWantedLevel.Equals(other.MaxWantedLevel)
                && MaxChaosLevel.Equals(other.MaxChaosLevel)
                && ModelName.Equals(other.ModelName);
        }

        public PlayerPed DeepClone()
        {
            return new PlayerPed(this);
        }
    }

    public enum CharCreatedBy
    {
        Unknown,
        Random,
        Mission
    }
}