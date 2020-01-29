using GTASaveData.Common;
using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    public sealed class Garage : SaveDataObject,
        IEquatable<Garage>
    {
        private GarageType m_type;
        private GarageState m_state;
        private byte m_unknown0;
        private bool m_closingWithoutTargetVehicle;
        private bool m_deactivated;
        private bool m_resprayHappened;
        private int m_targetVehicle;
        private uint m_door1Pointer;
        private uint m_door2Pointer;
        private bool m_isDoor1PoolIndex;
        private bool m_isDoor2PoolIndex;
        private bool m_isDoor1Object;
        private bool m_isDoor2Object;
        private byte m_unknown1;
        private bool m_isRotatedDoor;
        private bool m_cameraFollowsPlayer;
        private Rect3d m_position;
        private float m_doorOpenMinZOffset;
        private float m_doorOpenMaxZOffset;
        private Vector2d m_door1XY;
        private Vector2d m_door2XY;
        private float m_door1Z;
        private float m_door2Z;
        private uint m_doorLastOpenTime;
        private byte m_collectedCarsState;
        private uint m_targetVehiclePointer;
        private uint m_unknown3;
        private StoredCar m_unknown4;

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

        public byte Unknown0
        {
            get { return m_unknown0; }
            set { m_unknown0 = value; OnPropertyChanged(); }
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

        public int TargetVehicle
        {
            get { return m_targetVehicle; }
            set { m_targetVehicle = value; OnPropertyChanged(); }
        }

        public uint Door1Pointer
        {
            get { return m_door1Pointer; }
            set { m_door1Pointer = value; OnPropertyChanged(); }
        }

        public uint Door2Pointer
        {
            get { return m_door2Pointer; }
            set { m_door2Pointer = value; OnPropertyChanged(); }
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

        public byte Unknown1
        {
            get { return m_unknown1; }
            set { m_unknown1 = value; OnPropertyChanged(); }
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

        public Rect3d Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
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

        public Vector2d Door1XY
        {
            get { return m_door1XY; }
            set { m_door1XY = value; OnPropertyChanged(); }
        }

        public Vector2d Door2XY
        {
            get { return m_door2XY; }
            set { m_door2XY = value; OnPropertyChanged(); }
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

        public uint TargetVehiclePointer
        {
            get { return m_targetVehiclePointer; }
            set { m_targetVehiclePointer = value; OnPropertyChanged(); }
        }

        public uint Unknown3
        {
            get { return m_unknown3; }
            set { m_unknown3 = value; OnPropertyChanged(); }
        }

        public StoredCar Unknown4
        {
            get { return m_unknown4; }
            set { m_unknown4 = value; OnPropertyChanged(); }
        }

        public Garage()
        {
            m_position = new Rect3d();
            m_door1XY = new Vector2d();
            m_door2XY = new Vector2d();
            m_unknown4 = new StoredCar();
        }

        private Garage(SaveDataSerializer serializer, FileFormat format)
        {
            m_type = (GarageType) serializer.ReadByte();
            m_state = (GarageState) serializer.ReadByte();
            m_unknown0 = serializer.ReadByte();
            m_closingWithoutTargetVehicle = serializer.ReadBool();
            m_deactivated = serializer.ReadBool();
            m_resprayHappened = serializer.ReadBool();
            serializer.Align();
            m_targetVehicle = serializer.ReadInt32();
            m_door1Pointer = serializer.ReadUInt32();
            m_door2Pointer = serializer.ReadUInt32();
            m_isDoor1PoolIndex = serializer.ReadBool();
            m_isDoor2PoolIndex = serializer.ReadBool();
            m_isDoor1Object = serializer.ReadBool();
            m_isDoor2Object = serializer.ReadBool();
            m_unknown1 = serializer.ReadByte();
            m_isRotatedDoor = serializer.ReadBool();
            m_cameraFollowsPlayer = serializer.ReadBool();
            serializer.Align();
            m_position = serializer.ReadObject<Rect3d>();
            m_doorOpenMinZOffset = serializer.ReadSingle();
            m_doorOpenMaxZOffset = serializer.ReadSingle();
            m_door1XY = serializer.ReadObject<Vector2d>();
            m_door2XY = serializer.ReadObject<Vector2d>();
            m_door1Z = serializer.ReadSingle();
            m_door2Z = serializer.ReadSingle();
            m_doorLastOpenTime = serializer.ReadUInt32();
            m_collectedCarsState = serializer.ReadByte();
            serializer.Align();
            m_targetVehiclePointer = serializer.ReadUInt32();
            m_unknown3 = serializer.ReadUInt32();
            m_unknown4 = serializer.ReadObject<StoredCar>();
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.Write((byte) m_type);
            serializer.Write((byte) m_state);
            serializer.Write(m_unknown0);
            serializer.Write(m_closingWithoutTargetVehicle);
            serializer.Write(m_deactivated);
            serializer.Write(m_resprayHappened);
            serializer.Align();
            serializer.Write(m_targetVehicle);
            serializer.Write(m_door1Pointer);
            serializer.Write(m_door2Pointer);
            serializer.Write(m_isDoor1PoolIndex);
            serializer.Write(m_isDoor2PoolIndex);
            serializer.Write(m_isDoor1Object);
            serializer.Write(m_isDoor2Object);
            serializer.Write(m_unknown1);
            serializer.Write(m_isRotatedDoor);
            serializer.Write(m_cameraFollowsPlayer);
            serializer.Align();
            serializer.WriteObject(m_position);
            serializer.Write(m_doorOpenMinZOffset);
            serializer.Write(m_doorOpenMaxZOffset);
            serializer.WriteObject(m_door1XY);
            serializer.WriteObject(m_door2XY);
            serializer.Write(m_door1Z);
            serializer.Write(m_door2Z);
            serializer.Write(m_doorLastOpenTime);
            serializer.Write(m_collectedCarsState);
            serializer.Align();
            serializer.Write(m_targetVehiclePointer);
            serializer.Write(m_unknown3);
            serializer.WriteObject(m_unknown4);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
                && m_unknown0.Equals(other.m_unknown0)
                && m_closingWithoutTargetVehicle.Equals(other.m_closingWithoutTargetVehicle)
                && m_deactivated.Equals(other.m_deactivated)
                && m_resprayHappened.Equals(other.m_resprayHappened)
                && m_targetVehicle.Equals(other.m_targetVehicle)
                && m_door1Pointer.Equals(other.m_door1Pointer)
                && m_door2Pointer.Equals(other.m_door2Pointer)
                && m_isDoor1PoolIndex.Equals(other.m_isDoor1PoolIndex)
                && m_isDoor2PoolIndex.Equals(other.m_isDoor2PoolIndex)
                && m_isDoor1Object.Equals(other.m_isDoor1Object)
                && m_isDoor2Object.Equals(other.m_isDoor2Object)
                && m_unknown1.Equals(other.m_unknown1)
                && m_isRotatedDoor.Equals(other.m_isRotatedDoor)
                && m_cameraFollowsPlayer.Equals(other.m_cameraFollowsPlayer)
                && m_position.Equals(other.m_position)
                && m_doorOpenMinZOffset.Equals(other.m_doorOpenMinZOffset)
                && m_doorOpenMaxZOffset.Equals(other.m_doorOpenMaxZOffset)
                && m_door1XY.Equals(other.m_door1XY)
                && m_door2XY.Equals(other.m_door2XY)
                && m_door1Z.Equals(other.m_door1Z)
                && m_door2Z.Equals(other.m_door2Z)
                && m_doorLastOpenTime.Equals(other.m_doorLastOpenTime)
                && m_collectedCarsState.Equals(other.m_collectedCarsState)
                && m_targetVehiclePointer.Equals(other.m_targetVehiclePointer)
                && m_unknown3.Equals(other.m_unknown3)
                && m_unknown4.Equals(other.m_unknown4);
        }
    }
}
