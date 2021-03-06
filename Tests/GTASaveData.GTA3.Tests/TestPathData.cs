﻿using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPathData : Base<PathData>
    {
        public override PathData GenerateTestObject(FileFormat format)
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
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            PathData x0 = GenerateTestObject(format);
            byte[] data = Serializer.Write(x0, format);
            PathData x1 = PathData.Load(data);

            Assert.Equal(x0.PathNodes, x1.PathNodes);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            PathData x0 = GenerateTestObject();
            PathData x1 = new PathData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
