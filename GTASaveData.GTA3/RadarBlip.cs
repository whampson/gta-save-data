using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x30)]
    public class RadarBlip : SaveDataObject, IEquatable<RadarBlip>
    {
        private int m_colorId;
        private BlipType m_type;
        private int m_handle;
        private Vector2D m_radarPosition;
        private Vector m_worldPosition;
        private short m_blipIndex;
        private bool m_dim;
        private bool m_inUse;
        private float m_radius;
        private short m_scale;
        private BlipDisplay m_display;
        private BlipSprite m_sprite;

        public int ColorId
        {
            get { return m_colorId; }
            set { m_colorId = value; OnPropertyChanged(); }
        }

        public BlipType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public int Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        public Vector2D RadarPosition
        {
            get { return m_radarPosition; }
            set { m_radarPosition = value; OnPropertyChanged(); }
        }

        public Vector WorldPosition
        {
            get { return m_worldPosition; }
            set { m_worldPosition = value; OnPropertyChanged(); }
        }

        public short BlipIndex
        {
            get { return m_blipIndex; }
            set { m_blipIndex = value; OnPropertyChanged(); }
        }

        public bool Dim
        {
            get { return m_dim; }
            set { m_dim = value; OnPropertyChanged(); }
        }

        public bool InUse
        {
            get { return m_inUse; }
            set { m_inUse = value; OnPropertyChanged(); }
        }

        public float Radius
        {
            get { return m_radius; }
            set { m_radius = value; OnPropertyChanged(); }
        }

        public short Scale
        {
            get { return m_scale; }
            set { m_scale = value; OnPropertyChanged(); }
        }

        public BlipDisplay Display
        {
            get { return m_display; }
            set { m_display = value; OnPropertyChanged(); }
        }

        public BlipSprite Sprite
        {
            get { return m_sprite; }
            set { m_sprite = value; OnPropertyChanged(); }
        }

        public RadarBlip()
        {
            RadarPosition = new Vector2D();
            WorldPosition = new Vector();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            ColorId = buf.ReadInt32();
            Type = (BlipType) buf.ReadInt32();
            Handle = buf.ReadInt32();
            RadarPosition = buf.Read<Vector2D>();
            WorldPosition = buf.Read<Vector>();
            BlipIndex = buf.ReadInt16();
            Dim = buf.ReadBool();
            InUse = buf.ReadBool();
            Radius = buf.ReadFloat();
            Scale = buf.ReadInt16();
            Display = (BlipDisplay) buf.ReadInt16();
            Sprite = (BlipSprite) buf.ReadInt16();
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == SizeOf<RadarBlip>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(ColorId);
            buf.Write((int) Type);
            buf.Write(Handle);
            buf.Write(RadarPosition);
            buf.Write(WorldPosition);
            buf.Write(BlipIndex);
            buf.Write(Dim);
            buf.Write(InUse);
            buf.Write(Radius);
            buf.Write(Scale);
            buf.Write((short) Display);
            buf.Write((short) Sprite);
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == SizeOf<RadarBlip>());
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

            return ColorId.Equals(other.ColorId)
                && Type.Equals(other.Type)
                && Handle.Equals(other.Handle)
                && RadarPosition.Equals(other.RadarPosition)
                && WorldPosition.Equals(other.WorldPosition)
                && BlipIndex.Equals(other.BlipIndex)
                && Dim.Equals(other.Dim)
                && InUse.Equals(other.InUse)
                && Radius.Equals(other.Radius)
                && Scale.Equals(other.Scale)
                && Display.Equals(other.Display)
                && Sprite.Equals(other.Sprite);
        }
    }

    public enum BlipType
    {
        None,
        Car,
        Char,
        Object,
        Coord,
        ContactPoint
    }

    [Flags]
    public enum BlipDisplay
    {
        None,
        Marker,
        Blip,
        MarkerAndBlip
    }

    public enum BlipSprite
    {
        None,
        Asuka,
        Bomb,
        Cat,
        Center,
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
