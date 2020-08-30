using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Diagnostics;

namespace GTASaveData.LCS
{
    public class Garage : SaveDataObject, IGarage,
        IEquatable<Garage>, IDeepClonable<Garage>
    {
        // This class is still largely unconfirmed,
        // taken mostly from GTA3/VC.
        // Some things are overridden by MAIN.SCM

        private GarageType m_type;
        private GarageState m_state;
        private byte m_maxCarsAllowed;
        private bool m_closingWithoutTargetCar;
        private bool m_deactivated;
        private bool m_resprayHappened;
        //private int m_targetModelIndex;
        private uint m_pDoor1;      // zeroed on load
        private uint m_pDoor2;      // zeroed on load
        private int m_door1Handle;
        private int m_door2Handle;
        private bool m_isDoor1Dummy;
        private bool m_isDoor2Dummy;
        private bool m_recreateDoorOnNextRefresh;   // set to true on load
        private bool m_rotatedDoor;
        private bool m_cameraFollowsPlayer;
        private Vector3D m_position;
        private Quaternion m_rotation;
        private float m_ceilingZ;
        private float m_doorRelated1;
        private float m_doorRelated2;
        private float m_x1;
        private float m_x2;
        private float m_y1;
        private float m_y2;
        //private float m_z1;
        //private float m_z2; // replaced by ceilingZ?
        private float m_doorOpenOffset;
        private float m_doorOpenMax;
        private float m_door1X;
        private float m_door1Y;
        private float m_door2X;
        private float m_door2Y;
        private float m_door1Z;
        private float m_door2Z;
        private uint m_timer;
        //private byte m_collectedCarsState;
        //private uint m_pTargetCar;  // zeroed on load
        //private int m_field96h; // not used by game
        //private StoredCar m_storedCar;

        public GarageType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public GarageState State
        {
            get { return m_state; }
            set { m_state = value; OnPropertyChanged(); }
        }

        public byte MaxCarsAllowed
        {
            get { return m_maxCarsAllowed; }
            set { m_maxCarsAllowed = value; OnPropertyChanged(); }
        }

        public bool ClosingWithoutTargetVehicle
        {
            get { return m_closingWithoutTargetCar; }
            set { m_closingWithoutTargetCar = value; OnPropertyChanged(); }
        }

        public bool Deactivated
        {
            get { return m_deactivated; }
            set { m_deactivated = value; OnPropertyChanged(); }
        }

        public bool ResprayHappened
        {
            get { return m_resprayHappened; }
            set { m_resprayHappened = value; OnPropertyChanged(); }
        }

        public uint Door1Pointer
        {
            get { return m_pDoor1; }
            set { m_pDoor1 = value; OnPropertyChanged(); }
        }

        public uint Door2Pointer
        {
            get { return m_pDoor2; }
            set { m_pDoor2 = value; OnPropertyChanged(); }
        }

        public int Door1Handle
        {
            get { return m_door1Handle; }
            set { m_door1Handle = value; OnPropertyChanged(); }
        }

        public int Door2Handle
        {
            get { return m_door2Handle; }
            set { m_door2Handle = value; OnPropertyChanged(); }
        }

        public bool IsDoor1Dummy
        {
            get { return m_isDoor1Dummy; }
            set { m_isDoor1Dummy = value; OnPropertyChanged(); }
        }

        public bool IsDoor2Dummy
        {
            get { return m_isDoor2Dummy; }
            set { m_isDoor2Dummy = value; OnPropertyChanged(); }
        }

        public bool RecreateDoorOnNextRefresh
        {
            get { return m_recreateDoorOnNextRefresh; }
            set { m_recreateDoorOnNextRefresh = value; OnPropertyChanged(); }
        }

        public bool RotatingDoor
        {
            get { return m_rotatedDoor; }
            set { m_rotatedDoor = value; OnPropertyChanged(); }
        }

        public bool CameraFollowsPlayer
        {
            get { return m_cameraFollowsPlayer; }
            set { m_cameraFollowsPlayer = value; OnPropertyChanged(); }
        }

        public Vector3D Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public Quaternion Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; OnPropertyChanged(); }
        }

        public float CeilingZ
        {
            get { return m_ceilingZ; }
            set { m_ceilingZ = value; OnPropertyChanged(); }
        }

        public float DoorRelated1
        {
            get { return m_doorRelated1; }
            set { m_doorRelated1 = value; OnPropertyChanged(); }
        }

        public float DoorRelated2
        {
            get { return m_doorRelated2; }
            set { m_doorRelated2 = value; OnPropertyChanged(); }
        }

        public float X1
        {
            get { return m_x1; }
            set { m_x1 = value; OnPropertyChanged(); }
        }

        public float X2
        {
            get { return m_x2; }
            set { m_x2 = value; OnPropertyChanged(); }
        }

        public float Y1
        {
            get { return m_y1; }
            set { m_y1 = value; OnPropertyChanged(); }
        }

        public float Y2
        {
            get { return m_y2; }
            set { m_y2 = value; OnPropertyChanged(); }
        }

        public float DoorOpenOffset
        {
            get { return m_doorOpenOffset; }
            set { m_doorOpenOffset = value; OnPropertyChanged(); }
        }

        public float DoorOpenMax
        {
            get { return m_doorOpenMax; }
            set { m_doorOpenMax = value; OnPropertyChanged(); }
        }

        public float Door1X
        {
            get { return m_door1X; }
            set { m_door1X = value; OnPropertyChanged(); }
        }

        public float Door1Y
        {
            get { return m_door1Y; }
            set { m_door1Y = value; OnPropertyChanged(); }
        }

        public float Door2X
        {
            get { return m_door2X; }
            set { m_door2X = value; OnPropertyChanged(); }
        }

        public float Door2Y
        {
            get { return m_door2Y; }
            set { m_door2Y = value; OnPropertyChanged(); }
        }

        public float Door1Z
        {
            get { return m_door1Z; }
            set { m_door1Z = value; OnPropertyChanged(); }
        }

        public float Door2Z
        {
            get { return m_door2Z; }
            set { m_door2Z = value; OnPropertyChanged(); }
        }

        public uint Timer
        {
            get { return m_timer; }
            set { m_timer = value; OnPropertyChanged(); }
        }

        //public byte CollectedCarsState
        //{
        //    get { return m_collectedCarsState; }
        //    set { m_collectedCarsState = value; OnPropertyChanged(); }
        //}

        int IGarage.Type
        {
            get { return (int) Type; }
            set { Type = (GarageType) value; OnPropertyChanged(); }
        }

        int IGarage.State
        {
            get { return (int) State; }
            set { State = (GarageState) value; OnPropertyChanged(); }
        }

        public Vector3D PositionMin
        {
            get { return new Vector3D(X1, Y1, Position.Z); }    // TODO: confirm Z
            set { X1 = value.X; Y1 = value.Y; OnPropertyChanged(); }
        }

        public Vector3D PositionMax
        {
            get { return new Vector3D(X2, Y2, CeilingZ); }      // TODO: confirm Z
            set { X2 = value.X; Y2 = value.Y; OnPropertyChanged(); }
        }

        public Vector3D Door1Position
        {
            get { return new Vector3D(Door1X, Door1Y, Door1Z); }
            set { Door1X = value.X; Door1Y = value.Y; Door1Z = value.Z; OnPropertyChanged(); }
        }

        public Vector3D Door2Position
        {
            get { return new Vector3D(Door2X, Door2Y, Door2Z); }
            set { Door2X = value.X; Door2Y = value.Y; Door2Z = value.Z; OnPropertyChanged(); }
        }

        public Garage()
        {
            Position = new Vector3D();
            Rotation = new Quaternion();
        }

        public Garage(Garage other)
        {
            Type = other.Type;
            State = other.State;
            MaxCarsAllowed = other.MaxCarsAllowed;
            ClosingWithoutTargetVehicle = other.ClosingWithoutTargetVehicle;
            Deactivated = other.Deactivated;
            ResprayHappened = other.ResprayHappened;
            Door1Pointer = other.Door1Pointer;
            Door2Pointer = other.Door2Pointer;
            Door1Handle = other.Door1Handle;
            Door2Handle = other.Door2Handle;
            IsDoor1Dummy = other.IsDoor1Dummy;
            IsDoor2Dummy = other.IsDoor2Dummy;
            RecreateDoorOnNextRefresh = other.RecreateDoorOnNextRefresh;
            RotatingDoor = other.RotatingDoor;
            CameraFollowsPlayer = other.CameraFollowsPlayer;
            Position = other.Position;
            Rotation = other.Rotation;
            CeilingZ = other.CeilingZ;
            DoorRelated1 = other.DoorRelated1;
            DoorRelated2 = other.DoorRelated2;
            X1 = other.X1;
            X2 = other.X2;
            Y1 = other.Y1;
            Y2 = other.Y2;
            DoorOpenOffset = other.DoorOpenOffset;
            DoorOpenMax = other.DoorOpenMax;
            Door1X = other.Door1X;
            Door1Y = other.Door1Y;
            Door2X = other.Door2X;
            Door2Y = other.Door2Y;
            Door1Z = other.Door1Z;
            Door2Z = other.Door2Z;
            Timer = other.Timer;
            //CollectedCarsState = other.CollectedCarsState;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            if (fmt.IsPS2)
            {
                Type = (GarageType) buf.ReadByte();
                State = (GarageState) buf.ReadByte();
                MaxCarsAllowed = buf.ReadByte();
                buf.ReadByte();     // align?
                ClosingWithoutTargetVehicle = buf.ReadBool(4);
                Deactivated = buf.ReadBool(4);
                ResprayHappened = buf.ReadBool(4);
                Door1Handle = buf.ReadInt32();
                Door2Handle = buf.ReadInt32();
                Door1Pointer = buf.ReadUInt32();
                Door2Pointer = buf.ReadUInt32();
                buf.ReadInt32();    // unknown, nonzero
                IsDoor1Dummy = buf.ReadBool(4);
                IsDoor2Dummy = buf.ReadBool(4);
                RecreateDoorOnNextRefresh = buf.ReadBool(4);
                RotatingDoor = buf.ReadBool(4);
                CameraFollowsPlayer = buf.ReadBool(4);
                Position = buf.Read<Vector3D>();
                Rotation = buf.Read<Quaternion>();
                CeilingZ = buf.ReadFloat();
                buf.Skip(7 * sizeof(int));  // appears unused
                DoorRelated1 = buf.ReadFloat();     // not sure what this does
                DoorRelated2 = buf.ReadFloat();     // not sure what this does
                X1 = buf.ReadFloat();
                X2 = buf.ReadFloat();
                Y1 = buf.ReadFloat();
                Y2 = buf.ReadFloat();
                buf.ReadFloat();    // maybe unused z
                buf.ReadFloat();    // maybe unused z
                DoorOpenOffset = buf.ReadFloat();
                DoorOpenMax = buf.ReadFloat();
                Door1X = buf.ReadFloat();
                Door1Y = buf.ReadFloat();
                Door2X = buf.ReadFloat();
                Door2Y = buf.ReadFloat();
                Door1Z = buf.ReadFloat();
                Door2Z = buf.ReadFloat();
                Timer = buf.ReadUInt32();
                //CollectedCarsState = buf.ReadByte();  // no idea where this fits in
                buf.Skip(18 * sizeof(int));  // appears unused
            }
            else if (fmt.IsPSP)
            {
                Type = (GarageType) buf.ReadByte();
                State = (GarageState) buf.ReadByte();
                MaxCarsAllowed = buf.ReadByte();
                buf.ReadByte();     // align?
                ClosingWithoutTargetVehicle = buf.ReadBool();
                Deactivated = buf.ReadBool();
                ResprayHappened = buf.ReadBool();
                buf.ReadByte();     // align?
                buf.ReadInt32();    // unknown
                Door1Pointer = buf.ReadUInt32();
                Door2Pointer = buf.ReadUInt32();
                Door1Handle = buf.ReadByte();
                Door2Handle = buf.ReadByte();
                IsDoor1Dummy = buf.ReadBool();
                IsDoor2Dummy = buf.ReadBool();
                RecreateDoorOnNextRefresh = buf.ReadBool();
                RotatingDoor = buf.ReadBool();
                CameraFollowsPlayer = buf.ReadBool();
                buf.ReadByte();     // collectedCarsState
                Position = buf.Read<Vector3D>();
                Rotation = buf.Read<Quaternion>();
                CeilingZ = buf.ReadFloat();
                buf.Skip(6 * sizeof(int));  // appears unused
                DoorRelated1 = buf.ReadFloat();     // not sure what this does
                DoorRelated2 = buf.ReadFloat();     // not sure what this does
                X1 = buf.ReadFloat();
                X2 = buf.ReadFloat();
                Y1 = buf.ReadFloat();
                Y2 = buf.ReadFloat();
                buf.ReadFloat();    // maybe unused z
                buf.ReadFloat();    // maybe unused z
                DoorOpenOffset = buf.ReadFloat();
                DoorOpenMax = buf.ReadFloat();
                Door1X = buf.ReadFloat();
                Door1Y = buf.ReadFloat();
                Door2X = buf.ReadFloat();
                Door2Y = buf.ReadFloat();
                Door1Z = buf.ReadFloat();
                Door2Z = buf.ReadFloat();
                Timer = buf.ReadUInt32();
                buf.Skip(18 * sizeof(int));  // appears unused
            }
            else if (fmt.IsMobile)
            {
                Door1Pointer = buf.ReadUInt32();
                buf.ReadInt32();        // appears unused
                Door2Pointer = buf.ReadUInt32();
                buf.ReadInt32();        // appears unused
                buf.Skip(21 * sizeof(int));  // appears unused
                Position = buf.Read<Vector3D>();
                Rotation = buf.Read<Quaternion>();
                CeilingZ = buf.ReadFloat();
                buf.ReadInt32();  // appears unused
                DoorRelated1 = buf.ReadFloat();     // not sure what this does
                DoorRelated2 = buf.ReadFloat();     // not sure what this does
                X1 = buf.ReadFloat();
                X2 = buf.ReadFloat();
                Y1 = buf.ReadFloat();
                Y2 = buf.ReadFloat();
                buf.ReadInt32();    // appears unused
                DoorOpenOffset = buf.ReadFloat();
                DoorOpenMax = buf.ReadFloat();
                Door1X = buf.ReadFloat();
                Door1Y = buf.ReadFloat();
                Door2X = buf.ReadFloat();
                Door2Y = buf.ReadFloat();
                Door1Z = buf.ReadFloat();
                Door2Z = buf.ReadFloat();
                Timer = buf.ReadUInt32();
                Type = (GarageType) buf.ReadByte();
                State = (GarageState) buf.ReadByte();
                MaxCarsAllowed = buf.ReadByte();
                buf.ReadByte();     // align?
                ClosingWithoutTargetVehicle = buf.ReadBool();
                Deactivated = buf.ReadBool();
                ResprayHappened = buf.ReadBool();
                Door1Handle = buf.ReadByte();
                Door2Handle = buf.ReadByte();
                IsDoor1Dummy = buf.ReadBool();
                IsDoor2Dummy = buf.ReadBool();
                RecreateDoorOnNextRefresh = buf.ReadBool();
                RotatingDoor = buf.ReadBool();
                CameraFollowsPlayer = buf.ReadBool();
                buf.ReadByte();     // collectedCarsState
                buf.Skip(9);
            }

            Debug.Assert(buf.Offset == SizeOfType<Garage>(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            if (fmt.IsPS2)
            {
                buf.Write((byte) Type);
                buf.Write((byte) State);
                buf.Write(MaxCarsAllowed);
                buf.Skip(1);
                buf.Write(ClosingWithoutTargetVehicle, 4);
                buf.Write(Deactivated, 4);
                buf.Write(ResprayHappened, 4);
                buf.Write(Door1Handle);
                buf.Write(Door2Handle);
                buf.Write(Door1Pointer);
                buf.Write(Door2Pointer);
                buf.Write(0);       // original value nonzero
                buf.Write(IsDoor1Dummy, 4);
                buf.Write(IsDoor2Dummy, 4);
                buf.Write(RecreateDoorOnNextRefresh, 4);
                buf.Write(RotatingDoor, 4);
                buf.Write(CameraFollowsPlayer, 4);
                buf.Write(Position);
                buf.Write(Rotation);
                buf.Write(CeilingZ);
                buf.Write(0);
                buf.Write(0);
                buf.Write(0);
                buf.Write(0);
                buf.Write(0);
                buf.Write(0);
                buf.Write(0);
                buf.Write(DoorRelated1);
                buf.Write(DoorRelated2);
                buf.Write(X1);
                buf.Write(X2);
                buf.Write(Y1);
                buf.Write(Y2);
                buf.Write(0);
                buf.Write(0);
                buf.Write(DoorOpenOffset);
                buf.Write(DoorOpenMax);
                buf.Write(Door1X);
                buf.Write(Door1Y);
                buf.Write(Door2X);
                buf.Write(Door2Y);
                buf.Write(Door1Z);
                buf.Write(Door2Z);
                buf.Write(Timer);
                for (int i = 0; i < 18; i++) buf.Write(0);
            }
            else if (fmt.IsPSP)
            {
                buf.Write((byte) Type);
                buf.Write((byte) State);
                buf.Write(MaxCarsAllowed);
                buf.Skip(1);
                buf.Write(ClosingWithoutTargetVehicle);
                buf.Write(Deactivated);
                buf.Write(ResprayHappened);
                buf.Skip(1);
                buf.Write(0);
                buf.Write(Door1Pointer);
                buf.Write(Door2Pointer);
                buf.Write((byte) Door1Handle);
                buf.Write((byte) Door2Handle);
                buf.Write(IsDoor1Dummy);
                buf.Write(IsDoor2Dummy);
                buf.Write(RecreateDoorOnNextRefresh);
                buf.Write(RotatingDoor);
                buf.Write(CameraFollowsPlayer);
                buf.Write((byte) 0);
                buf.Write(Position);
                buf.Write(Rotation);
                buf.Write(CeilingZ);
                buf.Write(0);
                buf.Write(0);
                buf.Write(0);
                buf.Write(0);
                buf.Write(0);
                buf.Write(0);
                buf.Write(DoorRelated1);
                buf.Write(DoorRelated2);
                buf.Write(X1);
                buf.Write(X2);
                buf.Write(Y1);
                buf.Write(Y2);
                buf.Write(0);
                buf.Write(0);
                buf.Write(DoorOpenOffset);
                buf.Write(DoorOpenMax);
                buf.Write(Door1X);
                buf.Write(Door1Y);
                buf.Write(Door2X);
                buf.Write(Door2Y);
                buf.Write(Door1Z);
                buf.Write(Door2Z);
                buf.Write(Timer);
                for (int i = 0; i < 18; i++) buf.Write(0);
            }
            else if (fmt.IsMobile)
            {
                buf.Write(Door1Pointer);
                buf.Write(0);
                buf.Write(Door2Pointer);
                buf.Write(0);
                for (int i = 0; i < 21; i++) buf.Write(0);
                buf.Write(Position);
                buf.Write(Rotation);
                buf.Write(CeilingZ);
                buf.Write(0);
                buf.Write(DoorRelated1);
                buf.Write(DoorRelated2);
                buf.Write(X1);
                buf.Write(X2);
                buf.Write(Y1);
                buf.Write(Y2);
                buf.Write(0);
                buf.Write(DoorOpenOffset);
                buf.Write(DoorOpenMax);
                buf.Write(Door1X);
                buf.Write(Door1Y);
                buf.Write(Door2X);
                buf.Write(Door2Y);
                buf.Write(Door1Z);
                buf.Write(Door2Z);
                buf.Write(Timer);
                buf.Write((byte) Type);
                buf.Write((byte) State);
                buf.Write(MaxCarsAllowed);
                buf.Skip(1);
                buf.Write(ClosingWithoutTargetVehicle);
                buf.Write(Deactivated);
                buf.Write(ResprayHappened);
                buf.Write((byte) Door1Handle);
                buf.Write((byte) Door2Handle);
                buf.Write(IsDoor1Dummy);
                buf.Write(IsDoor2Dummy);
                buf.Write(RecreateDoorOnNextRefresh);
                buf.Write(RotatingDoor);
                buf.Write(CameraFollowsPlayer);
                buf.Write((byte) 0);
                buf.Skip(9);
            }

            Debug.Assert(buf.Offset == SizeOfType<Garage>(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsPS2) return 0x100;
            if (fmt.IsPSP) return 0xE0;
            if (fmt.IsMobile) return 0xE0;
            throw SizeNotDefined(fmt);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Garage);
        }

        public bool Equals(Garage other)
        {
            if (other == null)
            {
                return false;
            }

            return Type.Equals(other.Type)
                && State.Equals(other.State)
                && MaxCarsAllowed.Equals(other.MaxCarsAllowed)
                && ClosingWithoutTargetVehicle.Equals(other.ClosingWithoutTargetVehicle)
                && Deactivated.Equals(other.Deactivated)
                && ResprayHappened.Equals(other.ResprayHappened)
                && Door1Pointer.Equals(other.Door1Pointer)
                && Door2Pointer.Equals(other.Door2Pointer)
                && Door1Handle.Equals(other.Door1Handle)
                && Door2Handle.Equals(other.Door2Handle)
                && IsDoor1Dummy.Equals(other.IsDoor1Dummy)
                && IsDoor2Dummy.Equals(other.IsDoor2Dummy)
                && RecreateDoorOnNextRefresh.Equals(other.RecreateDoorOnNextRefresh)
                && RotatingDoor.Equals(other.RotatingDoor)
                && CameraFollowsPlayer.Equals(other.CameraFollowsPlayer)
                && Position.Equals(other.Position)
                && Rotation.Equals(other.Rotation)
                && CeilingZ.Equals(other.CeilingZ)
                && DoorRelated1.Equals(other.DoorRelated1)
                && DoorRelated2.Equals(other.DoorRelated2)
                && X1.Equals(other.X1)
                && X2.Equals(other.X2)
                && Y1.Equals(other.Y1)
                && Y2.Equals(other.Y2)
                && DoorOpenOffset.Equals(other.DoorOpenOffset)
                && DoorOpenMax.Equals(other.DoorOpenMax)
                && Door1X.Equals(other.Door1X)
                && Door1Y.Equals(other.Door1Y)
                && Door2X.Equals(other.Door2X)
                && Door2Y.Equals(other.Door2Y)
                && Door1Z.Equals(other.Door1Z)
                && Door2Z.Equals(other.Door2Z)
                && Timer.Equals(other.Timer);
        }

        public Garage DeepClone()
        {
            return new Garage(this);
        }
    }

    public enum GarageState
    {
        Closed,
        Opened,
        Closing,
        Opening,
        OpenedContainsCar,
        ClosedAfterDropOff
    }

    public enum GarageType
    {
        None,
        Mission,
        BombShop1,
        BombShop2,
        BombShop3,
        Respray,
        CollectorsItems,
        CollectSpecificCars,
        CollectCars1,
        CollectCars2,
        CollectCars3,
        ForCarToComeOutOf,
        SixtySeconds,
        Crusher,
        MissionKeepCar,
        ForScriptToOpen,
        Hideout1,
        Hideout2,
        Hideout3,
        ForScriptToOpenAndClose,
        KeepsOpeningForSpecificCar,
        MissionKeepCarRemainClosed,
        CollectCars4,
        ForScriptToOpenForCar,
    }
}
