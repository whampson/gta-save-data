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
        private bool m_closingWithoutTargetVehicle;
        private bool m_deactivated;
        private bool m_resprayHappened;
        private byte m_field06h;
        private byte m_field07h;
        private int m_targetModelIndex;
        private uint m_pDoor1;
        private uint m_pDoor2;
        private bool m_isDoor1PoolIndex;
        private bool m_isDoor2PoolIndex;
        private bool m_isDoor1Object;
        private bool m_isDoor2Object;
        private byte m_field24h;
        private bool m_isRotatedDoor;
        private bool m_cameraFollowsPlayer;
        private byte m_field27h;
        private Vector m_vecInf;
        private Vector m_vecSup;
        private float m_doorOpenMinZOffset;
        private float m_doorOpenMaxZOffset;
        private Vector m_door1Pos;
        private Vector m_door2Pos;
        private uint m_doorLastOpenTime;
        private byte m_collectedCarsState;
        private byte m_field89h;
        private byte m_field90h;
        private byte m_field91h;
        private uint m_pTargetVehicle;
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
            get { return m_closingWithoutTargetVehicle; }
            set { m_closingWithoutTargetVehicle = value; OnPropertyChanged(); }
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

        public byte Field06h
        {
            get { return m_field06h; }
            set { m_field06h = value; OnPropertyChanged(); }
        }

        public byte Field07h
        {
            get { return m_field07h; }
            set { m_field07h = value; OnPropertyChanged(); }
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

        public bool IsDoor1PoolIndex
        {
            get { return m_isDoor1PoolIndex; }
            set { m_isDoor1PoolIndex = value; OnPropertyChanged(); }
        }

        public bool IsDoor2PoolIndex
        {
            get { return m_isDoor2PoolIndex; }
            set { m_isDoor2PoolIndex = value; OnPropertyChanged(); }
        }

        public bool IsDoor1Object
        {
            get { return m_isDoor1Object; }
            set { m_isDoor1Object = value; OnPropertyChanged(); }
        }

        public bool IsDoor2Object
        {
            get { return m_isDoor2Object; }
            set { m_isDoor2Object = value; OnPropertyChanged(); }
        }

        public byte Field24h
        {
            get { return m_field24h; }
            set { m_field24h = value; OnPropertyChanged(); }
        }

        public bool IsRotatedDoor
        {
            get { return m_isRotatedDoor; }
            set { m_isRotatedDoor = value; OnPropertyChanged(); }
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

        public Vector PositionInf
        {
            get { return m_vecInf; }
            set { m_vecInf = value; OnPropertyChanged(); }
        }

        public Vector PositionSup
        {
            get { return m_vecSup; }
            set { m_vecSup = value; OnPropertyChanged(); }
        }

        public float DoorOpenMinZOffset
        {
            get { return m_doorOpenMinZOffset; }
            set { m_doorOpenMinZOffset = value; OnPropertyChanged(); }
        }

        public float DoorOpenMaxZOffset
        {
            get { return m_doorOpenMaxZOffset; }
            set { m_doorOpenMaxZOffset = value; OnPropertyChanged(); }
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
            get { return m_doorLastOpenTime; }
            set { m_doorLastOpenTime = value; OnPropertyChanged(); }
        }

        public byte CollectedCarsState
        {
            get { return m_collectedCarsState; }
            set { m_collectedCarsState = value; OnPropertyChanged(); }
        }

        public byte Field89h
        {
            get { return m_field89h; }
            set { m_field89h = value; OnPropertyChanged(); }
        }

        public byte Field90h
        {
            get { return m_field90h; }
            set { m_field90h = value; OnPropertyChanged(); }
        }

        public byte Field91h
        {
            get { return m_field91h; }
            set { m_field91h = value; OnPropertyChanged(); }
        }

        public uint TargetVehiclePointer
        {
            get { return m_pTargetVehicle; }
            set { m_pTargetVehicle = value; OnPropertyChanged(); }
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
            PositionInf = new Vector();
            PositionSup = new Vector();
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
            Field06h = buf.ReadByte();
            Field07h = buf.ReadByte();
            TargetModelIndex = buf.ReadInt32();
            Door1Pointer = buf.ReadUInt32();
            Door2Pointer = buf.ReadUInt32();
            IsDoor1PoolIndex = buf.ReadBool();
            IsDoor2PoolIndex = buf.ReadBool();
            IsDoor1Object = buf.ReadBool();
            IsDoor2Object = buf.ReadBool();
            Field24h = buf.ReadByte();
            IsRotatedDoor = buf.ReadBool();
            CameraFollowsPlayer = buf.ReadBool();
            Field27h = buf.ReadByte();
            PositionInf.X = buf.ReadFloat();
            PositionSup.X = buf.ReadFloat();
            PositionInf.Y = buf.ReadFloat();
            PositionSup.Y = buf.ReadFloat();
            PositionInf.Z = buf.ReadFloat();
            PositionSup.Z = buf.ReadFloat();
            DoorOpenMinZOffset = buf.ReadFloat();
            DoorOpenMaxZOffset = buf.ReadFloat();
            Door1Pos.X = buf.ReadFloat();
            Door1Pos.Y = buf.ReadFloat();
            Door2Pos.X = buf.ReadFloat();
            Door2Pos.Y = buf.ReadFloat();
            Door1Pos.Z = buf.ReadFloat();
            Door2Pos.Z = buf.ReadFloat();
            DoorLastOpenTime = buf.ReadUInt32();
            CollectedCarsState = buf.ReadByte();
            Field89h = buf.ReadByte();
            Field90h = buf.ReadByte();
            Field91h = buf.ReadByte();
            TargetVehiclePointer = buf.ReadUInt32();
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
            buf.Write(Field06h);
            buf.Write(Field07h);
            buf.Write(TargetModelIndex);
            buf.Write(Door1Pointer);
            buf.Write(Door2Pointer);
            buf.Write(IsDoor1PoolIndex);
            buf.Write(IsDoor2PoolIndex);
            buf.Write(IsDoor1Object);
            buf.Write(IsDoor2Object);
            buf.Write(Field24h);
            buf.Write(IsRotatedDoor);
            buf.Write(CameraFollowsPlayer);
            buf.Write(Field27h);
            buf.Write(PositionInf.X);
            buf.Write(PositionSup.X);
            buf.Write(PositionInf.Y);
            buf.Write(PositionSup.Y);
            buf.Write(PositionInf.Z);
            buf.Write(PositionSup.Z);
            buf.Write(DoorOpenMinZOffset);
            buf.Write(DoorOpenMaxZOffset);
            buf.Write(Door1Pos.X);
            buf.Write(Door1Pos.Y);
            buf.Write(Door2Pos.X);
            buf.Write(Door2Pos.Y);
            buf.Write(Door1Pos.Z);
            buf.Write(Door2Pos.Z);
            buf.Write(DoorLastOpenTime);
            buf.Write(CollectedCarsState);
            buf.Write(Field89h);
            buf.Write(Field90h);
            buf.Write(Field91h);
            buf.Write(TargetVehiclePointer);
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
                && Field06h.Equals(other.Field06h)
                && Field07h.Equals(other.Field07h)
                && TargetModelIndex.Equals(other.TargetModelIndex)
                && Door1Pointer.Equals(other.Door1Pointer)
                && Door2Pointer.Equals(other.Door2Pointer)
                && IsDoor1PoolIndex.Equals(other.IsDoor1PoolIndex)
                && IsDoor2PoolIndex.Equals(other.IsDoor2PoolIndex)
                && IsDoor1Object.Equals(other.IsDoor1Object)
                && IsDoor2Object.Equals(other.IsDoor2Object)
                && Field24h.Equals(other.Field24h)
                && IsRotatedDoor.Equals(other.IsRotatedDoor)
                && CameraFollowsPlayer.Equals(other.CameraFollowsPlayer)
                && Field27h.Equals(other.Field27h)
                && PositionInf.Equals(other.PositionInf)
                && PositionSup.Equals(other.PositionSup)
                && DoorOpenMinZOffset.Equals(other.DoorOpenMinZOffset)
                && DoorOpenMaxZOffset.Equals(other.DoorOpenMaxZOffset)
                && Door1Pos.Equals(other.Door1Pos)
                && Door2Pos.Equals(other.Door2Pos)
                && DoorLastOpenTime.Equals(other.DoorLastOpenTime)
                && CollectedCarsState.Equals(other.CollectedCarsState)
                && Field89h.Equals(other.Field89h)
                && Field90h.Equals(other.Field90h)
                && Field91h.Equals(other.Field91h)
                && TargetVehiclePointer.Equals(other.TargetVehiclePointer)
                && Field96h.Equals(other.Field96h)
                && StoredCar.Equals(other.StoredCar);
        }
    }
}
