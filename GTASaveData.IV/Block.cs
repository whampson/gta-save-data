using GTASaveData.Interfaces;
using System;
using System.Diagnostics;

namespace GTASaveData.IV
{
    public class Block : BufferedObject,
         IEquatable<Block>, IDeepClonable<Block>
    {
        public Block() { }
        public Block(Block other) : base(other) { }

        protected struct BlockHeader
        {
            public int BlockSize;
        }

        public ObservableArray<byte> Data
        {
            get { return WorkBuffer; }
            set { WorkBuffer = value; OnPropertyChanged(); }
        }

        protected BlockHeader ReadBlockHeader(DataBuffer buf)
        {
            string tag = buf.ReadString(5);
            Debug.Assert(tag == "BLOCK");

            BlockHeader head = new BlockHeader
            {
                BlockSize = buf.ReadInt32()
            };
            return head;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams p)
        {
            BlockHeader head = ReadBlockHeader(buf);
            WorkBuffer = buf.ReadBytes(head.BlockSize - 9);
        }

        protected override void WriteData(DataBuffer buf, SerializationParams p)
        {
            buf.Write("BLOCK", zeroTerminate: false);
            buf.Write(WorkBuffer.Count + 9);
            buf.Write(WorkBuffer);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Block);
        }

        public bool Equals(Block other)
        {
            throw new NotImplementedException();
        }

        public virtual Block DeepClone()
        {
            return new Block(this);
        }
    }
}
