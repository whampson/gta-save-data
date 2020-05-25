﻿using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class PedPool : SaveDataObject, IEquatable<PedPool>
    {
        // Yes, you can have multiple player peds
        // You get some weird behavior though...

        private Array<PlayerPed> m_playerPeds;

        public Array<PlayerPed> PlayerPeds
        {
            get { return m_playerPeds; }
            set { m_playerPeds = value; OnPropertyChanged(); }
        }

        public PlayerPed this[int i]
        {
            get { return PlayerPeds[i]; }
            set { PlayerPeds[i] = value; OnPropertyChanged(); }
        }

        public PedPool()
        {
            PlayerPeds = new Array<PlayerPed>();
        }

        public PlayerPed GetPlayerPed()
        {
            if (PlayerPeds.Count == 0)
            {
                throw new InvalidOperationException(Strings.Error_PlayerNotFound);
            }

            return PlayerPeds[0];
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int numPeds = buf.ReadInt32();

            PlayerPeds.Clear();

            for (int i = 0; i < numPeds; i++)
            {
                PedTypeId type = (PedTypeId) buf.ReadInt32();
                short model = buf.ReadInt16();
                int handle = buf.ReadInt32();
                PlayerPed p = new PlayerPed(model, handle);
                Serializer.Read(p, buf, fmt);
                p.MaxWantedLevel = buf.ReadInt32();
                p.MaxChaosLevel = buf.ReadInt32();
                p.ModelName = buf.ReadString(PlayerPed.Limits.MaxModelNameLength);
                PlayerPeds.Add(p);
            }

            Debug.Assert(buf.Offset == SizeOf(this, fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(PlayerPeds.Count);

            foreach (PlayerPed p in PlayerPeds)
            {
                buf.Write((int) p.Type);
                buf.Write(p.ModelIndex);
                buf.Write(p.Handle);
                buf.Write(p, fmt);
                buf.Write(p.MaxWantedLevel);
                buf.Write(p.MaxChaosLevel);
                buf.Write(p.ModelName, PlayerPed.Limits.MaxModelNameLength);
            }

            Debug.Assert(buf.Offset == SizeOf<PedPool>(this, fmt));
        }

        protected override int GetSize(DataFormat fmt)
        {
            int headerSize = 2 * sizeof(int) + sizeof(short);
            int footerSize = 2 * sizeof(int) + PlayerPed.Limits.MaxModelNameLength;

            int size = 0;
            foreach (PlayerPed p in PlayerPeds)
            {
                size += headerSize;
                size += SizeOf(p, fmt);
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
    }
}