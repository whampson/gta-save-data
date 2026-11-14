using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.LCS.Tests
{
    public class TestPlayerInfo : Base<PlayerInfo>
    {
        public override PlayerInfo GenerateTestObject(LCSSaveParams p)
        {
            Faker<PlayerInfo> model = new Faker<PlayerInfo>()
                .RuleFor(x => x.Money, f => f.Random.Int())
                .RuleFor(x => x.WastedBustedState, f => f.Random.Byte())
                .RuleFor(x => x.WastedBustedTime, f => f.Random.UInt())
                .RuleFor(x => x.TrafficMultiplier, f => f.Random.Short())
                .RuleFor(x => x.RoadDensity, f => f.Random.Float())
                .RuleFor(x => x.CarDensity, f => p.FileType.IsMobile ? f.Random.Short() : (short) 0)
                .RuleFor(x => x.PackagesTotal, f => f.Random.Int())
                .RuleFor(x => x.UnknownData, f => p.FileType.IsPS2 ? f.Random.Bytes(77) : new byte[77])
                .RuleFor(x => x.PspUnknownData, f => p.FileType.IsPSP ? f.Random.Bytes(74) : new byte[74])
                .RuleFor(x => x.UnknownState, f => p.FileType.IsMobile ? f.Random.Byte() : (byte) 0)
                .RuleFor(x => x.UnknownFlag, f => p.FileType.IsMobile ? f.Random.Byte() : (byte) 0)
                .RuleFor(x => x.MoneyOnScreen, f => f.Random.Int())
                .RuleFor(x => x.PackagesCollected, f => f.Random.Int())
                .RuleFor(x => x.MaxHealth, f => f.Random.Byte())
                .RuleFor(x => x.MaxArmor, f => f.Random.Byte())
                .RuleFor(x => x.InfiniteSprint, f => f.Random.Bool())
                .RuleFor(x => x.FastReload, f => f.Random.Bool())
                .RuleFor(x => x.FireProof, f => f.Random.Bool())
                .RuleFor(x => x.GetOutOfJailFree, f => f.Random.Bool())
                .RuleFor(x => x.GetOutOfHospitalFree, f => f.Random.Bool())
                .RuleFor(x => x.PlayerName, f => p.FileType.IsMobile ? Generator.Words(f, PlayerInfo.MaxPlayerNameLength - 1) : "");

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(LCSSaveParams p)
        {
            PlayerInfo x0 = GenerateTestObject(p);
            PlayerInfo x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Money, x1.Money);
            Assert.Equal(x0.WastedBustedState, x1.WastedBustedState);
            Assert.Equal(x0.WastedBustedTime, x1.WastedBustedTime);
            Assert.Equal(x0.TrafficMultiplier, x1.TrafficMultiplier);
            Assert.Equal(x0.RoadDensity, x1.RoadDensity);
            Assert.Equal(x0.CarDensity, x1.CarDensity);
            Assert.Equal(x0.PackagesTotal, x1.PackagesTotal);
            Assert.Equal(x0.UnknownData, x1.UnknownData);
            Assert.Equal(x0.PspUnknownData, x1.PspUnknownData);
            Assert.Equal(x0.UnknownState, x1.UnknownState);
            Assert.Equal(x0.UnknownFlag, x1.UnknownFlag);
            Assert.Equal(x0.MoneyOnScreen, x1.MoneyOnScreen);
            Assert.Equal(x0.PackagesCollected, x1.PackagesCollected);
            Assert.Equal(x0.MaxHealth, x1.MaxHealth);
            Assert.Equal(x0.MaxArmor, x1.MaxArmor);
            Assert.Equal(x0.InfiniteSprint, x1.InfiniteSprint);
            Assert.Equal(x0.FastReload, x1.FastReload);
            Assert.Equal(x0.FireProof, x1.FireProof);
            Assert.Equal(x0.GetOutOfJailFree, x1.GetOutOfJailFree);
            Assert.Equal(x0.GetOutOfHospitalFree, x1.GetOutOfHospitalFree);
            Assert.Equal(x0.PlayerName, x1.PlayerName);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            PlayerInfo x0 = GenerateTestObject();
            PlayerInfo x1 = new PlayerInfo(x0);

            Assert.Equal(x0, x1);
        }
    }
}
