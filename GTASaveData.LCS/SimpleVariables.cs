using System;
using System.Collections.Generic;
using System.Text;

namespace GTASaveData.LCS
{
    public class SimpleVariables : SaveDataObject,
        IEquatable<SimpleVariables>, IDeepClonable<SimpleVariables>
    {
        public SimpleVariables DeepClone()
        {
            throw new NotImplementedException();
        }

        public bool Equals(SimpleVariables other)
        {
            throw new NotImplementedException();
        }

        protected override int GetSize(FileFormat fmt)
        {
            throw new NotImplementedException();
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            throw new NotImplementedException();
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            throw new NotImplementedException();
        }
    }
}
