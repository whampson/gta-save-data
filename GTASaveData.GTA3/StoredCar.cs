using System;
using System.Diagnostics;
using System.Numerics;
using GTASaveData.Types;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// A vehicle stored in a safehouse garage.
    /// </summary>
    public class StoredCar : SaveDataObject,
        IEquatable<StoredCar>, IDeepClonable<StoredCar>
    {
        private int m_model;
        private Vehicle m_veh;
        private StoredCarFlags m_flags;
        private byte m_color1;
        private byte m_color2;
        private RadioStation m_radio;
        private sbyte m_extra1;
        private sbyte m_extra2;
        private BombType m_bomb;

        /// <summary>
        /// The vehicle model index.
        /// </summary>
        public int Model
        {
            get { return m_model; }
            set { m_model = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The vehicle's position in the game world.
        /// </summary>
        public Vector3 Position
        {
            get { return m_veh.Position; }
            set { m_veh.Position = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The vehicle's heading.
        /// </summary>
        public float Heading
        {
            get { return (ToDeg(m_veh.Matrix.GetEulerAngles().Z) + 360) % 360; }
            set { m_veh.SetHeading(ToRad(value)); OnPropertyChanged(); }
        }

        /// <summary>
        /// The vehicle's special perks (Explosion Proof, Fire Proof, etc.).
        /// </summary>
        public StoredCarFlags Flags
        {
            get { return m_flags; }
            set { m_flags = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Primary color. See <c>carcols.dat</c>.
        /// </summary>
        public byte Color1
        {
            get { return m_color1; }
            set { m_color1 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Secondary color. See <c>carcols.dat</c>.
        /// </summary>
        public byte Color2
        {
            get { return m_color2; }
            set { m_color2 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The selected radio station.
        /// </summary>
        public RadioStation Radio
        {
            get { return m_radio; }
            set { m_radio = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Extra part #1.
        /// </summary>
        /// <remarks>
        /// Taxi light, hardtop roof, bed covering, livery, etc.
        /// Only valid on some vehicles.
        /// </remarks>
        public sbyte Extra1
        {
            get { return m_extra1; }
            set { m_extra1 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Extra part #2.
        /// </summary>
        /// <remarks>
        /// Taxi light, hardtop roof, bed covering, livery, etc.
        /// Only valid on some vehicles.
        /// </remarks>
        public sbyte Extra2
        {
            get { return m_extra2; }
            set { m_extra2 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The bomb fitted to this vehicle.
        /// </summary>
        public BombType Bomb
        {
            get { return m_bomb; }
            set { m_bomb = value; OnPropertyChanged(); }
        }

        public StoredCar()
        {
            m_veh = Vehicle.CreateDefault();
        }

        public StoredCar(StoredCar other) : this()
        {
            Model = other.Model;
            Position = other.Position;
            Heading = other.Heading;
            Flags = other.Flags;
            Color1 = other.Color1;
            Color2 = other.Color2;
            Radio = other.Radio;
            Extra1 = other.Extra1;
            Extra2 = other.Extra2;
            Bomb = other.Bomb;
        }

        /// <summary>
        /// Gets a value indicating whether this stored car is valid.
        /// </summary>
        public bool HasCar()
        {
            return Model != 0;
        }

        /// <summary>
        /// Invalidates this stored car.
        /// </summary>
        public void Clear()
        {
            Model = 0;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            Model = buf.ReadInt32();
            Vector3 position = buf.ReadStruct<Vector3>();
            Vector3 angle = buf.ReadStruct<Vector3>();
            m_veh.Matrix = new Matrix()
            {
                Position = position,
                Forward = angle,
                Right = new Vector3(angle.Y, -angle.X, 0),
                Up = new Vector3(0, 0, 1)
            };
            Flags = (StoredCarFlags) buf.ReadInt32();
            Color1 = buf.ReadByte();
            Color2 = buf.ReadByte();
            Radio = (RadioStation) buf.ReadSByte();
            Extra1 = buf.ReadSByte();
            Extra2 = buf.ReadSByte();
            Bomb = (BombType) buf.ReadSByte();
            buf.Skip(2);

            Debug.Assert(buf.Offset == SizeOf<StoredCar>(prm));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            buf.Write(Model);
            buf.Write(Position);
            buf.Write(m_veh.Matrix.Forward);
            buf.Write((int) Flags);
            buf.Write(Color1);
            buf.Write(Color2);
            buf.Write((sbyte) Radio);
            buf.Write(Extra1);
            buf.Write(Extra2);
            buf.Write((sbyte) Bomb);
            buf.Skip(2);

            Debug.Assert(buf.Offset == SizeOf<StoredCar>(prm));
        }

        protected override int GetSize(SerializationParams prm)
        {
            return sizeof(int)
                + SizeOf<Vector3>(prm)
                + SizeOf<Vector3>(prm)
                + sizeof(int)
                + 2 * sizeof(byte)
                + sizeof(sbyte)
                + 2 * sizeof(byte)
                + sizeof(sbyte)
                + 2;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StoredCar);
        }

        public bool Equals(StoredCar other)
        {
            if (other == null)
            {
                return false;
            }

            return Model.Equals(other.Model)
                && Position.Equals(other.Position)
                && Heading.Equals(other.Heading)
                && Flags.Equals(other.Flags)
                && Color1.Equals(other.Color1)
                && Color2.Equals(other.Color2)
                && Radio.Equals(other.Radio)
                && Extra1.Equals(other.Extra1)
                && Extra2.Equals(other.Extra2)
                && Bomb.Equals(other.Bomb);
        }

        public StoredCar DeepClone()
        {
            return new StoredCar(this);
        }

        private static float ToDeg(float rad) => (float) (rad * 180.0f / Math.PI);
        private static float ToRad(float deg) => (float) (deg * Math.PI / 180.0f);
    }
}
