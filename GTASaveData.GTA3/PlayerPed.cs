using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// A saved player ped.
    /// </summary>
    /// <remarks>
    /// This is the saved version of the <c>CPlayerPed</c> class from the game.
    /// In the save file, the entirety of the <c>CPlayerPed</c> object is saved,
    /// but only a handful of fields are restored upon load. This class contains
    /// only those fields.
    /// </remarks>
    public class PlayerPed : SaveDataObject,
        IEquatable<PlayerPed>, IDeepClonable<PlayerPed>
    {
        /// <summary>
        /// The number of elements in the weapon array.
        /// </summary>
        public const int NumWeapons = 13;

        /// <summary>
        /// The maximum length of the model name including the NULL terminator.
        /// </summary>
        public const int ModelNameLength = 24;

        /// <summary>
        /// The number of elements in the targetable objects array.
        /// </summary>
        public const int NumTargetableObjects = 4;

        private PedTypeId m_type;
        private short m_modelIndex;
        private int m_handle;
        private Vector3 m_position;
        private CharCreatedBy m_createdBy;
        private float m_health;
        private float m_armor;
        private ObservableArray<Weapon> m_weapons;
        private byte m_maxWeaponTypeAllowed;
        private float m_maxStamina;
        private ObservableArray<int> m_targetableObjects;
        private int m_maxWantedLevel;
        private int m_maxChaosLevel;
        private string m_modelName;

        /// <summary>
        /// The ped type.
        /// </summary>
        /// <remarks>
        /// Should almost always be <see cref="PedTypeId.Player1"/>.
        /// Game may crash otherwise.
        /// </remarks>
        public PedTypeId Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The player model index.
        /// </summary>
        /// <remarks>
        /// Should almost always be 0 (<c>MI_PLAYER</c>).
        /// </remarks>
        public short ModelIndex
        {
            get { return m_modelIndex; }
            set { m_modelIndex = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The ped pool handle.
        /// </summary>
        /// <remarks>
        /// This is effectively the index in the pool of this player ped
        /// plus some flags. This value is largely irrelevent, but it should
        /// be unique and not too large. It's computed as follows: 
        /// <code>handle = (index &lt;&lt; 8) | flags</code>
        /// </remarks>
        public int Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The coordinates of this player in the game world.
        /// </summary>
        public Vector3 Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Used to distinguish between characters created for missions vs.
        /// random peds.
        /// </summary>
        /// <remarks>
        /// Should be <see cref="CharCreatedBy.Mission"/> for player peds, but
        /// I don't think changing it has any real effect for player peds.
        /// </remarks>
        public CharCreatedBy CharCreatedBy
        {
            get { return m_createdBy; }
            set { m_createdBy = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The player ped's current health.
        /// </summary>
        public float Health
        {
            get { return m_health; }
            set { m_health = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The player ped's current armor.
        /// </summary>
        public float Armor
        {
            get { return m_armor; }
            set { m_armor = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The player ped's weapon inventory.
        /// </summary>
        /// <seealso cref="GiveWeapon(WeaponType, int)"/>
        /// <seealso cref="GetWeapon(WeaponType)"/>
        /// <seealso cref="SetWeapon(WeaponType, Weapon)"/>
        public ObservableArray<Weapon> Weapons
        {
            get { return m_weapons; }
            set { m_weapons = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// This value is useless pretty much useless. It's not really
        /// used for anything in-game.
        /// </summary>
        public byte MaxWeaponTypeAllowed
        {
            get { return m_maxWeaponTypeAllowed; }
            set { m_maxWeaponTypeAllowed = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The player ped's maximum sprint energy. In a normal game, this
        /// ranges from 150 to 1000.
        /// </summary>
        public float MaxStamina
        {
            get { return m_maxStamina; }
            set { m_maxStamina = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// List of targetable objects. Each entry is an object handle.
        /// A max of 4 entries is allowed.
        /// </summary>
        /// <remarks>
        /// Entires are added to this list by opcode 035D (<c>MAKE_OBJECT_TARGETTABLE</c>).
        /// This is only used in a few places in the vanilla script, namely for the targets
        /// behind AmmuNation in Portland, the evidence pieces in <i>Evidence Dash</i>, and the
        /// full-body cast in <i>Plaster Blaster</i>.
        /// </remarks>
        public ObservableArray<int> TargetableObjects
        {
            get { return m_targetableObjects; }
            set { m_targetableObjects = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The maximum wanted level. Valid values: 0-6
        /// </summary>
        /// <seealso cref="SetMaximumWantedLevel(int)"/>
        public int MaxWantedLevel
        {
            get { return m_maxWantedLevel; }
            set { m_maxWantedLevel = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The maximum number of chaos points that can be attained.
        /// </summary>
        /// <remarks>
        /// Chaos points are responsible for determining the current
        /// wanted level. Each crime committed adds to the chaos points.
        /// Over time, chaos decreases if the player is out of sight of
        /// the cops and does not commit any crimes. Use
        /// <see cref="SetMaximumWantedLevel(int)"/> to set the maximum
        /// chaos to the default value for a given wanted level.
        /// </remarks>
        /// <seealso cref="SetMaximumWantedLevel(int)"/>
        public int MaxChaos
        {
            get { return m_maxChaosLevel; }
            set { m_maxChaosLevel = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The name of the ped model to use for this player ped.
        /// </summary>
        /// <remarks>
        /// Only 'Special' models are allowed. See
        /// <see href="https://gtamods.com/wiki/023C#List_of_valid_models"/>.
        /// <para>
        /// NOTE: starting a mission with any model other than "player"
        /// will crash the game when the cutscene loads!
        /// </para>
        /// </remarks>
        public string ModelName
        {
            get { return m_modelName; }
            set { m_modelName = value; OnPropertyChanged(); }
        }

        public PlayerPed()
        {
            ModelName = "player";
            Weapons = ArrayHelper.CreateArray<Weapon>(NumWeapons);
            TargetableObjects = ArrayHelper.CreateArray<int>(NumTargetableObjects);
        }

        public PlayerPed(PlayerPed other)
        {
            Type = other.Type;
            ModelIndex = other.ModelIndex;
            Handle = other.Handle;
            Position = other.Position;
            CharCreatedBy = other.CharCreatedBy;
            Health = other.Health;
            Armor = other.Armor;
            Weapons = ArrayHelper.DeepClone(other.Weapons);
            MaxWeaponTypeAllowed = other.MaxWeaponTypeAllowed;
            MaxStamina = other.MaxStamina;
            TargetableObjects = ArrayHelper.DeepClone(other.TargetableObjects);
            MaxWantedLevel = other.MaxWantedLevel;
            MaxChaos = other.MaxChaos;
            ModelName = other.ModelName;
        }

        /// <summary>
        /// Gives the player the specified amount of ammo for a given
        /// weapon slot.
        /// </summary>
        /// <returns>The <see cref="Weapon"/> object for the given slot.</returns>
        public Weapon GiveWeapon(WeaponType slot, int ammo)
        {
            Weapon w = Weapons[(int) slot];
            w.AmmoTotal += ammo;

            // Fix weapon type and state
            if (w.Type != slot)
            {
                w.Type = slot;
            }
            if (w.State != WeaponState.Ready)
            {
                w.State = WeaponState.Ready;
            }

            return w;
        }

        /// <summary>
        /// Gets the <see cref="Weapon"/> object for a given weapon slot.
        /// </summary>
        public Weapon GetWeapon(WeaponType slot)
        {
            return Weapons[(int) slot];
        }

        /// <summary>
        /// Sets the <see cref="Weapon"/> object for a given weapon slot.
        /// </summary>
        public void SetWeapon(WeaponType slot, Weapon w)
        {
            if (w.Type != slot)
            {
                w.Type = slot;
            }
            Weapons[(int) slot] = w;
        }

        /// <summary>
        /// Clears all weapon slots.
        /// </summary>
        public void ClearWeapons()
        {
            for (int i = 0; i < NumWeapons; i++)
            {
                Weapon w = GetWeapon((WeaponType) i);
                w.Type = (WeaponType) i;
                w.State = WeaponState.Ready;
                w.AmmoInClip = 0;
                w.AmmoTotal = 0;
                w.Timer = 0;
                w.AddRotOffset = false;
            }
        }

        /// <summary>
        /// Sets the maximum wanted level and maximum chaos. Valid values: 0-6.
        /// </summary>
        /// <remarks>
        /// The maximum chaos will be set to the game's default value for each
        /// wanted level.
        /// </remarks>
        public void SetMaximumWantedLevel(int level)
        {
            switch (level)
            {
                case 0:
                    MaxChaos = 0;
                    MaxWantedLevel = 0;
                    break;
                case 1:
                    MaxChaos = 120;
                    MaxWantedLevel = 1;
                    break;
                case 2:
                    MaxChaos = 300;
                    MaxWantedLevel = 2;
                    break;
                case 3:
                    MaxChaos = 600;
                    MaxWantedLevel = 3;
                    break;
                case 4:
                    MaxChaos = 1200;
                    MaxWantedLevel = 4;
                    break;
                case 5:
                    MaxChaos = 2400;
                    MaxWantedLevel = 5;
                    break;
                case 6:
                    MaxChaos = 4800;
                    MaxWantedLevel = 6;
                    break;
            }
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            GTA3SaveParams p = (GTA3SaveParams) prm;

            if (!p.IsPS2) buf.Skip(4);
            buf.Skip(48);
            Position = buf.ReadStruct<Vector3>();
            if (p.IsPC || p.IsXbox) buf.Skip(288);
            if (p.IsiOS) buf.Skip(292);
            if (p.IsAndroid || p.IsDE) buf.Skip(296);     // TODO: need to delineate between IsPC and FlagDE
            if (p.IsPS2JP) buf.Skip(324);
            else if (p.IsPS2) buf.Skip(356);
            CharCreatedBy = (CharCreatedBy) buf.ReadByte();
            buf.Skip(351);
            if (p.IsXbox) buf.Skip(4);
            Health = buf.ReadFloat();
            Armor = buf.ReadFloat();
            if (p.IsPS2) buf.Skip(164);
            else buf.Skip(148);
            Weapons = buf.ReadArray<Weapon>(NumWeapons, prm);
            buf.Skip(5);
            MaxWeaponTypeAllowed = buf.ReadByte();
            buf.Skip(178);
            if (p.IsMobile || p.IsDE) buf.Skip(4);
            if (p.IsPS2) buf.Skip(8);
            MaxStamina = buf.ReadFloat();
            buf.Skip(28);
            TargetableObjects = buf.ReadArray<int>(NumTargetableObjects);
            if (p.IsPC || p.IsXbox) buf.Skip(116);
            if (p.IsMobile || p.IsDE) buf.Skip(144);
            if (p.IsPS2) buf.Skip(16);

            Debug.Assert(buf.Offset == SizeOf<PlayerPed>(prm));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            GTA3SaveParams p = (GTA3SaveParams) prm;

            if (!p.IsPS2) buf.Skip(4);
            buf.Skip(48);
            buf.Write(Position);
            if (p.IsPC || p.IsXbox) buf.Skip(288);
            if (p.IsiOS) buf.Skip(292);
            if (p.IsAndroid || p.IsDE) buf.Skip(296);
            if (p.IsPS2JP) buf.Skip(324);
            else if (p.IsPS2) buf.Skip(356);
            buf.Write((byte) CharCreatedBy);
            buf.Skip(351);
            if (p.IsXbox) buf.Skip(4);
            buf.Write(Health);
            buf.Write(Armor);
            if (p.IsPS2) buf.Skip(164);
            else buf.Skip(148);
            buf.Write(Weapons, prm, NumWeapons);
            buf.Skip(5);
            buf.Write(MaxWeaponTypeAllowed);
            buf.Skip(178);
            if (p.IsMobile || p.IsDE) buf.Skip(4);
            if (p.IsPS2) buf.Skip(8);
            buf.Write(MaxStamina);
            buf.Skip(28);
            buf.Write(TargetableObjects, NumTargetableObjects);
            if (p.IsPC || p.IsXbox) buf.Skip(116);
            if (p.IsMobile || p.IsDE) buf.Skip(144);
            if (p.IsPS2) buf.Skip(16);

            Debug.Assert(buf.Offset == SizeOf<PlayerPed>(prm));
        }

        protected override int GetSize(SerializationParams prm)
        {
            // TODO: calculate...?

            GTA3SaveParams p = (GTA3SaveParams) prm;

            if (p.IsPS2JP) return 0x590;
            if (p.IsPS2) return 0x5B0;
            if (p.IsPC) return 0x5F0;
            if (p.IsXbox) return 0x5F4;
            if (p.IsiOS) return 0x614;
            if (p.IsAndroid || p.IsDE) return 0x618;
            throw SizeNotDefined(p);
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
                && CharCreatedBy.Equals(other.CharCreatedBy)
                && Health.Equals(other.Health)
                && Armor.Equals(other.Armor)
                && Weapons.SequenceEqual(other.Weapons)
                && MaxWeaponTypeAllowed.Equals(other.MaxWeaponTypeAllowed)
                && MaxStamina.Equals(other.MaxStamina)
                && TargetableObjects.SequenceEqual(other.TargetableObjects)
                && MaxWantedLevel.Equals(other.MaxWantedLevel)
                && MaxChaos.Equals(other.MaxChaos)
                && ModelName.Equals(other.ModelName);
        }

        public PlayerPed DeepClone()
        {
            return new PlayerPed(this);
        }
    }
}