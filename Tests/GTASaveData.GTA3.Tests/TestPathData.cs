using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPathData : Base<PathData>
    {
        public override PathData GenerateTestObject(SaveDataFormat format)
        {
            int nodeCount = 8 * (new Faker().Random.Int(1, 1000) / 8);       // must be multiple of 8
            Faker<PathNode> nodeModel = new Faker<PathNode>()
                .RuleFor(x => x.Disabled, f => f.Random.Bool())
                .RuleFor(x => x.BetweenLevels, f => f.Random.Bool());
              
            Faker<PathData> model = new Faker<PathData>()
                .RuleFor(x => x.PathNodes, f => Generator.Array(nodeCount, g => nodeModel.Generate()));

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            PathData x0 = GenerateTestObject();
            byte[] data = Serializer.Write(x0);
            PathData x1 = PathData.Load(data);

            Assert.Equal(x0.PathNodes, x1.PathNodes);

            Assert.Equal(x0, x1);
            Assert.Equal(Serializer.SizeOf(x0), data.Length);
        }
    }
}
