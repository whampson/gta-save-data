using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class Crane : SaveDataObject,
        IEquatable<Crane>, IDeepClonable<Crane>
    {
        private uint m_handle;
        private uint m_hookHandle;
        private int m_audioHandle;
        private float m_pickupX1;
        private float m_pickupX2;
        private float m_pickupY1;
        private float m_pickupY2;
        private Vector3D m_dropoffTarget;
        private float m_dropoffHeading;
        private float m_pickupAngle;
        private float m_dropoffAngle;
        private float m_pickupDistance;
        private float m_dropoffDistance;
        private float m_pickupHeight;
        private float m_dropoffHeight;
        private float m_hookAngle;
        private float m_hookDistance;
        private float m_hookHeight;
        private Vector3D m_hookInitialPosition;
        private Vector3D m_hookCurrentPosition;
        private Vector2D m_hookVelocity;
        private uint m_vehiclePickedUpHandle;
        private uint m_timeForNextCheck;
        private CraneStatus m_status;
        private CraneState m_state;
        private byte m_vehiclesCollected;
        private bool m_isCrusher;
        private bool m_isMilitaryCrane;
        private bool m_wasMilitaryCrane;
        private bool m_isTop;       // model is cranetopa or cranetopb

        public uint Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        public uint HookHandle
        {
            get { return m_hookHandle; }
            set { m_hookHandle = value; OnPropertyChanged(); }
        }

        public int AudioHandle
        {
            get { return m_audioHandle; }
            set { m_audioHandle = value; OnPropertyChanged(); }
        }

        public float PickupX1
        {
            get { return m_pickupX1; }
            set { m_pickupX1 = value; OnPropertyChanged(); }
        }

        public float PickupX2
        {
            get { return m_pickupX2; }
            set { m_pickupX2 = value; OnPropertyChanged(); }
        }

        public float PickupY1
        {
            get { return m_pickupY1; }
            set { m_pickupY1 = value; OnPropertyChanged(); }
        }

        public float PickupY2
        {
            get { return m_pickupY2; }
            set { m_pickupY2 = value; OnPropertyChanged(); }
        }

        public Vector3D DropoffTarget
        {
            get { return m_dropoffTarget; }
            set { m_dropoffTarget = value; OnPropertyChanged(); }
        }

        public float DropoffHeading
        {
            get { return m_dropoffHeading; }
            set { m_dropoffHeading = value; OnPropertyChanged(); }
        }

        public float PickupAngle
        {
            get { return m_pickupAngle; }
            set { m_pickupAngle = value; OnPropertyChanged(); }
        }

        public float DropoffAngle
        {
            get { return m_dropoffAngle; }
            set { m_dropoffAngle = value; OnPropertyChanged(); }
        }

        public float PickupDistance
        {
            get { return m_pickupDistance; }
            set { m_pickupDistance = value; OnPropertyChanged(); }
        }

        public float DropoffDistance
        {
            get { return m_dropoffDistance; }
            set { m_dropoffDistance = value; OnPropertyChanged(); }
        }

        public float PickupHeight
        {
            get { return m_pickupHeight; }
            set { m_pickupHeight = value; OnPropertyChanged(); }
        }

        public float DropoffHeight
        {
            get { return m_dropoffHeight; }
            set { m_dropoffHeight = value; OnPropertyChanged(); }
        }

        public float HookAngle
        {
            get { return m_hookAngle; }
            set { m_hookAngle = value; OnPropertyChanged(); }
        }

        public float HookDistance
        {
            get { return m_hookDistance; }
            set { m_hookDistance = value; OnPropertyChanged(); }
        }

        public float HookHeight
        {
            get { return m_hookHeight; }
            set { m_hookHeight = value; OnPropertyChanged(); }
        }

        public Vector3D HookInitialPosition
        {
            get { return m_hookInitialPosition; }
            set { m_hookInitialPosition = value; OnPropertyChanged(); }
        }

        public Vector3D HookCurrentPosition
        {
            get { return m_hookCurrentPosition; }
            set { m_hookCurrentPosition = value; OnPropertyChanged(); }
        }

        public Vector2D HookVelocity
        {
            get { return m_hookVelocity; }
            set { m_hookVelocity = value; OnPropertyChanged(); }
        }

        public uint VehiclePickedUpHandle
        {
            get { return m_vehiclePickedUpHandle; }
            set { m_vehiclePickedUpHandle = value; OnPropertyChanged(); }
        }

        public uint TimeForNextCheck
        {
            get { return m_timeForNextCheck; }
            set { m_timeForNextCheck = value; OnPropertyChanged(); }
        }

        public CraneStatus Status
        {
            get { return m_status; }
            set { m_status = value; OnPropertyChanged(); }
        }

        public CraneState State
        {
            get { return m_state; }
            set { m_state = value; OnPropertyChanged(); }
        }

        public byte VehiclesCollected
        {
            get { return m_vehiclesCollected; }
            set { m_vehiclesCollected = value; OnPropertyChanged(); }
        }

        public bool IsCrusher
        {
            get { return m_isCrusher; }
            set { m_isCrusher = value; OnPropertyChanged(); }
        }

        public bool IsMilitaryCrane
        {
            get { return m_isMilitaryCrane; }
            set { m_isMilitaryCrane = value; OnPropertyChanged(); }
        }

        public bool WasMilitaryCrane
        {
            get { return m_wasMilitaryCrane; }
            set { m_wasMilitaryCrane = value; OnPropertyChanged(); }
        }

        public bool IsTop
        {
            get { return m_isTop; }
            set { m_isTop = value; OnPropertyChanged(); }
        }

        public Crane()
        { }

        public Crane(Crane other)
        {
            Handle = other.Handle;
            HookHandle = other.HookHandle;
            AudioHandle = other.AudioHandle;
            PickupX1 = other.PickupX1;
            PickupX2 = other.PickupX2;
            PickupY1 = other.PickupY1;
            PickupY2 = other.PickupY2;
            DropoffTarget = other.DropoffTarget;
            DropoffHeading = other.DropoffHeading;
            PickupAngle = other.PickupAngle;
            DropoffAngle = other.DropoffAngle;
            PickupDistance = other.PickupDistance;
            DropoffDistance = other.DropoffDistance;
            PickupHeight = other.PickupHeight;
            DropoffHeight = other.DropoffHeight;
            HookAngle = other.HookAngle;
            HookDistance = other.HookDistance;
            HookHeight = other.HookHeight;
            HookInitialPosition = other.HookInitialPosition;
            HookCurrentPosition = other.HookCurrentPosition;
            HookVelocity = other.HookVelocity;
            VehiclePickedUpHandle = other.VehiclePickedUpHandle;
            TimeForNextCheck = other.TimeForNextCheck;
            Status = other.Status;
            State = other.State;
            VehiclesCollected = other.VehiclesCollected;
            IsCrusher = other.IsCrusher;
            IsMilitaryCrane = other.IsMilitaryCrane;
            WasMilitaryCrane = other.WasMilitaryCrane;
            IsTop = other.IsTop;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Handle = buf.ReadUInt32();
            HookHandle = buf.ReadUInt32();
            AudioHandle = buf.ReadInt32();
            PickupX1 = buf.ReadFloat();
            PickupX2 = buf.ReadFloat();
            PickupY1 = buf.ReadFloat();
            PickupY2 = buf.ReadFloat();
            DropoffTarget = buf.Read<Vector3D>();
            DropoffHeading = buf.ReadFloat();
            PickupAngle = buf.ReadFloat();
            DropoffAngle = buf.ReadFloat();
            PickupDistance = buf.ReadFloat();
            DropoffDistance = buf.ReadFloat();
            PickupHeight = buf.ReadFloat();
            DropoffHeight = buf.ReadFloat();
            HookAngle = buf.ReadFloat();
            HookDistance = buf.ReadFloat();
            HookHeight = buf.ReadFloat();
            HookInitialPosition = buf.Read<Vector3D>();
            HookCurrentPosition = buf.Read<Vector3D>();
            HookVelocity = buf.Read<Vector2D>();
            VehiclePickedUpHandle = buf.ReadUInt32();
            TimeForNextCheck = buf.ReadUInt32();
            Status = (CraneStatus) buf.ReadByte();
            State = (CraneState) buf.ReadByte();
            VehiclesCollected = buf.ReadByte();
            IsCrusher = buf.ReadBool();
            IsMilitaryCrane = buf.ReadBool();
            WasMilitaryCrane = buf.ReadBool();
            IsTop = buf.ReadBool();
            buf.Skip(1);

            Debug.Assert(buf.Offset == SizeOfType<Crane>());
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(Handle);
            buf.Write(HookHandle);
            buf.Write(AudioHandle);
            buf.Write(PickupX1);
            buf.Write(PickupX2);
            buf.Write(PickupY1);
            buf.Write(PickupY2);
            buf.Write(DropoffTarget);
            buf.Write(DropoffHeading);
            buf.Write(PickupAngle);
            buf.Write(DropoffAngle);
            buf.Write(PickupDistance);
            buf.Write(DropoffDistance);
            buf.Write(PickupHeight);
            buf.Write(DropoffHeight);
            buf.Write(HookAngle);
            buf.Write(HookDistance);
            buf.Write(HookHeight);
            buf.Write(HookInitialPosition);
            buf.Write(HookCurrentPosition);
            buf.Write(HookVelocity);
            buf.Write(VehiclePickedUpHandle);
            buf.Write(TimeForNextCheck);
            buf.Write((byte) Status);
            buf.Write((byte) State);
            buf.Write(VehiclesCollected);
            buf.Write(IsCrusher);
            buf.Write(IsMilitaryCrane);
            buf.Write(WasMilitaryCrane);
            buf.Write(IsTop);
            buf.Skip(1);

            Debug.Assert(buf.Offset == SizeOfType<Crane>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x80;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Crane);
        }

        public bool Equals(Crane other)
        {
            if (other == null)
            {
                return false;
            }

            return Handle.Equals(other.Handle)
                && HookHandle.Equals(other.HookHandle)
                && AudioHandle.Equals(other.AudioHandle)
                && PickupX1.Equals(other.PickupX1)
                && PickupX2.Equals(other.PickupX2)
                && PickupY1.Equals(other.PickupY1)
                && PickupY2.Equals(other.PickupY2)
                && DropoffTarget.Equals(other.DropoffTarget)
                && DropoffHeading.Equals(other.DropoffHeading)
                && PickupAngle.Equals(other.PickupAngle)
                && DropoffAngle.Equals(other.DropoffAngle)
                && PickupDistance.Equals(other.PickupDistance)
                && DropoffDistance.Equals(other.DropoffDistance)
                && PickupHeight.Equals(other.PickupHeight)
                && DropoffHeight.Equals(other.DropoffHeight)
                && HookAngle.Equals(other.HookAngle)
                && HookDistance.Equals(other.HookDistance)
                && HookHeight.Equals(other.HookHeight)
                && HookInitialPosition.Equals(other.HookInitialPosition)
                && HookCurrentPosition.Equals(other.HookCurrentPosition)
                && HookVelocity.Equals(other.HookVelocity)
                && VehiclePickedUpHandle.Equals(other.VehiclePickedUpHandle)
                && TimeForNextCheck.Equals(other.TimeForNextCheck)
                && Status.Equals(other.Status)
                && State.Equals(other.State)
                && VehiclesCollected.Equals(other.VehiclesCollected)
                && IsCrusher.Equals(other.IsCrusher)
                && IsMilitaryCrane.Equals(other.IsMilitaryCrane)
                && WasMilitaryCrane.Equals(other.WasMilitaryCrane)
                && IsTop.Equals(other.IsTop);
        }

        public Crane DeepClone()
        {
            return new Crane(this);
        }
    }
}
