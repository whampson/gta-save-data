using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPlayerInfo : Base<PlayerInfo>
    {
        public override PlayerInfo GenerateTestObject(SaveFileFormat format)
        {
            Faker<PlayerInfo> model = new Faker<PlayerInfo>()
                .RuleFor(x => x.Money, f => f.Random.Int())
                .RuleFor(x => x.WastedBustedState, f => f.PickRandom<WastedBustedState>())
                .RuleFor(x => x.WastedBustedTime, f => f.Random.UInt())
                .RuleFor(x => x.TrafficMultiplier, f => f.Random.Short())
                .RuleFor(x => x.RoadDensity, f => f.Random.Float())
                .RuleFor(x => x.MoneyOnScreen, f => f.Random.Int())
                .RuleFor(x => x.PackagesCollected, f => f.Random.Int())
                .RuleFor(x => x.PackagesTotal, f => f.Random.Int())
                .RuleFor(x => x.InfinteSprint, f => f.Random.Bool())
                .RuleFor(x => x.FastReload, f => f.Random.Bool())
                .RuleFor(x => x.GetOutOfJailFree, f => f.Random.Bool())
                .RuleFor(x => x.GetOutOfHospitalFree, f => f.Random.Bool())
                .RuleFor(x => x.PlayerName, f => Generator.RandomWords(f, PlayerInfo.Limits.PlayerNameLength - 1));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            PlayerInfo x0 = GenerateTestObject();
            PlayerInfo x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Money, x1.Money);
            Assert.Equal(x0.WastedBustedState, x1.WastedBustedState);
            Assert.Equal(x0.WastedBustedTime, x1.WastedBustedTime);
            Assert.Equal(x0.TrafficMultiplier, x1.TrafficMultiplier);
            Assert.Equal(x0.RoadDensity, x1.RoadDensity);
            Assert.Equal(x0.MoneyOnScreen, x1.MoneyOnScreen);
            Assert.Equal(x0.PackagesCollected, x1.PackagesCollected);
            Assert.Equal(x0.PackagesTotal, x1.PackagesTotal);
            Assert.Equal(x0.InfinteSprint, x1.InfinteSprint);
            Assert.Equal(x0.FastReload, x1.FastReload);
            Assert.Equal(x0.GetOutOfJailFree, x1.GetOutOfJailFree);
            Assert.Equal(x0.GetOutOfHospitalFree, x1.GetOutOfHospitalFree);
            Assert.Equal(x0.PlayerName, x1.PlayerName);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
