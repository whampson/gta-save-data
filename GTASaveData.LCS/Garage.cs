using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.LCS
{
    public class Garage : SaveDataObject,
        IEquatable<Garage>, IDeepClonable<Garage>
    {
        private GarageType m_type;
        private GarageState m_state;
        private int m_maxStoredCars;
        private int m_unusedField;
        private bool m_closingWithoutTargetCar;
        private bool m_deactivated;
        private bool m_resprayHappened;
        private int m_targetModelIndex;
        private long m_door1Pointer;
        private long m_door2Pointer;
        private int m_door1PoolIndex;
        private int m_door2PoolIndex;
        private bool m_door1IsDummy;
        private bool m_door2IsDummy;
        private bool m_recreateDoorsOnNextRefresh;
        private bool m_rotatingDoor;
        private bool m_cameraFollowsPlayer;
        private Vector2D m_corner1;
        private float m_infZ;
        private Vector2D m_dir1;
        private Vector2D m_dir2;
        private float m_supZ;
        private Vector3D m_scriptGaragePos;
        private float m_scriptGarageAngle;
        private int m_unknown1;
        private int m_unknown2;
        private int m_unknown3;
        private int m_unknown4;
        private float m_dir1Len;
        private float m_dir2Len;
        private float m_infX;
        private float m_supX;
        private float m_infY;
        private float m_supY;
        private int m_timeCrusherCraneActivated;
        private int m_scriptTargetCar;
        private float m_doorPos;
        private float m_doorHeight;
        private float m_door1X;
        private float m_door1Y;
        private float m_door2X;
        private float m_door2Y;
        private float m_door1Z;
        private float m_door2Z;
        private int m_timeToStartAction;
        private int m_collectedCarState;
        private long m_targetCarPointer;
        private StoredCar m_storedCar;
        private bool m_initialized;
        private int m_multiplayerCar;
        private bool m_scriptGarageAcceptedCar;
        private bool m_locked;
        private int m_scriptGarageState;
        private bool m_scriptGarageStateChanging;

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

        public int MaxStoredCars
        {
            get { return m_maxStoredCars; }
            set { m_maxStoredCars = value; OnPropertyChanged(); }
        }

        public int UnusedField
        {
            get { return m_unusedField; }
            set { m_unusedField = value; OnPropertyChanged(); }
        }

        public bool ClosingWithoutTargetCar
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

        public long Door1Pointer
        {
            get { return m_door1Pointer; }
            set { m_door1Pointer = value; OnPropertyChanged(); }
        }

        public long Door2Pointer
        {
            get { return m_door2Pointer; }
            set { m_door2Pointer = value; OnPropertyChanged(); }
        }

        public int Door1PoolIndex
        {
            get { return m_door1PoolIndex; }
            set { m_door1PoolIndex = value; OnPropertyChanged(); }
        }

        public int Door2PoolIndex
        {
            get { return m_door2PoolIndex; }
            set { m_door2PoolIndex = value; OnPropertyChanged(); }
        }

        public bool Door1IsDummy
        {
            get { return m_door1IsDummy; }
            set { m_door1IsDummy = value; OnPropertyChanged(); }
        }

        public bool Door2IsDummy
        {
            get { return m_door2IsDummy; }
            set { m_door2IsDummy = value; OnPropertyChanged(); }
        }

        public bool RecreateDoorsOnNextRefresh
        {
            get { return m_recreateDoorsOnNextRefresh; }
            set { m_recreateDoorsOnNextRefresh = value; OnPropertyChanged(); }
        }

        public bool RotatingDoor
        {
            get { return m_rotatingDoor; }
            set { m_rotatingDoor = value; OnPropertyChanged(); }
        }

        public bool CameraFollowsPlayer
        {
            get { return m_cameraFollowsPlayer; }
            set { m_cameraFollowsPlayer = value; OnPropertyChanged(); }
        }

        public Vector2D Corner1
        {
            get { return m_corner1; }
            set { m_corner1 = value; OnPropertyChanged(); }
        }

        public float InfZ
        {
            get { return m_infZ; }
            set { m_infZ = value; OnPropertyChanged(); }
        }

        public Vector2D Dir1
        {
            get { return m_dir1; }
            set { m_dir1 = value; OnPropertyChanged(); }
        }

        public Vector2D Dir2
        {
            get { return m_dir2; }
            set { m_dir2 = value; OnPropertyChanged(); }
        }

        public float SupZ
        {
            get { return m_supZ; }
            set { m_supZ = value; OnPropertyChanged(); }
        }

        public Vector3D ScriptGaragePos
        {
            get { return m_scriptGaragePos; }
            set { m_scriptGaragePos = value; OnPropertyChanged(); }
        }

        public float ScriptGarageAngle
        {
            get { return m_scriptGarageAngle; }
            set { m_scriptGarageAngle = value; OnPropertyChanged(); }
        }

        public int Unknown1
        {
            get { return m_unknown1; }
            set { m_unknown1 = value; OnPropertyChanged(); }
        }

        public int Unknown2
        {
            get { return m_unknown2; }
            set { m_unknown2 = value; OnPropertyChanged(); }
        }

        public int Unknown3
        {
            get { return m_unknown3; }
            set { m_unknown3 = value; OnPropertyChanged(); }
        }

        public int Unknown4
        {
            get { return m_unknown4; }
            set { m_unknown4 = value; OnPropertyChanged(); }
        }

        public float Dir1Len
        {
            get { return m_dir1Len; }
            set { m_dir1Len = value; OnPropertyChanged(); }
        }

        public float Dir2Len
        {
            get { return m_dir2Len; }
            set { m_dir2Len = value; OnPropertyChanged(); }
        }

        public float InfX
        {
            get { return m_infX; }
            set { m_infX = value; OnPropertyChanged(); }
        }

        public float SupX
        {
            get { return m_supX; }
            set { m_supX = value; OnPropertyChanged(); }
        }

        public float InfY
        {
            get { return m_infY; }
            set { m_infY = value; OnPropertyChanged(); }
        }

        public float SupY
        {
            get { return m_supY; }
            set { m_supY = value; OnPropertyChanged(); }
        }

        public int TimeCrusherCraneActivated
        {
            get { return m_timeCrusherCraneActivated; }
            set { m_timeCrusherCraneActivated = value; OnPropertyChanged(); }
        }

        public int ScriptTargetCar
        {
            get { return m_scriptTargetCar; }
            set { m_scriptTargetCar = value; OnPropertyChanged(); }
        }

        public float DoorPos
        {
            get { return m_doorPos; }
            set { m_doorPos = value; OnPropertyChanged(); }
        }

        public float DoorHeight
        {
            get { return m_doorHeight; }
            set { m_doorHeight = value; OnPropertyChanged(); }
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

        public int TimeToStartAction
        {
            get { return m_timeToStartAction; }
            set { m_timeToStartAction = value; OnPropertyChanged(); }
        }

        public int CollectedCarState
        {
            get { return m_collectedCarState; }
            set { m_collectedCarState = value; OnPropertyChanged(); }
        }

        public long TargetCarPointer
        {
            get { return m_targetCarPointer; }
            set { m_targetCarPointer = value; OnPropertyChanged(); }
        }

        public StoredCar StoredCar
        {
            get { return m_storedCar; }
            set { m_storedCar = value; OnPropertyChanged(); }
        }

        public bool Initialized
        {
            get { return m_initialized; }
            set { m_initialized = value; OnPropertyChanged(); }
        }

        public int MultiplayerCar
        {
            get { return m_multiplayerCar; }
            set { m_multiplayerCar = value; OnPropertyChanged(); }
        }

        public bool ScriptGarageAcceptedCar
        {
            get { return m_scriptGarageAcceptedCar; }
            set { m_scriptGarageAcceptedCar = value; OnPropertyChanged(); }
        }

        public bool Locked
        {
            get { return m_locked; }
            set { m_locked = value; OnPropertyChanged(); }
        }

        public int ScriptGarageState
        {
            get { return m_scriptGarageState; }
            set { m_scriptGarageState = value; OnPropertyChanged(); }
        }

        public bool ScriptGarageStateChanging
        {
            get { return m_scriptGarageStateChanging; }
            set { m_scriptGarageStateChanging = value; OnPropertyChanged(); }
        }

        public Garage()
        {
            m_storedCar = new StoredCar();
        }

        public Garage(Garage other)
        {
            m_type = other.m_type;
            m_state = other.m_state;
            m_maxStoredCars = other.m_maxStoredCars;
            m_unusedField = other.m_unusedField;
            m_closingWithoutTargetCar = other.m_closingWithoutTargetCar;
            m_deactivated = other.m_deactivated;
            m_resprayHappened = other.m_resprayHappened;
            m_targetModelIndex = other.m_targetModelIndex;
            m_door1Pointer = other.m_door1Pointer;
            m_door2Pointer = other.m_door2Pointer;
            m_door1PoolIndex = other.m_door1PoolIndex;
            m_door2PoolIndex = other.m_door2PoolIndex;
            m_door1IsDummy = other.m_door1IsDummy;
            m_door2IsDummy = other.m_door2IsDummy;
            m_recreateDoorsOnNextRefresh = other.m_recreateDoorsOnNextRefresh;
            m_rotatingDoor = other.m_rotatingDoor;
            m_cameraFollowsPlayer = other.m_cameraFollowsPlayer;
            m_corner1 = other.m_corner1;
            m_infZ = other.m_infZ;
            m_dir1 = other.m_dir1;
            m_dir2 = other.m_dir2;
            m_supZ = other.m_supZ;
            m_scriptGaragePos = other.m_scriptGaragePos;
            m_scriptGarageAngle = other.m_scriptGarageAngle;
            m_unknown1 = other.m_unknown1;
            m_unknown2 = other.m_unknown2;
            m_unknown3 = other.m_unknown3;
            m_unknown4 = other.m_unknown4;
            m_dir1Len = other.m_dir1Len;
            m_dir2Len = other.m_dir2Len;
            m_infX = other.m_infX;
            m_supX = other.m_supX;
            m_infY = other.m_infY;
            m_supY = other.m_supY;
            m_timeCrusherCraneActivated = other.m_timeCrusherCraneActivated;
            m_scriptTargetCar = other.m_scriptTargetCar;
            m_doorPos = other.m_doorPos;
            m_doorHeight = other.m_doorHeight;
            m_door1X = other.m_door1X;
            m_door1Y = other.m_door1Y;
            m_door2X = other.m_door2X;
            m_door2Y = other.m_door2Y;
            m_door1Z = other.m_door1Z;
            m_door2Z = other.m_door2Z;
            m_timeToStartAction = other.m_timeToStartAction;
            m_collectedCarState = other.m_collectedCarState;
            m_targetCarPointer = other.m_targetCarPointer;
            m_storedCar = other.m_storedCar;
            m_initialized = other.m_initialized;
            m_multiplayerCar = other.m_multiplayerCar;
            m_scriptGarageAcceptedCar = other.m_scriptGarageAcceptedCar;
            m_locked = other.m_locked;
            m_scriptGarageState = other.m_scriptGarageState;
            m_scriptGarageStateChanging = other.m_scriptGarageStateChanging;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            if (fmt.IsPS2)
            {
                m_type = (GarageType) buf.ReadByte();
                m_state = (GarageState) buf.ReadByte();
                m_maxStoredCars = buf.ReadByte();
                buf.ReadByte(); // align
                m_unusedField = buf.ReadInt32();
                m_closingWithoutTargetCar = buf.ReadBool(4);
                m_deactivated = buf.ReadBool(4);
                m_resprayHappened = buf.ReadBool(4);
                m_targetModelIndex = buf.ReadInt32();
                m_door1Pointer = buf.ReadInt32();
                m_door2Pointer = buf.ReadInt32();
                m_door1PoolIndex = buf.ReadByte();
                m_door2PoolIndex = buf.ReadByte();
                buf.ReadByte(); // align
                buf.ReadByte(); // align
                m_door1IsDummy = buf.ReadBool(4);
                m_door2IsDummy = buf.ReadBool(4);
                m_recreateDoorsOnNextRefresh = buf.ReadBool(4);
                m_rotatingDoor = buf.ReadBool(4);
                m_cameraFollowsPlayer = buf.ReadBool(4);
                m_corner1 = buf.Read<Vector2D>();
                m_infZ = buf.ReadFloat();
                m_dir1 = buf.Read<Vector2D>();
                m_dir2 = buf.Read<Vector2D>();
                m_supZ = buf.ReadFloat();
                m_scriptGaragePos = buf.Read<Vector3D>();
                m_scriptGarageAngle = buf.ReadFloat();
                m_unknown1 = buf.ReadInt32();   // 0x68
                m_unknown2 = buf.ReadInt32();   // 0x6C
                m_unknown3 = buf.ReadInt32();   // 0x70
                m_dir1Len = buf.ReadFloat();
                m_dir2Len = buf.ReadFloat();
                m_infX = buf.ReadFloat();
                m_supX = buf.ReadFloat();
                m_infY = buf.ReadFloat();
                m_supY = buf.ReadFloat();
                m_timeCrusherCraneActivated = buf.ReadInt32();
                m_scriptTargetCar = buf.ReadInt32();
                m_doorPos = buf.ReadFloat();
                m_doorHeight = buf.ReadFloat();
                m_door1X = buf.ReadFloat();
                m_door1Y = buf.ReadFloat();
                m_door2X = buf.ReadFloat();
                m_door2Y = buf.ReadFloat();
                m_door1Z = buf.ReadFloat();
                m_door2Z = buf.ReadFloat();
                m_timeToStartAction = buf.ReadInt32();
                m_collectedCarState = buf.ReadInt32();
                m_targetCarPointer = buf.ReadInt32();
                m_storedCar = buf.Read<StoredCar>();
                m_initialized = buf.ReadBool(4);
                m_scriptGarageAcceptedCar = buf.ReadBool(4);
                m_locked = buf.ReadBool(4);
                m_scriptGarageState = buf.ReadInt32();
                m_scriptGarageStateChanging = buf.ReadBool(4);

                Debug.Assert(buf.Offset == 0x100);
            }
            else if (fmt.IsPSP)
            {
                m_type = (GarageType) buf.ReadByte();
                m_state = (GarageState) buf.ReadByte();
                m_maxStoredCars = buf.ReadByte();
                m_unusedField = buf.ReadByte();
                m_closingWithoutTargetCar = buf.ReadBool();
                m_deactivated = buf.ReadBool();
                m_resprayHappened = buf.ReadBool();
                buf.ReadByte(); // align
                m_targetModelIndex = buf.ReadInt32();
                m_door1Pointer = buf.ReadInt32();
                m_door2Pointer = buf.ReadInt32();
                m_door1PoolIndex = buf.ReadByte();
                m_door2PoolIndex = buf.ReadByte();
                m_door1IsDummy = buf.ReadBool();
                m_door2IsDummy = buf.ReadBool();
                m_recreateDoorsOnNextRefresh = buf.ReadBool();
                m_rotatingDoor = buf.ReadBool();
                m_cameraFollowsPlayer = buf.ReadBool();
                buf.ReadByte(); // align
                m_corner1 = buf.Read<Vector2D>();
                m_infZ = buf.ReadFloat();
                m_dir1 = buf.Read<Vector2D>();
                m_dir2 = buf.Read<Vector2D>();
                m_supZ = buf.ReadFloat();
                m_scriptGaragePos = buf.Read<Vector3D>();
                m_scriptGarageAngle = buf.ReadFloat();
                m_unknown1 = buf.ReadInt32();   // 0x4C
                m_unknown2 = buf.ReadInt32();   // 0x50
                m_dir1Len = buf.ReadFloat();
                m_dir2Len = buf.ReadFloat();
                m_infX = buf.ReadFloat();
                m_supX = buf.ReadFloat();
                m_infY = buf.ReadFloat();
                m_supY = buf.ReadFloat();
                m_timeCrusherCraneActivated = buf.ReadInt32();
                m_scriptTargetCar = buf.ReadInt32();
                m_doorPos = buf.ReadFloat();
                m_doorHeight = buf.ReadFloat();
                m_door1X = buf.ReadFloat();
                m_door1Y = buf.ReadFloat();
                m_door2X = buf.ReadFloat();
                m_door2Y = buf.ReadFloat();
                m_door1Z = buf.ReadFloat();
                m_door2Z = buf.ReadFloat();
                m_timeToStartAction = buf.ReadInt32();
                m_collectedCarState = buf.ReadByte();
                buf.ReadByte(); // align
                buf.ReadByte(); // align
                buf.ReadByte(); // align
                m_targetCarPointer = buf.ReadInt32();
                m_storedCar = buf.Read<StoredCar>();
                m_initialized = buf.ReadBool();
                buf.ReadByte(); // align
                buf.ReadByte(); // align
                buf.ReadByte(); // align
                m_multiplayerCar = buf.ReadInt32();
                m_scriptGarageAcceptedCar = buf.ReadBool();
                m_locked = buf.ReadBool();
                m_scriptGarageState = buf.ReadByte();
                m_scriptGarageStateChanging = buf.ReadBool();
                m_unknown3 = buf.ReadInt32();   // 0xD8
                m_unknown4 = buf.ReadInt32();   // 0xDC

                Debug.Assert(buf.Offset == 0xE0);
            }
            else if (fmt.IsMobile)
            {
                m_door1Pointer = buf.ReadInt64();
                m_door2Pointer = buf.ReadInt64();
                m_targetCarPointer = buf.ReadInt32();
                m_storedCar = buf.Read<StoredCar>();    // 0x18, unconfirmed, not referenced in game
                m_unknown1 = buf.ReadInt32();           // 0x44, unconfirmed, not referenced in game
                m_unknown2 = buf.ReadInt32();           // 0x48, unconfirmed, not referenced in game
                m_unknown3 = buf.ReadInt32();           // 0x4C, unconfirmed, not referenced in game
                m_scriptGaragePos = buf.Read<Vector3D>();
                m_scriptGarageAngle = buf.ReadFloat();
                m_targetModelIndex = buf.ReadInt32();
                m_corner1 = buf.Read<Vector2D>();
                m_infZ = buf.ReadFloat();
                m_dir1 = buf.Read<Vector2D>();
                m_dir2 = buf.Read<Vector2D>();
                m_supZ = buf.ReadFloat();
                m_unknown4 = buf.ReadInt32();   // 0x84
                m_dir1Len = buf.ReadFloat();
                m_dir2Len = buf.ReadFloat();
                m_infX = buf.ReadFloat();
                m_supX = buf.ReadFloat();
                m_infY = buf.ReadFloat();
                m_supY = buf.ReadFloat();
                m_timeCrusherCraneActivated = buf.ReadInt32();
                m_scriptTargetCar = buf.ReadInt32();
                m_doorPos = buf.ReadFloat();
                m_doorHeight = buf.ReadFloat();
                m_door1X = buf.ReadFloat();
                m_door1Y = buf.ReadFloat();
                m_door2X = buf.ReadFloat();
                m_door2Y = buf.ReadFloat();
                m_door1Z = buf.ReadFloat();
                m_door2Z = buf.ReadFloat();
                m_timeToStartAction = buf.ReadInt32();
                m_type = (GarageType) buf.ReadByte();
                m_state = (GarageState) buf.ReadByte();
                m_maxStoredCars = buf.ReadByte();
                m_unusedField = buf.ReadByte();
                m_closingWithoutTargetCar = buf.ReadBool();
                m_deactivated = buf.ReadBool();
                m_resprayHappened = buf.ReadBool();
                m_door1PoolIndex = buf.ReadByte();
                m_door2PoolIndex = buf.ReadByte();
                m_door1IsDummy = buf.ReadBool();
                m_door2IsDummy = buf.ReadBool();
                m_recreateDoorsOnNextRefresh = buf.ReadBool();
                m_rotatingDoor = buf.ReadBool();
                m_cameraFollowsPlayer = buf.ReadBool();
                m_collectedCarState = buf.ReadByte();
                m_initialized = buf.ReadBool();
                m_scriptGarageAcceptedCar = buf.ReadBool();
                m_locked = buf.ReadBool();
                m_scriptGarageState = buf.ReadByte();
                m_scriptGarageStateChanging = buf.ReadBool();
                buf.ReadInt32();    // align (8-byte alignment)

                Debug.Assert(buf.Offset == 0xE0);
            }

            Debug.Assert(buf.Offset == SizeOfType<Garage>(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            if (fmt.IsPS2)
            {
                buf.Write((byte) m_type);
                buf.Write((byte) m_state);
                buf.Write((byte) m_maxStoredCars);
                buf.Skip(1);    // align
                buf.Write(m_unusedField);
                buf.Write(m_closingWithoutTargetCar, 4);
                buf.Write(m_deactivated, 4);
                buf.Write(m_resprayHappened, 4);
                buf.Write(m_targetModelIndex);
                buf.Write((int) m_door1Pointer);
                buf.Write((int) m_door2Pointer);
                buf.Write((byte) m_door1PoolIndex);
                buf.Write((byte) m_door2PoolIndex);
                buf.Skip(2);    // align
                buf.Write(m_door1IsDummy, 4);
                buf.Write(m_door2IsDummy, 4);
                buf.Write(m_recreateDoorsOnNextRefresh, 4);
                buf.Write(m_rotatingDoor, 4);
                buf.Write(m_cameraFollowsPlayer, 4);
                buf.Write(m_corner1);
                buf.Write(m_infZ);
                buf.Write(m_dir1);
                buf.Write(m_dir2);
                buf.Write(m_supZ);
                buf.Write(m_scriptGaragePos);
                buf.Write(m_scriptGarageAngle);
                buf.Write(m_unknown1);
                buf.Write(m_unknown2);
                buf.Write(m_unknown3);
                buf.Write(m_dir1Len);
                buf.Write(m_dir2Len);
                buf.Write(m_infX);
                buf.Write(m_supX);
                buf.Write(m_infY);
                buf.Write(m_supY);
                buf.Write(m_timeCrusherCraneActivated);
                buf.Write(m_scriptTargetCar);
                buf.Write(m_doorPos);
                buf.Write(m_doorHeight);
                buf.Write(m_door1X);
                buf.Write(m_door1Y);
                buf.Write(m_door2X);
                buf.Write(m_door2Y);
                buf.Write(m_door1Z);
                buf.Write(m_door2Z);
                buf.Write(m_timeToStartAction);
                buf.Write(m_collectedCarState);
                buf.Write((int) m_targetCarPointer);
                buf.Write(m_storedCar);
                buf.Write(m_initialized, 4);
                buf.Write(m_scriptGarageAcceptedCar, 4);
                buf.Write(m_locked, 4);
                buf.Write(m_scriptGarageState);
                buf.Write(m_scriptGarageStateChanging, 4);

                Debug.Assert(buf.Offset == 0x100);
            }
            else if (fmt.IsPSP)
            {
                buf.Write((byte) m_type);
                buf.Write((byte) m_state);
                buf.Write((byte) m_maxStoredCars);
                buf.Write((byte) m_unusedField);
                buf.Write(m_closingWithoutTargetCar);
                buf.Write(m_deactivated);
                buf.Write(m_resprayHappened);
                buf.Skip(1);    // align
                buf.Write(m_targetModelIndex);
                buf.Write((int) m_door1Pointer);
                buf.Write((int) m_door2Pointer);
                buf.Write(m_door1PoolIndex);
                buf.Write(m_door2PoolIndex);
                buf.Write(m_door1IsDummy);
                buf.Write(m_door2IsDummy);
                buf.Write(m_recreateDoorsOnNextRefresh);
                buf.Write(m_rotatingDoor);
                buf.Write(m_cameraFollowsPlayer);
                buf.Skip(1);    // align
                buf.Write(m_corner1);
                buf.Write(m_infZ);
                buf.Write(m_dir1);
                buf.Write(m_dir2);
                buf.Write(m_supZ);
                buf.Write(m_scriptGaragePos);
                buf.Write(m_scriptGarageAngle);
                buf.Write(m_unknown1);
                buf.Write(m_unknown2);
                buf.Write(m_dir1Len);
                buf.Write(m_dir2Len);
                buf.Write(m_infX);
                buf.Write(m_supX);
                buf.Write(m_infY);
                buf.Write(m_supY);
                buf.Write(m_timeCrusherCraneActivated);
                buf.Write(m_scriptTargetCar);
                buf.Write(m_doorPos);
                buf.Write(m_doorHeight);
                buf.Write(m_door1X);
                buf.Write(m_door1Y);
                buf.Write(m_door2X);
                buf.Write(m_door2Y);
                buf.Write(m_door1Z);
                buf.Write(m_door2Z);
                buf.Write(m_timeToStartAction);
                buf.Write(m_collectedCarState);
                buf.Skip(3);    // align
                buf.Write((int) m_targetCarPointer);
                buf.Write(m_storedCar);
                buf.Write(m_initialized);
                buf.Skip(3);    // align
                buf.Write(m_multiplayerCar);
                buf.Write(m_scriptGarageAcceptedCar);
                buf.Write(m_locked);
                buf.Write(m_scriptGarageState);
                buf.Write(m_scriptGarageStateChanging);
                buf.Write(m_unknown3);
                buf.Write(m_unknown4);

                Debug.Assert(buf.Offset == 0xE0);
            }
            else if (fmt.IsMobile)
            {
                buf.Write(m_door1Pointer);
                buf.Write(m_door2Pointer);
                buf.Write(m_targetCarPointer);
                buf.Write(m_storedCar);
                buf.Write(m_unknown1);
                buf.Write(m_unknown2);
                buf.Write(m_unknown3);
                buf.Write(m_scriptGaragePos);
                buf.Write(m_scriptGarageAngle);
                buf.Write(m_targetModelIndex);
                buf.Write(m_corner1);
                buf.Write(m_infZ);
                buf.Write(m_dir1);
                buf.Write(m_dir2);
                buf.Write(m_supZ);
                buf.Write(m_unknown4);
                buf.Write(m_dir1Len);
                buf.Write(m_dir2Len);
                buf.Write(m_infX);
                buf.Write(m_supX);
                buf.Write(m_infY);
                buf.Write(m_supY);
                buf.Write(m_timeCrusherCraneActivated);
                buf.Write(m_scriptTargetCar);
                buf.Write(m_doorPos);
                buf.Write(m_doorHeight);
                buf.Write(m_door1X);
                buf.Write(m_door1Y);
                buf.Write(m_door2X);
                buf.Write(m_door2Y);
                buf.Write(m_door1Z);
                buf.Write(m_door2Z);
                buf.Write(m_timeToStartAction);
                buf.Write((byte) m_type);
                buf.Write((byte) m_state);
                buf.Write((byte) m_maxStoredCars);
                buf.Write((byte) m_unusedField);
                buf.Write(m_closingWithoutTargetCar);
                buf.Write(m_deactivated);
                buf.Write(m_resprayHappened);
                buf.Write(m_door1PoolIndex);
                buf.Write(m_door2PoolIndex);
                buf.Write(m_door1IsDummy);
                buf.Write(m_door2IsDummy);
                buf.Write(m_recreateDoorsOnNextRefresh);
                buf.Write(m_rotatingDoor);
                buf.Write(m_cameraFollowsPlayer);
                buf.Write(m_collectedCarState);
                buf.Write(m_initialized);
                buf.Write(m_scriptGarageAcceptedCar);
                buf.Write(m_locked);
                buf.Write(m_scriptGarageState);
                buf.Write(m_scriptGarageStateChanging);
                buf.Skip(4);    // align (8-byte alignment)

                Debug.Assert(buf.Offset == 0xE0);
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

            return m_type.Equals(other.m_type)
                && m_state.Equals(other.m_state)
                && m_maxStoredCars.Equals(other.m_maxStoredCars)
                && m_unusedField.Equals(other.m_unusedField)
                && m_closingWithoutTargetCar.Equals(other.m_closingWithoutTargetCar)
                && m_deactivated.Equals(other.m_deactivated)
                && m_resprayHappened.Equals(other.m_resprayHappened)
                && m_targetModelIndex.Equals(other.m_targetModelIndex)
                && m_door1Pointer.Equals(other.m_door1Pointer)
                && m_door2Pointer.Equals(other.m_door2Pointer)
                && m_door1PoolIndex.Equals(other.m_door1PoolIndex)
                && m_door2PoolIndex.Equals(other.m_door2PoolIndex)
                && m_door1IsDummy.Equals(other.m_door1IsDummy)
                && m_door2IsDummy.Equals(other.m_door2IsDummy)
                && m_recreateDoorsOnNextRefresh.Equals(other.m_recreateDoorsOnNextRefresh)
                && m_rotatingDoor.Equals(other.m_rotatingDoor)
                && m_cameraFollowsPlayer.Equals(other.m_cameraFollowsPlayer)
                && m_corner1.Equals(other.m_corner1)
                && m_infZ.Equals(other.m_infZ)
                && m_dir1.Equals(other.m_dir1)
                && m_dir2.Equals(other.m_dir2)
                && m_supZ.Equals(other.m_supZ)
                && m_scriptGaragePos.Equals(other.m_scriptGaragePos)
                && m_scriptGarageAngle.Equals(other.m_scriptGarageAngle)
                && m_unknown1.Equals(other.m_unknown1)
                && m_unknown2.Equals(other.m_unknown2)
                && m_unknown3.Equals(other.m_unknown3)
                && m_unknown4.Equals(other.m_unknown4)
                && m_dir1Len.Equals(other.m_dir1Len)
                && m_dir2Len.Equals(other.m_dir2Len)
                && m_infX.Equals(other.m_infX)
                && m_supX.Equals(other.m_supX)
                && m_infY.Equals(other.m_infY)
                && m_supY.Equals(other.m_supY)
                && m_timeCrusherCraneActivated.Equals(other.m_timeCrusherCraneActivated)
                && m_scriptTargetCar.Equals(other.m_scriptTargetCar)
                && m_doorPos.Equals(other.m_doorPos)
                && m_doorHeight.Equals(other.m_doorHeight)
                && m_door1X.Equals(other.m_door1X)
                && m_door1Y.Equals(other.m_door1Y)
                && m_door2X.Equals(other.m_door2X)
                && m_door2Y.Equals(other.m_door2Y)
                && m_door1Z.Equals(other.m_door1Z)
                && m_door2Z.Equals(other.m_door2Z)
                && m_timeToStartAction.Equals(other.m_timeToStartAction)
                && m_collectedCarState.Equals(other.m_collectedCarState)
                && m_targetCarPointer.Equals(other.m_targetCarPointer)
                && m_storedCar.Equals(other.m_storedCar)
                && m_initialized.Equals(other.m_initialized)
                && m_multiplayerCar.Equals(other.m_multiplayerCar)
                && m_scriptGarageAcceptedCar.Equals(other.m_scriptGarageAcceptedCar)
                && m_locked.Equals(other.m_locked)
                && m_scriptGarageState.Equals(other.m_scriptGarageState)
                && m_scriptGarageStateChanging.Equals(other.m_scriptGarageStateChanging);
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
        CrateGarage,
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
