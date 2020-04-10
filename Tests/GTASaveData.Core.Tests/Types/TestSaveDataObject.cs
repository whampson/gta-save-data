using GTASaveData.Types;
using Xunit;

namespace GTASaveData.Core.Tests.Types
{
    public class TestSaveDataObject
    {
        [Fact]
        public void TestSizeOf_Attribute()
        {
            int size = SaveDataObject.SizeOf<TestObject>();

            Assert.Equal(DummySize, size);
        }

        [Fact]
        public void TestSizeOf_Serialization()
        {
            TestObject o = new TestObject();
            int size = SaveDataObject.SizeOf(o);

            Assert.Equal(ActualSize, size);
        }

        const int DummySize = 8;
        const int ActualSize = 12;

        [Size(DummySize)]       // Intentionally wrong
        private class TestObject : SaveDataObject
        {
            private int m_field00h;
            private float m_field04h;
            private bool m_field08h;

            protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
            {
                m_field00h = buf.ReadInt32();
                m_field04h = buf.ReadFloat();
                m_field08h = buf.ReadBool(4);
            }

            protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
            {
                buf.Write(m_field00h);
                buf.Write(m_field04h);
                buf.Write(m_field08h, 4);
            }
        }
    }
}
