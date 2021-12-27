using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class RadarBlip : SaveDataObject,
        IEquatable<RadarBlip>, IDeepClonable<RadarBlip>
    {
        private int m_colorId;
        private RadarBlipType m_type;
        private int m_handle;
        private Vector2 m_radarPosition;
        private Vector3 m_worldPosition;
        private short m_blipIndex;
        private bool m_isBright;
        private bool m_inUse;
        private float m_radius;
        private short m_scale;
        private RadarBlipDisplay m_display;
        private RadarBlipSprite m_sprite;

        public int Color
        {
            get { return m_colorId; }
            set { m_colorId = value; OnPropertyChanged(); }
        }

        public RadarBlipType Type
        {
            get { return m_type; }
            set
            {
                m_type = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public int EntityHandle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        public Vector2 RadarPosition
        {
            get { return m_radarPosition; }
            set { m_radarPosition = value; OnPropertyChanged(); }
        }

        public Vector3 MarkerPosition
        {
            get { return m_worldPosition; }
            set { m_worldPosition = value; OnPropertyChanged(); }
        }

        public short Index
        {
            get { return m_blipIndex; }
            set { m_blipIndex = value; OnPropertyChanged(); }
        }

        public bool IsBright
        {
            get { return m_isBright; }
            set { m_isBright = value; OnPropertyChanged(); }
        }

        public bool IsInUse
        {
            get { return m_inUse; }
            set
            {
                m_inUse = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public float DebugSphereRadius
        {
            get { return m_radius; }
            set { m_radius = value; OnPropertyChanged(); }
        }

        public short Scale
        {
            get { return m_scale; }
            set
            {
                m_scale = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public RadarBlipDisplay Display
        {
            get { return m_display; }
            set
            {
                m_display = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public RadarBlipSprite Sprite
        {
            get { return m_sprite; }
            set
            {
                m_sprite = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSprite));
                OnPropertyChanged(nameof(Color));
            }
        }

        public bool IsValid => Type != RadarBlipType.None;

        public bool IsVisible => 
            IsValid && 
            IsInUse && 
            Display != RadarBlipDisplay.None &&
            ((!HasSprite && Scale > 0) || HasSprite);

        public bool HasSprite => Sprite != RadarBlipSprite.None;

        public RadarBlip()
        {
            Index = 1;
            RadarPosition = new Vector2();
            MarkerPosition = new Vector3();
        }

        public RadarBlip(RadarBlip other)
        {
            Color = other.Color;
            Type = other.Type;
            EntityHandle = other.EntityHandle;
            RadarPosition = other.RadarPosition;
            MarkerPosition = other.MarkerPosition;
            Index = other.Index;
            IsBright = other.IsBright;
            IsInUse = other.IsInUse;
            DebugSphereRadius = other.DebugSphereRadius;
            Scale = other.Scale;
            Display = other.Display;
            Sprite = other.Sprite;
        }

        public void Invalidate()
        {
            Index = 1;
            Display = RadarBlipDisplay.None;
            Type = RadarBlipType.None;
            IsInUse = false;
            Scale = 0;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            Color = buf.ReadInt32();
            Type = (RadarBlipType) buf.ReadInt32();
            EntityHandle = buf.ReadInt32();
            RadarPosition = buf.ReadStruct<Vector2>();
            MarkerPosition = buf.ReadStruct<Vector3>();
            Index = buf.ReadInt16();
            IsBright = buf.ReadBool();
            IsInUse = buf.ReadBool();
            DebugSphereRadius = buf.ReadFloat();
            Scale = buf.ReadInt16();
            Display = (RadarBlipDisplay) buf.ReadInt16();
            Sprite = (RadarBlipSprite) buf.ReadInt16();
            buf.ReadInt16();

            Debug.Assert(buf.Offset == SizeOf<RadarBlip>());
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            buf.Write(Color);
            buf.Write((int) Type);
            buf.Write(EntityHandle);
            buf.Write(RadarPosition);
            buf.Write(MarkerPosition);
            buf.Write(Index);
            buf.Write(IsBright);
            buf.Write(IsInUse);
            buf.Write(DebugSphereRadius);
            buf.Write(Scale);
            buf.Write((short) Display);
            buf.Write((short) Sprite);
            buf.Write((short) 0);

            Debug.Assert(buf.Offset == SizeOf<RadarBlip>());
        }

        protected override int GetSize(SerializationParams prm)
        {
            return 0x30;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RadarBlip);
        }

        public bool Equals(RadarBlip other)
        {
            if (other == null)
            {
                return false;
            }

            return Color.Equals(other.Color)
                && Type.Equals(other.Type)
                && EntityHandle.Equals(other.EntityHandle)
                && RadarPosition.Equals(other.RadarPosition)
                && MarkerPosition.Equals(other.MarkerPosition)
                && Index.Equals(other.Index)
                && IsBright.Equals(other.IsBright)
                && IsInUse.Equals(other.IsInUse)
                && DebugSphereRadius.Equals(other.DebugSphereRadius)
                && Scale.Equals(other.Scale)
                && Display.Equals(other.Display)
                && Sprite.Equals(other.Sprite);
        }

        public RadarBlip DeepClone()
        {
            return new RadarBlip(this);
        }
    }

    [Flags]
    public enum RadarBlipDisplay
    {
        [Description("None")]
        None,

        [Description("Marker")]
        Marker,

        [Description("Blip")]
        Blip,

        [Description("Marker & Blip")]
        MarkerAndBlip
    }

    public enum RadarBlipType
    {
        [Description("None")]
        None,

        [Description("Car")]
        Car,

        [Description("Charcter")]
        Char,

        [Description("Object")]
        Object,

        [Description("Coordinate")]
        Coord,

        [Description("Contact Point")]
        ContactPoint
    }

    public enum RadarBlipSprite
    {
        None,
        Asuka,
        Bomb,
        Cat,
        Centre,
        Copcar,
        Don,
        Eight,
        El,
        Ice,
        Joey,
        Kenji,
        Liz,
        Luigi,
        North,
        Ray,
        Sal,
        Save,
        Spray,
        Tony,
        Weapon
    }
}
