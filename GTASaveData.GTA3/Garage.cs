using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x8C)]
    public class Garage : SaveDataObject, IEquatable<Garage>
    {
        private GarageType m_type;
        private GarageState m_state;
        private byte m_field02h;
        private bool m_closingWithoutTargetCar;
        private bool m_deactivated;
        private bool m_resprayHappened;
        private int m_targetModelIndex;
        private uint m_pDoor1;
        private uint m_pDoor2;
        private byte m_door1PoolIndex;
        private byte m_door2PoolIndex;
        private bool m_isDoor1Dummy;
        private bool m_isDoor2Dummy;
        private bool m_recreateDoorOnNextRefresh;
        private bool m_rotatedDoor;
        private bool m_cameraFollowsPlayer;
        private byte m_field27h;
        private Vector m_posInf;
        private Vector m_posSup;
        private float m_doorPos;
        private float m_doorHeight;
        private Vector m_door1Pos;
        private Vector m_door2Pos;
        private uint m_timeToStartAction;
        private byte m_collectedCarsState;
        private uint m_pTargetCar;
        private int m_field96h;
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

        public byte Field02h
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

        public byte Door1PoolIndex
        {
            get { return m_door1PoolIndex; }
            set { m_door1PoolIndex = value; OnPropertyChanged(); }
        }

        public byte Door2PoolIndex
        {
            get { return m_door2PoolIndex; }
            set { m_door2PoolIndex = value; OnPropertyChanged(); }
        }

        public bool IsDoor1Object
        {
            get { return m_isDoor1Dummy; }
            set { m_isDoor1Dummy = value; OnPropertyChanged(); }
        }

        public bool IsDoor2Object
        {
            get { return m_isDoor2Dummy; }
            set { m_isDoor2Dummy = value; OnPropertyChanged(); }
        }

        public bool RecreateDoorOnNextRefresh
        {
            get { return m_recreateDoorOnNextRefresh; }
            set { m_recreateDoorOnNextRefresh = value; OnPropertyChanged(); }
        }

        public bool RotatedDoor
        {
            get { return m_rotatedDoor; }
            set { m_rotatedDoor = value; OnPropertyChanged(); }
        }

        public bool CameraFollowsPlayer
        {
            get { return m_cameraFollowsPlayer; }
            set { m_cameraFollowsPlayer = value; OnPropertyChanged(); }
        }

        public byte Field27h
        {
            get { return m_field27h; }
            set { m_field27h = value; OnPropertyChanged(); }
        }

        public Vector Position1
        {
            get { return m_posInf; }
            set { m_posInf = value; OnPropertyChanged(); }
        }

        public Vector Position2
        {
            get { return m_posSup; }
            set { m_posSup = value; OnPropertyChanged(); }
        }

        public float DoorPosition
        {
            get { return m_doorPos; }
            set { m_doorPos = value; OnPropertyChanged(); }
        }

        public float DoorHeight
        {
            get { return m_doorHeight; }
            set { m_doorHeight = value; OnPropertyChanged(); }
        }

        public Vector Door1Pos
        {
            get { return m_door1Pos; }
            set { m_door1Pos = value; OnPropertyChanged(); }
        }

        public Vector Door2Pos
        {
            get { return m_door2Pos; }
            set { m_door2Pos = value; OnPropertyChanged(); }
        }

        public uint DoorLastOpenTime
        {
            get { return m_timeToStartAction; }
            set { m_timeToStartAction = value; OnPropertyChanged(); }
        }

        public byte CollectedCarsState
        {
            get { return m_collectedCarsState; }
            set { m_collectedCarsState = value; OnPropertyChanged(); }
        }

        public uint TargetCarPointer
        {
            get { return m_pTargetCar; }
            set { m_pTargetCar = value; OnPropertyChanged(); }
        }

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

        public Garage()
        {
            Position1 = new Vector();
            Position2 = new Vector();
            Door1Pos = new Vector();
            Door2Pos = new Vector();
            StoredCar = new StoredCar();
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            Type = (GarageType) buf.ReadByte();
            State = (GarageState) buf.ReadByte();
            Field02h = buf.ReadByte();
            ClosingWithoutTargetVehicle = buf.ReadBool();
            Deactivated = buf.ReadBool();
            ResprayHappened = buf.ReadBool();
            buf.Align4Bytes();
            TargetModelIndex = buf.ReadInt32();
            Door1Pointer = buf.ReadUInt32();
            Door2Pointer = buf.ReadUInt32();
            Door1PoolIndex = buf.ReadByte();
            Door2PoolIndex = buf.ReadByte();
            IsDoor1Object = buf.ReadBool();
            IsDoor2Object = buf.ReadBool();
            RecreateDoorOnNextRefresh = buf.ReadBool();
            RotatedDoor = buf.ReadBool();
            CameraFollowsPlayer = buf.ReadBool();
            Field27h = buf.ReadByte();
            Position1.X = buf.ReadFloat();
            Position2.X = buf.ReadFloat();
            Position1.Y = buf.ReadFloat();
            Position2.Y = buf.ReadFloat();
            Position1.Z = buf.ReadFloat();
            Position2.Z = buf.ReadFloat();
            DoorPosition = buf.ReadFloat();
            DoorHeight = buf.ReadFloat();
            Door1Pos.X = buf.ReadFloat();
            Door1Pos.Y = buf.ReadFloat();
            Door2Pos.X = buf.ReadFloat();
            Door2Pos.Y = buf.ReadFloat();
            Door1Pos.Z = buf.ReadFloat();
            Door2Pos.Z = buf.ReadFloat();
            DoorLastOpenTime = buf.ReadUInt32();
            CollectedCarsState = buf.ReadByte();
            buf.Align4Bytes();
            TargetCarPointer = buf.ReadUInt32();
            Field96h = buf.ReadInt32();
            StoredCar = buf.Read<StoredCar>();

            Debug.Assert(buf.Offset == SizeOf<Garage>());
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write((byte) Type);
            buf.Write((byte) State);
            buf.Write(Field02h);
            buf.Write(ClosingWithoutTargetVehicle);
            buf.Write(Deactivated);
            buf.Write(ResprayHappened);
            buf.Align4Bytes();
            buf.Write(TargetModelIndex);
            buf.Write(Door1Pointer);
            buf.Write(Door2Pointer);
            buf.Write(Door1PoolIndex);
            buf.Write(Door2PoolIndex);
            buf.Write(IsDoor1Object);
            buf.Write(IsDoor2Object);
            buf.Write(RecreateDoorOnNextRefresh);
            buf.Write(RotatedDoor);
            buf.Write(CameraFollowsPlayer);
            buf.Write(Field27h);
            buf.Write(Position1.X);
            buf.Write(Position2.X);
            buf.Write(Position1.Y);
            buf.Write(Position2.Y);
            buf.Write(Position1.Z);
            buf.Write(Position2.Z);
            buf.Write(DoorPosition);
            buf.Write(DoorHeight);
            buf.Write(Door1Pos.X);
            buf.Write(Door1Pos.Y);
            buf.Write(Door2Pos.X);
            buf.Write(Door2Pos.Y);
            buf.Write(Door1Pos.Z);
            buf.Write(Door2Pos.Z);
            buf.Write(DoorLastOpenTime);
            buf.Write(CollectedCarsState);
            buf.Align4Bytes();
            buf.Write(TargetCarPointer);
            buf.Write(Field96h);
            buf.Write(StoredCar);

            Debug.Assert(buf.Offset == SizeOf<Garage>());
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
                && Door1PoolIndex.Equals(other.Door1PoolIndex)
                && Door2PoolIndex.Equals(other.Door2PoolIndex)
                && IsDoor1Object.Equals(other.IsDoor1Object)
                && IsDoor2Object.Equals(other.IsDoor2Object)
                && RecreateDoorOnNextRefresh.Equals(other.RecreateDoorOnNextRefresh)
                && RotatedDoor.Equals(other.RotatedDoor)
                && CameraFollowsPlayer.Equals(other.CameraFollowsPlayer)
                && Field27h.Equals(other.Field27h)
                && Position1.Equals(other.Position1)
                && Position2.Equals(other.Position2)
                && DoorPosition.Equals(other.DoorPosition)
                && DoorHeight.Equals(other.DoorHeight)
                && Door1Pos.Equals(other.Door1Pos)
                && Door2Pos.Equals(other.Door2Pos)
                && DoorLastOpenTime.Equals(other.DoorLastOpenTime)
                && CollectedCarsState.Equals(other.CollectedCarsState)
                && TargetCarPointer.Equals(other.TargetCarPointer)
                && Field96h.Equals(other.Field96h)
                && StoredCar.Equals(other.StoredCar);
        }
    }
}
