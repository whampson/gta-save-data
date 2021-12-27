using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPathData : Base<PathData>
    {
        public override PathData GenerateTestObject(GTA3SaveParams p)
        {
            int nodeCount = 8 * (new Faker().Random.Int(1, 1000) / 8);       // must be multiple of 8
            Faker<PathNode> nodeModel = new Faker<PathNode>()
                .RuleFor(x => x.Disabled, f => f.Random.Bool())
                .RuleFor(x => x.BetweenLevels, f => f.Random.Bool());
              
            Faker<PathData> model = new Faker<PathData>()
                .RuleFor(x => x.PathNodes, f => Generator.Array(nodeCount, g => nodeModel.Generate()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PathData x0 = GenerateTestObject(p);
            byte[] data = Serializer.Write(x0, p);
            PathData x1 = PathData.Load(data);

            Assert.Equal(x0.PathNodes, x1.PathNodes);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PathData x0 = GenerateTestObject(p);
            PathData x1 = new PathData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
