using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPlayerInfo : Base<PlayerInfo>
    {
        public override PlayerInfo GenerateTestObject(GTA3SaveParams p)
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
                .RuleFor(x => x.InfiniteSprint, f => f.Random.Bool())
                .RuleFor(x => x.FastReload, f => f.Random.Bool())
                .RuleFor(x => x.GetOutOfJailFree, f => f.Random.Bool())
                .RuleFor(x => x.GetOutOfHospitalFree, f => f.Random.Bool())
                .RuleFor(x => x.PlayerName, f => Generator.Words(f, PlayerInfo.MaxPlayerNameLength - 1));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PlayerInfo x0 = GenerateTestObject(p);
            PlayerInfo x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Money, x1.Money);
            Assert.Equal(x0.WastedBustedState, x1.WastedBustedState);
            Assert.Equal(x0.WastedBustedTime, x1.WastedBustedTime);
            Assert.Equal(x0.TrafficMultiplier, x1.TrafficMultiplier);
            Assert.Equal(x0.RoadDensity, x1.RoadDensity);
            Assert.Equal(x0.MoneyOnScreen, x1.MoneyOnScreen);
            Assert.Equal(x0.PackagesCollected, x1.PackagesCollected);
            Assert.Equal(x0.PackagesTotal, x1.PackagesTotal);
            Assert.Equal(x0.InfiniteSprint, x1.InfiniteSprint);
            Assert.Equal(x0.FastReload, x1.FastReload);
            Assert.Equal(x0.GetOutOfJailFree, x1.GetOutOfJailFree);
            Assert.Equal(x0.GetOutOfHospitalFree, x1.GetOutOfHospitalFree);
            Assert.Equal(x0.PlayerName, x1.PlayerName);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PlayerInfo x0 = GenerateTestObject(p);
            PlayerInfo x1 = new PlayerInfo(x0);

            Assert.Equal(x0, x1);
        }
    }
}
