using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class PedPool : SaveDataObject,
        IEquatable<PedPool>, IDeepClonable<PedPool>
    {
        private ObservableArray<PlayerPed> m_playerPeds;

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

        public PlayerPed GetPlayerPed()
        {
            return PlayerPeds.Count != 0
                ? PlayerPeds[0]
                : throw new InvalidOperationException(Strings.Error_InvalidOperation_NoPlayerPed);
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
                PlayerPed ped = new PlayerPed(model, handle) { Type = type };
                Serializer.Read(ped, buf, prm);
                ped.MaxWantedLevel = buf.ReadInt32();
                ped.MaxChaosLevel = buf.ReadInt32();
                ped.ModelName = buf.ReadString(PlayerPed.MaxModelNameLength);
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
                buf.Write(ped.MaxChaosLevel);
                buf.Write(ped.ModelName, PlayerPed.MaxModelNameLength);
            }

            Debug.Assert(buf.Offset == SizeOf(this, prm));
        }

        protected override int GetSize(SerializationParams prm)
        {
            int headerSize = 2 * sizeof(int) + sizeof(short);
            int footerSize = 2 * sizeof(int) + PlayerPed.MaxModelNameLength;

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