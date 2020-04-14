using Bogus;
using System;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPathFind : Base<PathFind>
    {
        public override PathFind GenerateTestObject(SaveFileFormat format)
        {
            Faker<PathFind> model = new Faker<PathFind>()
                .RuleFor(x => x.PathNodes, f => Generator.CreateArray<PathNode>(f.Random.Int(0, 5000)));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            // Special test conditions:
            // nodes written will always be a multiple of 8,
            // so node lists may not be equal, and that is ok

            PathFind x0 = GenerateTestObject();
            PathFind x1 = CreateSerializedCopy(x0, out byte[] data);

            int count0 = x0.PathNodes.Count;
            int count1 = x1.PathNodes.Count;
            int min = Math.Min(count0, count1);

            var path0 = x0.PathNodes.Take(min);
            var path1 = x1.PathNodes.Take(min);

            Assert.True(count1 - count0 < 8);
            Assert.Equal(path0, path1);

            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
