using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestSaveData
    {
        public static SaveData Generate(SystemType system)
        {
            Faker faker = new Faker();

            Faker<SaveData> model = new Faker<SaveData>()
                .RuleFor(x => x.SimpleVars, f => TestHelper.Generate<SimpleVars, TestSimpleVars>(system))
                .RuleFor(x => x.Scripts, TestHelper.Generate<Scripts, TestScripts>());

            return model.Generate();
        }

        [Fact]
        public void SerializationPC()
        {
            Serialization(SystemType.PC);
        }

        [Fact]
        public void SerializationPS2()
        {
            Serialization(SystemType.PS2);
        }

        private void Serialization(SystemType system)
        {
            SaveData data0 = Generate(system);
            SaveData data1 = TestHelper.CreateSerializedCopy(data0, out byte[] _, system);

            Assert.Equal(data0, data1);
        }
    }
}
