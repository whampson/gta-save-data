using System;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// the data block in GTA3 saves that contains the player peds.
    /// </summary>
    public class PedPool : SaveDataObject,
        IEquatable<PedPool>, IDeepClonable<PedPool>
    {
        private ObservableArray<PlayerPed> m_playerPeds;

        /// <summary>
        /// The list of saved player peds.
        /// </summary>
        /// <remarks>
        /// There should always be at least one player ped otherwise the game
        /// will crash.
        /// </remarks>
        public ObservableArray<PlayerPed> PlayerPeds
        {
            get { return m_playerPeds; }
            set { m_playerPeds = value; OnPropertyChanged(); }
        }

        public PedPool()
        {
            PlayerPeds = new ObservableArray<PlayerPed>()
            {
                new PlayerPed()
            };
        }

        public PedPool(PedPool other)
        {
            PlayerPeds = ArrayHelper.DeepClone(other.PlayerPeds);
        }

        /// <summary>
        /// Creates a new <see cref="PlayerPed"/>.
        /// </summary>
        /// <param name="modelName">
        /// The player model name. Only 'Special' models can be used.
        /// See: <see href="https://gtamods.com/wiki/023C#List_of_valid_models"/>.
        /// </param>
        public PlayerPed CreatePlayer(string modelName)
        {
            return new PlayerPed()
            {
                Type = PedTypeId.Player1,
                CharCreatedBy = CharCreatedBy.Mission,
                ModelName = modelName,
                Health = 100,
                Armor = 0,
                MaxStamina = 150,
            };
        }

        /// <summary>
        /// Adds a <see cref="PlayerPed"/> to the ped pool.
        /// </summary>
        /// <remarks>
        /// New player peds are automatically assigned a unique handle.
        /// </remarks>
        /// <param name="p">The <see cref="PlayerPed"/> to add.</param>
        /// <returns>The handle of the new <see cref="PlayerPed"/>.</returns>
        public int AddPlayer(PlayerPed p)
        {
            p.Handle = GetNextFreeHandle();
            PlayerPeds.Add(p);

            return p.Handle;
        }

        /// <summary>
        /// Gets the <see cref="PlayerPed"/> in focus.
        /// </summary>
        public PlayerPed GetPlayerInFocus()
        {
            return PlayerPeds.LastOrDefault();
        }

        /// <summary>
        /// Sets the <see cref="PlayerPed"/> in control.
        /// </summary>
        /// <remarks>
        /// If you add multiple player peds to the pool, the most recent one
        /// added will be in focus. Change which player ped is in focus by
        /// using this function.
        /// </remarks>
        public void SetPlayerInFocus(PlayerPed p)
        {
            int idx = PlayerPeds.IndexOf(p);
            int end = PlayerPeds.Count - 1;
            if (idx > -1)
            {
                PlayerPeds.Move(idx, end);
            }
        }

        /// <summary>
        /// Gets the next free handle in the ped pool.
        /// </summary>
        /// <remarks>
        /// If multiple player peds share a handle, weird things may happen.
        /// Use this function to give each player ped a unique handle.
        /// </remarks>
        public int GetNextFreeHandle()
        {
            int idx = 1;
            foreach (var p in PlayerPeds)
            {
                int pedIndex = p.Handle >> 8;
                if (pedIndex != idx)
                {
                    return idx;
                }
                idx++;
            }

            return (idx << 8) | 1;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            int numPeds = buf.ReadInt32();

            PlayerPeds.Clear();

            for (int i = 0; i < numPeds; i++)
            {
                PedTypeId type = (PedTypeId) buf.ReadInt32();
                short model = buf.ReadInt16();
                int handle = buf.ReadInt32();
                PlayerPed ped = new PlayerPed()
                {
                    ModelIndex = model,
                    Handle = handle,
                    Type = type
                };
                Serializer.Read(ped, buf, prm);
                ped.MaxWantedLevel = buf.ReadInt32();
                ped.MaxChaos = buf.ReadInt32();
                ped.ModelName = buf.ReadString(PlayerPed.ModelNameLength);
                PlayerPeds.Add(ped);
            }

            Debug.Assert(buf.Offset == SizeOf(this, prm));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            buf.Write(PlayerPeds.Count);

            foreach (PlayerPed ped in PlayerPeds)
            {
                buf.Write((int) ped.Type);
                buf.Write(ped.ModelIndex);
                buf.Write(ped.Handle);
                buf.Write(ped, prm);
                buf.Write(ped.MaxWantedLevel);
                buf.Write(ped.MaxChaos);
                buf.Write(ped.ModelName, PlayerPed.ModelNameLength);
            }

            Debug.Assert(buf.Offset == SizeOf(this, prm));
        }

        protected override int GetSize(SerializationParams prm)
        {
            int headerSize = 2 * sizeof(int) + sizeof(short);
            int footerSize = 2 * sizeof(int) + PlayerPed.ModelNameLength;

            int size = 0;
            foreach (PlayerPed ped in PlayerPeds)
            {
                size += headerSize;
                size += SizeOf(ped, prm);
                size += footerSize;
            }

            return sizeof(int) + size;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PedPool);
        }

        public bool Equals(PedPool other)
        {
            if (other == null)
            {
                return false;
            }

            return PlayerPeds.SequenceEqual(other.PlayerPeds);
        }

        public PedPool DeepClone()
        {
            return new PedPool(this);
        }
    }
}