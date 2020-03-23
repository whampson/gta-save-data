using System;
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
            byte[] data;

            using (TestSave t = new TestSave() { Padding = mode, PaddingBytes = seq })
            {
                data = t.GetPaddingBytes(100);
            }

            switch (mode)
            {
                case PaddingType.Random:
                    Assert.NotEqual(0, data.Sum(x => x));
                    break;
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

        private class TestSave : SaveFile
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

            protected override bool DetectFileFormat(byte[] data, out SaveFileFormat fmt)
            {
                throw new NotImplementedException();
            }

            protected override int ReadBlock(WorkBuffer file)
            {
                throw new NotImplementedException();
            }

            protected override int WriteBlock(WorkBuffer buf)
            {
                throw new NotImplementedException();
            }

            protected override void LoadAllData(WorkBuffer buf)
            {
                throw new NotImplementedException();
            }

            protected override void SaveAllData(WorkBuffer buf)
            {
                throw new NotImplementedException();
            }

            public new byte[] GetPaddingBytes(int length)
            {
                return base.GetPaddingBytes(length);
            }
        }
    }
}
