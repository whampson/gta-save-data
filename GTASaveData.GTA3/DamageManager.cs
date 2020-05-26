using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class DamageManager : SaveDataObject, IEquatable<DamageManager>
    {
        public static class Limits
        {
            public const int NumWheels = 4;
            public const int NumDoors = 6;
            public const int NumLights = 4;
            public const int NumPanels = 7;
        }

        private float m_wheelDamageEffect;
        private byte m_engine;
        private Array<WheelStatus> m_wheels;
        private Array<DoorStatus> m_doors;
        private Array<LightStatus> m_lights;
        private Array<PanelStatus> m_panels;
        private int m_field24h;

        public float WheelDamageEffect
        {
            get { return m_wheelDamageEffect; }
            set { m_wheelDamageEffect = value; OnPropertyChanged(); }
        }

        public byte Engine
        {
            get { return m_engine; }
            set { m_engine = value; OnPropertyChanged(); }
        }

        public Array<WheelStatus> Wheels
        {
            get { return m_wheels; }
            set { m_wheels = value; OnPropertyChanged(); }
        }

        public Array<DoorStatus> Doors
        {
            get { return m_doors; }
            set { m_doors = value; OnPropertyChanged(); }
        }

        public Array<LightStatus> Lights
        {
            get { return m_lights; }
            set { m_lights = value; OnPropertyChanged(); }
        }

        public Array<PanelStatus> Panels
        {
            get { return m_panels; }
            set { m_panels = value; OnPropertyChanged(); }
        }

        public int Field24h
        {
            get { return m_field24h; }
            set { m_field24h = value; OnPropertyChanged(); }
        }

        public DamageManager()
        {
            WheelDamageEffect = 0.75f;
            Wheels = ArrayHelper.CreateArray<WheelStatus>(Limits.NumWheels);
            Doors = ArrayHelper.CreateArray<DoorStatus>(Limits.NumDoors);
            Lights = ArrayHelper.CreateArray<LightStatus>(Limits.NumLights);
            Panels = ArrayHelper.CreateArray<PanelStatus>(Limits.NumPanels);
            Field24h = 1;
        }

        public DamageManager(DamageManager other)
        {
            WheelDamageEffect = other.WheelDamageEffect;
            Engine = other.Engine;
            Wheels = new Array<WheelStatus>(other.Wheels);
            Doors = new Array<DoorStatus>(other.Doors);
            Lights = new Array<LightStatus>(other.Lights);
            Panels = new Array<PanelStatus>(other.Panels);
            Field24h = other.Field24h;
        }

        public WheelStatus GetWheelStatus(Wheel wheel)
        {
            return Wheels[(int) wheel];
        }

        public void SetWheelStatus(Wheel wheel, WheelStatus status)
        {
            Wheels[(int) wheel] = status;
        }

        public DoorStatus GetDoorStatus(Door door)
        {
            return Doors[(int) door];
        }

        public void SetDoorStatus(Door door, DoorStatus status)
        {
            Doors[(int) door] = status;
        }

        public LightStatus GetLightStatus(Light light)
        {
            return Lights[(int) light];
        }

        public void SetLightStatus(Light light, LightStatus status)
        {
            Lights[(int) light] = status;
        }

        public PanelStatus GetPanelStatus(Panel panel)
        {
            return Panels[(int) panel];
        }

        public void SetPanelStatus(Panel panel, PanelStatus status)
        {
            Panels[(int) panel] = status;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            WheelDamageEffect = buf.ReadFloat();
            Engine = buf.ReadByte();
            Wheels = buf.Read<WheelStatus>(Limits.NumWheels);
            Doors = buf.Read<DoorStatus>(Limits.NumDoors);
            buf.Skip(1);
            int lightStatus = buf.ReadInt32();
            int panelStatus = buf.ReadInt32();
            Field24h = buf.ReadInt32();

            Lights.Clear();
            for (int i = 0; i < Limits.NumLights; i++)
            {
                Lights.Add((LightStatus) (lightStatus & 0x3));
                lightStatus >>= 2;
            }

            Panels.Clear();
            for (int i = 0; i < Limits.NumPanels; i++)
            {
                Panels.Add((PanelStatus) (panelStatus & 0xF));
                panelStatus >>= 4;
            }

            Debug.Assert(buf.Offset == SizeOf<DamageManager>());
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            int lightStatus = 0;
            int panelStatus = 0;

            for (int i = 0; i < Limits.NumLights; i++)
            {
                lightStatus |= (((int) Lights[i]) & 0x3) << (i * 2);
            }

            for (int i = 0; i < Limits.NumPanels; i++)
            {
                panelStatus |= (((int) Panels[i]) & 0xF) << (i * 4);
            }

            buf.Write(WheelDamageEffect);
            buf.Write(Engine);
            buf.Write(Wheels.ToArray(), Limits.NumWheels);
            buf.Write(Doors.ToArray(), Limits.NumDoors);
            buf.Skip(1);
            buf.Write(lightStatus);
            buf.Write(panelStatus);
            buf.Write(Field24h);

            Debug.Assert(buf.Offset == SizeOf<DamageManager>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x1C;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DamageManager);
        }

        public bool Equals(DamageManager other)
        {
            if (other == null)
            {
                return false;
            }

            return WheelDamageEffect.Equals(other.WheelDamageEffect)
                && Engine.Equals(other.Engine)
                && Wheels.SequenceEqual(other.Wheels)
                && Doors.SequenceEqual(other.Doors)
                && Lights.SequenceEqual(other.Lights)
                && Panels.SequenceEqual(other.Panels)
                && Field24h.Equals(other.Field24h);
        }
    }

    public enum EngineStatus : byte
    {
        Steam1 = 100,
        Steam2 = 150,
        Smoke = 200,
        OnFire = 225,
    }

    public enum Door
    {
        Hood,
        Trunk,
        FrontLeft,
        FrontRight,
        RearLeft,
        RearRight
    }

    public enum DoorStatus : byte
    {
        Ok,
        Smashed,
        Swinging,
        Missing
    }

    public enum Wheel
    {
        FrontLeft,
        FrontRight,
        RearLeft,
        RearRight
    }

    public enum WheelStatus : byte
    {
        Ok,
        Burst,
        Missing
    }

    public enum Light
    {
        FrontLeft,
        FrontRight,
        RearLeft,
        RearRight
    }

    public enum LightStatus
    {
        Ok,
        Broken
    }

    public enum Panel
    {
        FrontLeft,
        FrontRight,
        RearLeft,
        RearRight,
        Windscreen,
        BumperFront,
        BumperRear
    }

    public enum PanelStatus
    {
        Ok,
        Smashed1,
        Smashed2,
        Missing
    }
}
