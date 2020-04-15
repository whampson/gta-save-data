using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x80)]
    public class Crane : SaveDataObject, IEquatable<Crane>
    {
        private uint m_pCraneEntity;
        private uint m_pHook;
        private int m_audioEntity;
        private float m_pickupX1;
        private float m_pickupX2;
        private float m_pickupY1;
        private float m_pickupY2;
        private Vector m_dropoffTarget;
        private float m_dropoffHeading;
        private float m_pickupAngle;
        private float m_dropoffAngle;
        private float m_pickupDistance;
        private float m_dropoffDistance;
        private float m_pickupHeight;
        private float m_dropoffHeight;
        private float m_hookAngle;
        private float m_hookOffset;
        private float m_hookHeight;
        private Vector m_hookInitialPosition;
        private Vector m_hookCurrentPosition;
        private Vector2D m_hookVelocity;
        private uint m_pVehiclePickedUp;
        private uint m_timeForNextCheck;
        private CraneStatus m_status;
        private CraneState m_state;
        private byte m_vehiclesCollected;
        private bool m_isCrusher;
        private bool m_isMilitaryCrane;
        private bool m_wasMilitaryCrane;
        private bool m_isTop;       // model is cranetopa or cranetopb

        public uint CraneEntityPointer
        {
            get { return m_pCraneEntity; }
            set { m_pCraneEntity = value; OnPropertyChanged(); }
        }

        public uint HookPointer
        {
            get { return m_pHook; }
            set { m_pHook = value; OnPropertyChanged(); }
        }

        public int AudioEntity
        {
            get { return m_audioEntity; }
            set { m_audioEntity = value; OnPropertyChanged(); }
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

        public Vector DropoffTarget
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

        public float HookOffset
        {
            get { return m_hookOffset; }
            set { m_hookOffset = value; OnPropertyChanged(); }
        }

        public float HookHeight
        {
            get { return m_hookHeight; }
            set { m_hookHeight = value; OnPropertyChanged(); }
        }

        public Vector HookInitialPosition
        {
            get { return m_hookInitialPosition; }
            set { m_hookInitialPosition = value; OnPropertyChanged(); }
        }

        public Vector HookCurrentPosition
        {
            get { return m_hookCurrentPosition; }
            set { m_hookCurrentPosition = value; OnPropertyChanged(); }
        }

        public Vector2D HookVelocity
        {
            get { return m_hookVelocity; }
            set { m_hookVelocity = value; OnPropertyChanged(); }
        }

        public uint VehiclePickedUpPointer
        {
            get { return m_pVehiclePickedUp; }
            set { m_pVehiclePickedUp = value; OnPropertyChanged(); }
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
        {
            DropoffTarget = new Vector();
            HookInitialPosition = new Vector();
            HookCurrentPosition = new Vector();
            HookVelocity = new Vector2D();
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            CraneEntityPointer = buf.ReadUInt32();
            HookPointer = buf.ReadUInt32();
            AudioEntity = buf.ReadInt32();
            PickupX1 = buf.ReadFloat();
            PickupX2 = buf.ReadFloat();
            PickupY1 = buf.ReadFloat();
            PickupY2 = buf.ReadFloat();
            DropoffTarget = buf.Read<Vector>();
            DropoffHeading = buf.ReadFloat();
            PickupAngle = buf.ReadFloat();
            DropoffAngle = buf.ReadFloat();
            PickupDistance = buf.ReadFloat();
            DropoffDistance = buf.ReadFloat();
            PickupHeight = buf.ReadFloat();
            DropoffHeight = buf.ReadFloat();
            HookAngle = buf.ReadFloat();
            HookOffset = buf.ReadFloat();
            HookHeight = buf.ReadFloat();
            HookInitialPosition = buf.Read<Vector>();
            HookCurrentPosition = buf.Read<Vector>();
            HookVelocity = buf.Read<Vector2D>();
            VehiclePickedUpPointer = buf.ReadUInt32();
            TimeForNextCheck = buf.ReadUInt32();
            Status = (CraneStatus) buf.ReadByte();
            State = (CraneState) buf.ReadByte();
            VehiclesCollected = buf.ReadByte();
            IsCrusher = buf.ReadBool();
            IsMilitaryCrane = buf.ReadBool();
            WasMilitaryCrane = buf.ReadBool();
            IsTop = buf.ReadBool();
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == SizeOf<Crane>());
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {

            buf.Write(CraneEntityPointer);
            buf.Write(HookPointer);
            buf.Write(AudioEntity);
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
            buf.Write(HookOffset);
            buf.Write(HookHeight);
            buf.Write(HookInitialPosition);
            buf.Write(HookCurrentPosition);
            buf.Write(HookVelocity);
            buf.Write(VehiclePickedUpPointer);
            buf.Write(TimeForNextCheck);
            buf.Write((byte) Status);
            buf.Write((byte) State);
            buf.Write(VehiclesCollected);
            buf.Write(IsCrusher);
            buf.Write(IsMilitaryCrane);
            buf.Write(WasMilitaryCrane);
            buf.Write(IsTop);
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == SizeOf<Crane>());
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

            return CraneEntityPointer.Equals(other.CraneEntityPointer)
                && HookPointer.Equals(other.HookPointer)
                && AudioEntity.Equals(other.AudioEntity)
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
                && HookOffset.Equals(other.HookOffset)
                && HookHeight.Equals(other.HookHeight)
                && HookInitialPosition.Equals(other.HookInitialPosition)
                && HookCurrentPosition.Equals(other.HookCurrentPosition)
                && HookVelocity.Equals(other.HookVelocity)
                && VehiclePickedUpPointer.Equals(other.VehiclePickedUpPointer)
                && TimeForNextCheck.Equals(other.TimeForNextCheck)
                && Status.Equals(other.Status)
                && State.Equals(other.State)
                && VehiclesCollected.Equals(other.VehiclesCollected)
                && IsCrusher.Equals(other.IsCrusher)
                && IsMilitaryCrane.Equals(other.IsMilitaryCrane)
                && WasMilitaryCrane.Equals(other.WasMilitaryCrane)
                && IsTop.Equals(other.IsTop);
        }
    }
}
