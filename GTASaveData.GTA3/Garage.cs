using GTASaveData.Interfaces;
using System;
using System.Diagnostics;
using System.Numerics;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// A script-controlled garage.
    /// </summary>
    /// <remarks>
    /// There are a lot of useless fields in this class. Be sure to read the
    /// documentation.
    /// </remarks>
    public class Garage : SaveDataObject,
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
        private StoredCar m_storedCar; // not used by game

        /// <summary>
        /// The garage type.
        /// </summary>
        public GarageType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The current garage state.
        /// </summary>
        /// <remarks>
        /// Is almost always <see cref="GarageState.Opened"/> or <see cref="GarageState.FullyClosed"/>
        /// in a saved game.
        /// </remarks>
        public GarageState State
        {
            get { return m_state; }
            set { m_state = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// An unused field. Does nothing in-game.
        /// </summary>
        public bool Field02h
        {
            get { return m_field02h; }
            set { m_field02h = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Name is self-explanatory. Pretty much useless in a save file.
        /// </summary>
        public bool ClosingWithoutTargetCar
        {
            get { return m_closingWithoutTargetCar; }
            set { m_closingWithoutTargetCar = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets wheter this garage is deactivated.
        /// </summary>
        /// <remarks>
        /// Garages must be activated in order to be used.
        /// </remarks>
        public bool Deactivated
        {
            get { return m_deactivated; }
            set { m_deactivated = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Name is self-explanatory. Pretty much useless in a save file.
        /// </summary>
        public bool ResprayHappened
        {
            get { return m_resprayHappened; }
            set { m_resprayHappened = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the vehicle model that this garage should accept.
        /// </summary>
        /// <remarks>
        /// Only used for <see cref="GarageType.CollectSpecificCars"/>.
        /// </remarks>
        public int TargetModelIndex
        {
            get { return m_targetModelIndex; }
            set { m_targetModelIndex = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The pointer of the <c>CEntity</c> object used for door #1.
        /// </summary>
        /// <remarks>
        /// This value is zeroed when the game loads, making it useless in save files.
        /// </remarks>
        public uint Door1Pointer
        {
            get { return m_pDoor1; }
            set { m_pDoor1 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The pointer of the <c>CEntity</c> object used for door #2.
        /// </summary>
        /// <remarks>
        /// This value is zeroed when the game loads, making it useless in save files.
        /// </remarks>
        public uint Door2Pointer
        {
            get { return m_pDoor2; }
            set { m_pDoor2 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Pool index for the <c>CEntity</c> object for door #1.
        /// Pretty much useless in a save file.
        /// </summary>
        public byte Door1Handle
        {
            get { return m_door1Handle; }
            set { m_door1Handle = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Pool index for the <c>CEntity</c> object for door #1.
        /// Pretty much useless in a save file.
        /// </summary>
        public byte Door2Handle
        {
            get { return m_door2Handle; }
            set { m_door2Handle = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indicates whether the <c>CEntity</c> object for door #1 is stored
        /// in the object pool or dummy pool. Pretty much useless in a save file.
        /// </summary>
        public bool Door1IsDummy
        {
            get { return m_isDoor1Dummy; }
            set { m_isDoor1Dummy = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indicates whether the <c>CEntity</c> object for door #2 is stored
        /// in the object pool or dummy pool. Pretty much useless in a save file.
        /// </summary>
        public bool Door2IsDummy
        {
            get { return m_isDoor2Dummy; }
            set { m_isDoor2Dummy = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Useless field, set to <c>true</c> when the game loads.
        /// </summary>
        public bool RecreateDoorOnNextRefresh
        {
            get { return m_recreateDoorOnNextRefresh; }
            set { m_recreateDoorOnNextRefresh = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indicates whether this garage has a rotating door.
        /// </summary>
        /// <seealso cref="SetDoorToRotate(bool)"/>.
        public bool RotatingDoor
        {
            get { return m_rotatedDoor; }
            set { m_rotatedDoor = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indicates whether the camera should stay attached to the player
        /// when the player enters the garage.
        /// </summary>
        public bool CameraFollowsPlayer
        {
            get { return m_cameraFollowsPlayer; }
            set { m_cameraFollowsPlayer = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Minor X-coordinate of garage cube.
        /// </summary>
        /// <seealso cref="GetPositionMinor"/>
        public float X1
        {
            get { return m_x1; }
            set { m_x1 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Major X-coordinate of garage cube.
        /// </summary>
        /// <seealso cref="GetPositionMajor"/>
        public float X2
        {
            get { return m_x2; }
            set { m_x2 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Minor Y-coordinate of garage cube.
        /// </summary>
        /// <seealso cref="GetPositionMinor"/>
        public float Y1
        {
            get { return m_y1; }
            set { m_y1 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Major Y-coordinate of garage cube.
        /// </summary>
        /// <seealso cref="GetPositionMajor"/>
        public float Y2
        {
            get { return m_y2; }
            set { m_y2 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Minor Z-coordinate of garage cube.
        /// </summary>
        /// <seealso cref="GetPositionMinor"/>
        public float Z1
        {
            get { return m_z1; }
            set { m_z1 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Major Z-coordinate of garage cube.
        /// </summary>
        /// <seealso cref="GetPositionMajor"/>
        public float Z2
        {
            get { return m_z2; }
            set { m_z2 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Current door-open position.
        /// </summary>
        public float DoorPosition
        {
            get { return m_doorOpenOffset; }
            set { m_doorOpenOffset = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Maximum door-open height.
        /// </summary>
        public float DoorHeight
        {
            get { return m_doorOpenMax; }
            set { m_doorOpenMax = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Door #1 X-coordinate.
        /// </summary>
        public float Door1X
        {
            get { return m_door1X; }
            set { m_door1X = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Door #1 Y-coordinate.
        /// </summary>
        public float Door1Y
        {
            get { return m_door1Y; }
            set { m_door1Y = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Door #2 X-coordinate.
        /// </summary>
        public float Door2X
        {
            get { return m_door2X; }
            set { m_door2X = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Door #2 Y-coordinate.
        /// </summary>
        public float Door2Y
        {
            get { return m_door2Y; }
            set { m_door2Y = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Door #1 Z-coordinate.
        /// </summary>
        public float Door1Z
        {
            get { return m_door1Z; }
            set { m_door1Z = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Door #2 Z-coordinate.
        /// </summary>
        public float Door2Z
        {
            get { return m_door2Z; }
            set { m_door2Z = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Time at which to open garage after respray, bomb fitting, crusher, etc.
        /// Pretty much useless in a saved game.
        /// </summary>
        public uint Timer
        {
            get { return m_timeToStartAction; }
            set { m_timeToStartAction = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Unused bitmask for collected cars. Probably a scrapped feature or
        /// early import/export implementation. Utterly useless.
        /// </summary>
        public byte CollectedCarsState
        {
            get { return m_collectedCarsState; }
            set { m_collectedCarsState = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// <c>CVehicle</c> pointer for cars delivered to the garage.
        /// Useless in a saved game.
        /// </summary>
        public uint TargetCarPointer
        {
            get { return m_pTargetCar; }
            set { m_pTargetCar = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// An unused field. Does nothing in-game.
        /// </summary>
        public int Field96h
        {
            get { return m_field96h; }
            set { m_field96h = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// An unused field. Does nothing in-game.
        /// </summary>
        public StoredCar StoredCar
        {
            get { return m_storedCar; }
            set { m_storedCar = value; OnPropertyChanged(); }
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
            ClosingWithoutTargetCar = other.ClosingWithoutTargetCar;
            Deactivated = other.Deactivated;
            ResprayHappened = other.ResprayHappened;
            TargetModelIndex = other.TargetModelIndex;
            Door1Pointer = other.Door1Pointer;
            Door2Pointer = other.Door2Pointer;
            Door1Handle = other.Door1Handle;
            Door2Handle = other.Door2Handle;
            Door1IsDummy = other.Door1IsDummy;
            Door2IsDummy = other.Door2IsDummy;
            RecreateDoorOnNextRefresh = other.RecreateDoorOnNextRefresh;
            RotatingDoor = other.RotatingDoor;
            CameraFollowsPlayer = other.CameraFollowsPlayer;
            X1 = other.X1;
            X2 = other.X2;
            Y1 = other.Y1;
            Y2 = other.Y2;
            Z1 = other.Z1;
            Z2 = other.Z2;
            DoorPosition = other.DoorPosition;
            DoorHeight = other.DoorHeight;
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

        /// <summary>
        /// Gets a value indicating whether this garage is valid.
        /// </summary>
        public bool IsUsed()
        {
            return Type != GarageType.None;
        }

        /// <summary>
        /// Gets a value indicating whether this garage is currently closed.
        /// </summary>
        public bool IsClosed()
        {
            return State == GarageState.FullyClosed || State == GarageState.ClosedContainsCar;
        }

        /// <summary>
        /// Gets a value indicating whether this garage is currently open.
        /// </summary>
        public bool IsOpen()
        {
            return State == GarageState.Opened || State == GarageState.OpenedContainsCar;
        }

        /// <summary>
        /// Sets the door to rotate or move vertically.
        /// </summary>
        /// <remarks>
        /// Most garage doors in GTA3 do not rotate, and rotating doors seems to only work
        /// properly for west-facing garages. Setting the door to rotate may cause some weird
        /// door behavior. You've been warned!
        /// </remarks>
        public void SetDoorToRotate(bool rotate)
        {
            if (RotatingDoor == rotate)
            {
                return;
            }

            RotatingDoor = rotate;
            if (rotate)
            {
                DoorHeight /= 2.0f;
                DoorHeight -= 0.1f;
            }
            else
            {
                DoorHeight += 0.1f;
                DoorHeight *= 2.0f;
            }
        }

        /// <summary>
        /// Gets the X-coordinate of the center of the garage cube.
        /// </summary>
        public float GetCenterX()
        {
            return (X1 + X2) / 2.0f;
        }

        /// <summary>
        /// Gets the Y-coordinate of the center of the garage cube.
        /// </summary>
        public float GetCenterY()
        {
            return (Y1 + Y2) / 2.0f;
        }

        /// <summary>
        /// Gets the minor (min) corner position of the garage cube.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPositionMinor()
        {
            return new Vector3(X1, Y1, Z1);
        }

        /// <summary>
        /// Gets the major (max) corner position of the garage cube.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPositionMajor()
        {
            return new Vector3(X2, Y2, Z2);
        }

        /// <summary>
        /// Sets the garage cube position.
        /// </summary>
        public void SetPosition(Vector3 p1, Vector3 p2)
        {
            X1 = Math.Min(p1.X, p2.X);
            X2 = Math.Max(p1.X, p2.X);
            Y1 = Math.Min(p1.Y, p2.Y);
            Y2 = Math.Max(p1.Y, p2.Y);
            Z1 = Math.Min(p1.Z, p2.Z);
            Z2 = Math.Max(p1.Z, p2.Z);
        }

        /// <summary>
        /// Gets the position of door #1.
        /// </summary>
        public Vector3 GetDoor1Position()
        {
            return new Vector3(Door1X, Door1Y, Door1Z);
        }

        /// <summary>
        /// Gets the position of door #2.
        /// </summary>
        public Vector3 GetDoor2Position()
        {
            return new Vector3(Door2X, Door2Y, Door2Z);
        }

        /// <summary>
        /// Sets the position of door #1.
        /// </summary>
        public void SetDoor1Position(Vector3 p)
        {
            Door1X = p.X;
            Door1Y = p.Y;
            Door1Z = p.Z;
        }

        /// <summary>
        /// Sets the position of door #2.
        /// </summary>
        public void SetDoor2Position(Vector3 p)
        {
            Door2X = p.X;
            Door2Y = p.Y;
            Door2Z = p.Z;
        }

        /// <summary>
        /// Checks whether the given 3D point is within the garage cube.
        /// </summary>
        public bool IsPointWithinGarage(Vector3 p)
        {
            return IsUsed()
                && p.X > X1 && p.X < X2
                && p.Y > Y1 && p.Y < Y2
                && p.Z > Z1 && p.Z < Z2;
        }

        /// <summary>
        /// Checks whether the given sphere intersects the garage cube.
        /// </summary>
        public bool IsGarageWithinSphere(Vector3 p, float r)
        {
            float x = Math.Max(X1, Math.Min(p.X, X2));
            float y = Math.Max(Y1, Math.Min(p.Y, Y2));
            float z = Math.Max(Z1, Math.Min(p.Z, Z2));

            float dx = x - p.X;
            float dy = y - p.Y;
            float dz = z - p.Z;

            return (dx * dx + dy * dy + dz * dz) <= r * r;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            Type = (GarageType) buf.ReadByte();
            State = (GarageState) buf.ReadByte();
            Field02h = buf.ReadBool();
            ClosingWithoutTargetCar = buf.ReadBool();
            Deactivated = buf.ReadBool();
            ResprayHappened = buf.ReadBool();
            buf.Skip(2);
            TargetModelIndex = buf.ReadInt32();
            Door1Pointer = buf.ReadUInt32();
            Door2Pointer = buf.ReadUInt32();
            Door1Handle = buf.ReadByte();
            Door2Handle = buf.ReadByte();
            Door1IsDummy = buf.ReadBool();
            Door2IsDummy = buf.ReadBool();
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
            DoorPosition = buf.ReadFloat();
            DoorHeight = buf.ReadFloat();
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

            Debug.Assert(buf.Offset == SizeOf<Garage>(prm));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            buf.Write((byte) Type);
            buf.Write((byte) State);
            buf.Write(Field02h);
            buf.Write(ClosingWithoutTargetCar);
            buf.Write(Deactivated);
            buf.Write(ResprayHappened);
            buf.Skip(2);
            buf.Write(TargetModelIndex);
            buf.Write(Door1Pointer);
            buf.Write(Door2Pointer);
            buf.Write(Door1Handle);
            buf.Write(Door2Handle);
            buf.Write(Door1IsDummy);
            buf.Write(Door2IsDummy);
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
            buf.Write(DoorPosition);
            buf.Write(DoorHeight);
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

            Debug.Assert(buf.Offset == SizeOf<Garage>(prm));
        }

        protected override int GetSize(SerializationParams prm)
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
                && ClosingWithoutTargetCar.Equals(other.ClosingWithoutTargetCar)
                && Deactivated.Equals(other.Deactivated)
                && ResprayHappened.Equals(other.ResprayHappened)
                && TargetModelIndex.Equals(other.TargetModelIndex)
                && Door1Pointer.Equals(other.Door1Pointer)
                && Door2Pointer.Equals(other.Door2Pointer)
                && Door1Handle.Equals(other.Door1Handle)
                && Door2Handle.Equals(other.Door2Handle)
                && Door1IsDummy.Equals(other.Door1IsDummy)
                && Door2IsDummy.Equals(other.Door2IsDummy)
                && RecreateDoorOnNextRefresh.Equals(other.RecreateDoorOnNextRefresh)
                && RotatingDoor.Equals(other.RotatingDoor)
                && CameraFollowsPlayer.Equals(other.CameraFollowsPlayer)
                && X1.Equals(other.X1)
                && X2.Equals(other.X2)
                && Y1.Equals(other.Y1)
                && Y2.Equals(other.Y2)
                && Z1.Equals(other.Z1)
                && Z2.Equals(other.Z2)
                && DoorPosition.Equals(other.DoorPosition)
                && DoorHeight.Equals(other.DoorHeight)
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
}
