using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestObjectPool : Base<ObjectPool>
    {
        public override ObjectPool GenerateTestObject(GTA3SaveParams p)
        {
            Faker<ObjectPool> model = new Faker<ObjectPool>()
                .RuleFor(x => x.Objects,
                    f => Generator.Array(f.Random.Int(1, 25), g => Generator.Generate<PhysicalObject, TestPhysicalObject, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t); ObjectPool x0 = GenerateTestObject(p);
            ObjectPool x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Objects, x1.Objects);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            ObjectPool x0 = GenerateTestObject(p);
            ObjectPool x1 = new ObjectPool(x0);

            Assert.Equal(x0, x1);
        }
    }
}
