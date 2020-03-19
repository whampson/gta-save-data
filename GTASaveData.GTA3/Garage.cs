using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x8C)]
    public class Garage : GTAObject,
        IEquatable<Garage>
    {
        private GarageType m_type;
        private GarageState m_state;
        private byte m_field02h;
        private bool m_closingWithoutTargetVehicle;
        private bool m_deactivated;
        private bool m_resprayHappened;
        private byte m_field06h;
        private byte m_field07h;
        private VehicleType m_targetModel;
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

        public VehicleType TargetVehicle
        {
            get { return m_targetModel; }
            set { m_targetModel = value; OnPropertyChanged(); }
        }

        public uint Door1
        {
            get { return m_pDoor1; }
            set { m_pDoor1 = value; OnPropertyChanged(); }
        }

        public uint Door2
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

        public Vector PosInf
        {
            get { return m_vecInf; }
            set { m_vecInf = value; OnPropertyChanged(); }
        }

        public Vector PosSup
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
            m_vecInf = new Vector();
            m_vecSup = new Vector();
            m_door1Pos = new Vector();
            m_door2Pos = new Vector();
            m_storedCar = new StoredCar();
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            m_type = (GarageType) buf.ReadByte();
            m_state = (GarageState) buf.ReadByte();
            m_field02h = buf.ReadByte();
            m_closingWithoutTargetVehicle = buf.ReadBool();
            m_deactivated = buf.ReadBool();
            m_resprayHappened = buf.ReadBool();
            m_field06h = buf.ReadByte();
            m_field07h = buf.ReadByte();
            m_targetModel = (VehicleType) buf.ReadInt32();
            m_pDoor1 = buf.ReadUInt32();
            m_pDoor2 = buf.ReadUInt32();
            m_isDoor1PoolIndex = buf.ReadBool();
            m_isDoor2PoolIndex = buf.ReadBool();
            m_isDoor1Object = buf.ReadBool();
            m_isDoor2Object = buf.ReadBool();
            m_field24h = buf.ReadByte();
            m_isRotatedDoor = buf.ReadBool();
            m_cameraFollowsPlayer = buf.ReadBool();
            m_field27h = buf.ReadByte();
            m_vecInf.X = buf.ReadSingle();
            m_vecSup.X = buf.ReadSingle();
            m_vecInf.Y = buf.ReadSingle();
            m_vecSup.Y = buf.ReadSingle();
            m_vecInf.Z = buf.ReadSingle();
            m_vecSup.Z = buf.ReadSingle();
            m_doorOpenMinZOffset = buf.ReadSingle();
            m_doorOpenMaxZOffset = buf.ReadSingle();
            m_door1Pos.X = buf.ReadSingle();
            m_door1Pos.Y = buf.ReadSingle();
            m_door2Pos.X = buf.ReadSingle();
            m_door2Pos.Y = buf.ReadSingle();
            m_door1Pos.Z = buf.ReadSingle();
            m_door2Pos.Z = buf.ReadSingle();
            m_doorLastOpenTime = buf.ReadUInt32();
            m_collectedCarsState = buf.ReadByte();
            m_field89h = buf.ReadByte();
            m_field90h = buf.ReadByte();
            m_field91h = buf.ReadByte();
            m_pTargetVehicle = buf.ReadUInt32();
            m_field96h = buf.ReadInt32();
            m_storedCar = buf.ReadObject<StoredCar>();

            Debug.Assert(buf.Offset == SizeOf<Garage>());
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write((byte) m_type);
            buf.Write((byte) m_state);
            buf.Write(m_field02h);
            buf.Write(m_closingWithoutTargetVehicle);
            buf.Write(m_deactivated);
            buf.Write(m_resprayHappened);
            buf.Write(m_field06h);
            buf.Write(m_field07h);
            buf.Write((int) m_targetModel);
            buf.Write(m_pDoor1);
            buf.Write(m_pDoor2);
            buf.Write(m_isDoor1PoolIndex);
            buf.Write(m_isDoor2PoolIndex);
            buf.Write(m_isDoor1Object);
            buf.Write(m_isDoor2Object);
            buf.Write(m_field24h);
            buf.Write(m_isRotatedDoor);
            buf.Write(m_cameraFollowsPlayer);
            buf.Write(m_field27h);
            buf.Write(m_vecInf.X);
            buf.Write(m_vecSup.X);
            buf.Write(m_vecInf.Y);
            buf.Write(m_vecSup.Y);
            buf.Write(m_vecInf.Z);
            buf.Write(m_vecSup.Z);
            buf.Write(m_doorOpenMinZOffset);
            buf.Write(m_doorOpenMaxZOffset);
            buf.Write(m_door1Pos.X);
            buf.Write(m_door1Pos.Y);
            buf.Write(m_door2Pos.X);
            buf.Write(m_door2Pos.Y);
            buf.Write(m_door1Pos.Z);
            buf.Write(m_door2Pos.Z);
            buf.Write(m_doorLastOpenTime);
            buf.Write(m_collectedCarsState);
            buf.Write(m_field89h);
            buf.Write(m_field90h);
            buf.Write(m_field91h);
            buf.Write(m_pTargetVehicle);
            buf.Write(m_field96h);
            buf.Write(m_storedCar);

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

            return m_type.Equals(other.m_type)
                && m_state.Equals(other.m_state)
                && m_field02h.Equals(other.m_field02h)
                && m_closingWithoutTargetVehicle.Equals(other.m_closingWithoutTargetVehicle)
                && m_deactivated.Equals(other.m_deactivated)
                && m_resprayHappened.Equals(other.m_resprayHappened)
                && m_field06h.Equals(other.m_field06h)
                && m_field07h.Equals(other.m_field07h)
                && m_targetModel.Equals(other.m_targetModel)
                && m_pDoor1.Equals(other.m_pDoor1)
                && m_pDoor2.Equals(other.m_pDoor2)
                && m_isDoor1PoolIndex.Equals(other.m_isDoor1PoolIndex)
                && m_isDoor2PoolIndex.Equals(other.m_isDoor2PoolIndex)
                && m_isDoor1Object.Equals(other.m_isDoor1Object)
                && m_isDoor2Object.Equals(other.m_isDoor2Object)
                && m_field24h.Equals(other.m_field24h)
                && m_isRotatedDoor.Equals(other.m_isRotatedDoor)
                && m_cameraFollowsPlayer.Equals(other.m_cameraFollowsPlayer)
                && m_field27h.Equals(other.m_field27h)
                && m_vecInf.Equals(other.m_vecInf)
                && m_vecSup.Equals(other.m_vecSup)
                && m_doorOpenMinZOffset.Equals(other.m_doorOpenMinZOffset)
                && m_doorOpenMaxZOffset.Equals(other.m_doorOpenMaxZOffset)
                && m_door1Pos.Equals(other.m_door1Pos)
                && m_door2Pos.Equals(other.m_door2Pos)
                && m_doorLastOpenTime.Equals(other.m_doorLastOpenTime)
                && m_collectedCarsState.Equals(other.m_collectedCarsState)
                && m_field89h.Equals(other.m_field89h)
                && m_field90h.Equals(other.m_field90h)
                && m_field91h.Equals(other.m_field91h)
                && m_pTargetVehicle.Equals(other.m_pTargetVehicle)
                && m_field96h.Equals(other.m_field96h)
                && m_storedCar.Equals(other.m_storedCar);
        }
    }
}
