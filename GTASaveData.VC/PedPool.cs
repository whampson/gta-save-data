using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.VC
{
    public class PedPool : SaveDataObject,
        IEquatable<PedPool>, IDeepClonable<PedPool>,
        IEnumerable<PlayerPed>
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

        public int NumPlayerPeds
        {
            get { return PlayerPeds.Count; }
        }

        public PedPool()
        {
            PlayerPeds = new Array<PlayerPed>()
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
            if (PlayerPeds.Count == 0)
            {
                throw new InvalidOperationException("The ped pool must contain at least one PlayerPed.");
            }

            return PlayerPeds[0];
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            PlayerPeds.Clear();

            int numPeds = buf.ReadInt32();
            for (int i = 0; i < numPeds; i++)
            {
                PedTypeId type = (PedTypeId) buf.ReadInt32();
                short model = buf.ReadInt16();
                int handle = buf.ReadInt32();
                PlayerPed p = new PlayerPed(model, handle) { Type = type };
                Serializer.Read(p, buf, fmt);
                p.MaxWantedLevel = buf.ReadInt32();
                p.MaxChaosLevel = buf.ReadInt32();
                p.ModelName = buf.ReadString(PlayerPed.MaxModelNameLength);
                PlayerPeds.Add(p);
            }

            Debug.Assert(buf.Offset == SizeOfObject(this, fmt));
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
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
                buf.Write(p.ModelName, PlayerPed.MaxModelNameLength);
            }

            Debug.Assert(buf.Offset == SizeOfObject(this, fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            int headerSize = 2 * sizeof(int) + sizeof(short);
            int footerSize = 2 * sizeof(int) + PlayerPed.MaxModelNameLength;

            int size = 0;
            foreach (PlayerPed p in PlayerPeds)
            {
                size += headerSize;
                size += SizeOfObject(p, fmt);
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

        public IEnumerator<PlayerPed> GetEnumerator()
        {
            return PlayerPeds.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}