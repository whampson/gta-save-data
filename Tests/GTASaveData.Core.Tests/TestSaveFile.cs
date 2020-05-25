using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestSaveFile
    {
        [Theory]
        [InlineData(PaddingType.Pattern, new byte[] { 0 })]
        [InlineData(PaddingType.Random, null)]
        [InlineData(PaddingType.Pattern, new byte[] { 0xCA, 0xFE, 0xBA, 0xBE })]
        public void Padding(PaddingType mode, byte[] seq)
        {
            TestSave t = new TestSave() { Padding = mode, PaddingBytes = seq };
            byte[] data = t.GetPaddingBytes(100);

            switch (mode)
            {
                case PaddingType.Random:
                {
                    Assert.NotEqual(0, data.Sum(x => x));
                    break;
                }
                case PaddingType.Pattern:
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        Assert.Equal(seq[i % seq.Length], data[i]);
                    }
                    break;
                }
            }
        }

        private class TestSave : GTASaveFile
        {
            public override string Name
            {
                get => throw new NotImplementedException();
                set => throw new NotImplementedException();
            }

            public override DateTime TimeLastSaved
            {
                get => throw new NotImplementedException();
                set => throw new NotImplementedException();
            }

            public override IReadOnlyList<SaveDataObject> Blocks => throw new NotImplementedException();

            public byte[] GetPaddingBytes(int length)
            {
                return GenerateSpecialPadding(length);
            }

            protected override void LoadAllData(StreamBuffer buf)
            {
                throw new NotImplementedException();
            }

            protected override void SaveAllData(StreamBuffer buf)
            {
                throw new NotImplementedException();
            }

            protected override bool DetectFileFormat(byte[] data, out SaveDataFormat fmt)
            {
                throw new NotImplementedException();
            }

            protected override int GetSize(SaveDataFormat fmt)
            {
                throw new NotImplementedException();
            }
        }
    }
}
