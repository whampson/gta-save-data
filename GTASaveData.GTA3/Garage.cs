﻿using GTASaveData.Interfaces;
using System;
using System.Diagnostics;
using System.Numerics;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3
{
    public class Garage : SaveDataObject, IGarage,
        IEquatable<Garage>, IDeepClonable<Garage>
    {
        private GarageType m_type;
        private GarageState m_state;
        private bool m_field02h;    // not used by game
        private bool m_closingWithoutTargetCar;
        private bool m_deactivated;
        private bool m_resprayHappened;
        private int m_targetModelIndex;
        private uint m_pDoor1;      // zeroed on load
        private uint m_pDoor2;      // zeroed on load
        private byte m_door1Handle;
        private byte m_door2Handle;
        private bool m_isDoor1Dummy;
        private bool m_isDoor2Dummy;
        private bool m_recreateDoorOnNextRefresh;   // set to true on load
        private bool m_rotatedDoor;
        private bool m_cameraFollowsPlayer;
        private float m_x1;
        private float m_x2;
        private float m_y1;
        private float m_y2;
        private float m_z1;
        private float m_z2;
        private float m_doorOpenOffset;
        private float m_doorOpenMax;
        private float m_door1X;
        private float m_door1Y;
        private float m_door2X;
        private float m_door2Y;
        private float m_door1Z;
        private float m_door2Z;
        private uint m_timeToStartAction;
        private byte m_collectedCarsState;
        private uint m_pTargetCar;  // zeroed on load
        private int m_field96h; // not used by game
        private StoredCar m_storedCar;

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

        [Obsolete("Not used by the game.")]
        public bool Field02h
        {
            get { return m_field02h; }
            set { m_field02h = value; OnPropertyChanged(); }
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

        public int TargetModelIndex
        {
            get { return m_targetModelIndex; }
            set { m_targetModelIndex = value; OnPropertyChanged(); }
        }

        [Obsolete("Value overridden by the game.")]
        public uint Door1Pointer
        {
            get { return m_pDoor1; }
            set { m_pDoor1 = value; OnPropertyChanged(); }
        }

        [Obsolete("Value overridden by the game.")]
        public uint Door2Pointer
        {
            get { return m_pDoor2; }
            set { m_pDoor2 = value; OnPropertyChanged(); }
        }

        public byte Door1Handle
        {
            get { return m_door1Handle; }
            set { m_door1Handle = value; OnPropertyChanged(); }
        }

        public byte Door2Handle
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

        [Obsolete("Value overridden by the game.")]
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

        public float Z1
        {
            get { return m_z1; }
            set { m_z1 = value; OnPropertyChanged(); }
        }

        public float Z2
        {
            get { return m_z2; }
            set { m_z2 = value; OnPropertyChanged(); }
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
            get { return m_timeToStartAction; }
            set { m_timeToStartAction = value; OnPropertyChanged(); }
        }

        public byte CollectedCarsState
        {
            get { return m_collectedCarsState; }
            set { m_collectedCarsState = value; OnPropertyChanged(); }
        }

        [Obsolete("Value overridden by the game.")]
        public uint TargetCarPointer
        {
            get { return m_pTargetCar; }
            set { m_pTargetCar = value; OnPropertyChanged(); }
        }

        [Obsolete("Not used by the game.")]
        public int Field96h
        {
            get { return m_field96h; }
            set { m_field96h = value; OnPropertyChanged(); }
        }

        public StoredCar StoredCar
        {
            get { return m_storedCar; }
            set { m_storedCar = value; OnPropertyChanged(); }
        }

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

        public Vector3 PositionMin
        {
            get { return new Vector3(X1, Y1, Z1); }
            set { X1 = value.X; Y1 = value.Y; Z1 = value.Z; OnPropertyChanged(); }
        }

        public Vector3 PositionMax
        {
            get { return new Vector3(X2, Y2, Z2); }
            set { X2 = value.X; Y2 = value.Y; Z2 = value.Z; OnPropertyChanged(); }
        }

        public Vector3 Door1Position
        {
            get { return new Vector3(Door1X, Door1Y, Door1Z); }
            set { Door1X = value.X; Door1Y = value.Y; Door1Z = value.Z; OnPropertyChanged(); }
        }

        public Vector3 Door2Position
        {
            get { return new Vector3(Door2X, Door2Y, Door2Z); }
            set { Door2X = value.X; Door2Y = value.Y; Door2Z = value.Z; OnPropertyChanged(); }
        }

        public Garage()
        {
            StoredCar = new StoredCar();
        }

        public Garage(Garage other)
        {
            Type = other.Type;
            State = other.State;
            Field02h = other.Field02h;
            ClosingWithoutTargetVehicle = other.ClosingWithoutTargetVehicle;
            Deactivated = other.Deactivated;
            ResprayHappened = other.ResprayHappened;
            TargetModelIndex = other.TargetModelIndex;
            Door1Pointer = other.Door1Pointer;
            Door2Pointer = other.Door2Pointer;
            Door1Handle = other.Door1Handle;
            Door2Handle = other.Door2Handle;
            IsDoor1Dummy = other.IsDoor1Dummy;
            IsDoor2Dummy = other.IsDoor2Dummy;
            RecreateDoorOnNextRefresh = other.RecreateDoorOnNextRefresh;
            RotatingDoor = other.RotatingDoor;
            CameraFollowsPlayer = other.CameraFollowsPlayer;
            X1 = other.X1;
            X2 = other.X2;
            Y1 = other.Y1;
            Y2 = other.Y2;
            Z1 = other.Z1;
            Z2 = other.Z2;
            DoorOpenOffset = other.DoorOpenOffset;
            DoorOpenMax = other.DoorOpenMax;
            Door1X = other.Door1X;
            Door1Y = other.Door1Y;
            Door2X = other.Door2X;
            Door2Y = other.Door2Y;
            Door1Z = other.Door1Z;
            Door2Z = other.Door2Z;
            Timer = other.Timer;
            CollectedCarsState = other.CollectedCarsState;
            TargetCarPointer = other.TargetCarPointer;
            Field96h = other.Field96h;
            StoredCar = new StoredCar(other.StoredCar);
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            Type = (GarageType) buf.ReadByte();
            State = (GarageState) buf.ReadByte();
            Field02h = buf.ReadBool();
            ClosingWithoutTargetVehicle = buf.ReadBool();
            Deactivated = buf.ReadBool();
            ResprayHappened = buf.ReadBool();
            buf.Skip(2);
            TargetModelIndex = buf.ReadInt32();
            Door1Pointer = buf.ReadUInt32();
            Door2Pointer = buf.ReadUInt32();
            Door1Handle = buf.ReadByte();
            Door2Handle = buf.ReadByte();
            IsDoor1Dummy = buf.ReadBool();
            IsDoor2Dummy = buf.ReadBool();
            RecreateDoorOnNextRefresh = buf.ReadBool();
            RotatingDoor = buf.ReadBool();
            CameraFollowsPlayer = buf.ReadBool();
            buf.Skip(1);
            X1 = buf.ReadFloat();
            X2 = buf.ReadFloat();
            Y1 = buf.ReadFloat();
            Y2 = buf.ReadFloat();
            Z1 = buf.ReadFloat();
            Z2 = buf.ReadFloat();
            DoorOpenOffset = buf.ReadFloat();
            DoorOpenMax = buf.ReadFloat();
            Door1X = buf.ReadFloat();
            Door1Y = buf.ReadFloat();
            Door2X = buf.ReadFloat();
            Door2Y = buf.ReadFloat();
            Door1Z = buf.ReadFloat();
            Door2Z = buf.ReadFloat();
            Timer = buf.ReadUInt32();
            CollectedCarsState = buf.ReadByte();
            buf.Skip(3);
            TargetCarPointer = buf.ReadUInt32();
            Field96h = buf.ReadInt32();
            StoredCar = buf.ReadObject<StoredCar>();

            Debug.Assert(buf.Offset == SizeOfType<Garage>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write((byte) Type);
            buf.Write((byte) State);
            buf.Write(Field02h);
            buf.Write(ClosingWithoutTargetVehicle);
            buf.Write(Deactivated);
            buf.Write(ResprayHappened);
            buf.Skip(2);
            buf.Write(TargetModelIndex);
            buf.Write(Door1Pointer);
            buf.Write(Door2Pointer);
            buf.Write(Door1Handle);
            buf.Write(Door2Handle);
            buf.Write(IsDoor1Dummy);
            buf.Write(IsDoor2Dummy);
            buf.Write(RecreateDoorOnNextRefresh);
            buf.Write(RotatingDoor);
            buf.Write(CameraFollowsPlayer);
            buf.Skip(1);
            buf.Write(X1);
            buf.Write(X2);
            buf.Write(Y1);
            buf.Write(Y2);
            buf.Write(Z1);
            buf.Write(Z2);
            buf.Write(DoorOpenOffset);
            buf.Write(DoorOpenMax);
            buf.Write(Door1X);
            buf.Write(Door1Y);
            buf.Write(Door2X);
            buf.Write(Door2Y);
            buf.Write(Door1Z);
            buf.Write(Door2Z);
            buf.Write(Timer);
            buf.Write(CollectedCarsState);
            buf.Skip(3);
            buf.Write(TargetCarPointer);
            buf.Write(Field96h);
            buf.Write(StoredCar);

            Debug.Assert(buf.Offset == SizeOfType<Garage>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x8C;
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
                && Field02h.Equals(other.Field02h)
                && ClosingWithoutTargetVehicle.Equals(other.ClosingWithoutTargetVehicle)
                && Deactivated.Equals(other.Deactivated)
                && ResprayHappened.Equals(other.ResprayHappened)
                && TargetModelIndex.Equals(other.TargetModelIndex)
                && Door1Pointer.Equals(other.Door1Pointer)
                && Door2Pointer.Equals(other.Door2Pointer)
                && Door1Handle.Equals(other.Door1Handle)
                && Door2Handle.Equals(other.Door2Handle)
                && IsDoor1Dummy.Equals(other.IsDoor1Dummy)
                && IsDoor2Dummy.Equals(other.IsDoor2Dummy)
                && RecreateDoorOnNextRefresh.Equals(other.RecreateDoorOnNextRefresh)
                && RotatingDoor.Equals(other.RotatingDoor)
                && CameraFollowsPlayer.Equals(other.CameraFollowsPlayer)
                && X1.Equals(other.X1)
                && X2.Equals(other.X2)
                && Y1.Equals(other.Y1)
                && Y2.Equals(other.Y2)
                && Z1.Equals(other.Z1)
                && Z2.Equals(other.Z2)
                && DoorOpenOffset.Equals(other.DoorOpenOffset)
                && DoorOpenMax.Equals(other.DoorOpenMax)
                && Door1X.Equals(other.Door1X)
                && Door1Y.Equals(other.Door1Y)
                && Door2X.Equals(other.Door2X)
                && Door2Y.Equals(other.Door2Y)
                && Door1Z.Equals(other.Door1Z)
                && Door2Z.Equals(other.Door2Z)
                && Timer.Equals(other.Timer)
                && CollectedCarsState.Equals(other.CollectedCarsState)
                && TargetCarPointer.Equals(other.TargetCarPointer)
                && Field96h.Equals(other.Field96h)
                && StoredCar.Equals(other.StoredCar);
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
        MissionKeepCarRemainClosed
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
