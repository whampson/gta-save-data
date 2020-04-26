using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestSaveDataObject
    {
        [Fact]
        public void SizeOfFromAttribute()
        {
            int size = Serializer.SizeOf<TestObject>();

            Assert.Equal(DummySize, size);
        }

        [Fact]
        public void SizeOfFromSerialization()
        {
            TestObject o = new TestObject();
            int size = Serializer.SizeOf(o);

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

            protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
            {
                m_field00h = buf.ReadInt32();
                m_field04h = buf.ReadFloat();
                m_field08h = buf.ReadBool(4);
            }

            protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
            {
                buf.Write(m_field00h);
                buf.Write(m_field04h);
                buf.Write(m_field08h, 4);
            }
        }
    }
}
