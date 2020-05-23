using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public abstract class Vehicle : SaveDataObject, IEquatable<Vehicle>
    {
        // TODO: private members

        // TODO: public properties

        public Vehicle()
        {
            // TODO:
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            // TODO:
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            // TODO
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vehicle);
        }

        public bool Equals(Vehicle other)
        {
            if (other == null)
            {
                return false;
            }

            return false;   // TODO
        }
    }
}
