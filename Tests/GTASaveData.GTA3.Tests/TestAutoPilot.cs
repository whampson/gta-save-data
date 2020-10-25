using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestAutoPilot : Base<AutoPilot>
    {
        public override AutoPilot GenerateTestObject(FileFormat format)
        {
            Faker<AutoPilot> model = new Faker<AutoPilot>()
                .RuleFor(x => x.CurrRouteNode, f => f.Random.Int())
                .RuleFor(x => x.NextRouteNode, f => f.Random.Int())
                .RuleFor(x => x.PrevRouteNode, f => f.Random.Int())
                .RuleFor(x => x.TimeEnteredCurve, f => f.Random.UInt())
                .RuleFor(x => x.TimeToSpendOnCurrentCurve, f => f.Random.UInt())
                .RuleFor(x => x.CurrPathNodeInfo, f => f.Random.Int())
                .RuleFor(x => x.NextPathNodeInfo, f => f.Random.Int())
                .RuleFor(x => x.PrevPathNodeInfo, f => f.Random.Int())
                .RuleFor(x => x.AntiReverseTimer, f => f.Random.UInt())
                .RuleFor(x => x.TimeToStartMission, f => f.Random.UInt())
                .RuleFor(x => x.PrevDirection, f => f.Random.Byte())
                .RuleFor(x => x.CurrDirection, f => f.Random.Byte())
                .RuleFor(x => x.NextDirection, f => f.Random.Byte())
                .RuleFor(x => x.CurrLane, f => f.Random.Byte())
                .RuleFor(x => x.NextLane, f => f.Random.Byte())
                .RuleFor(x => x.DrivingStyle, f => f.PickRandom<CarDrivingStyle>())
                .RuleFor(x => x.Mission, f => f.PickRandom<CarMission>())
                .RuleFor(x => x.TempAction, f => f.PickRandom<CarTempAction>())
                .RuleFor(x => x.TimeTempAction, f => f.Random.UInt())
                .RuleFor(x => x.MaxTrafficSpeed, f => f.Random.Float())
                .RuleFor(x => x.CruiseSpeed, f => f.Random.Byte())
                .RuleFor(x => x.SlowedDownByCars, f => f.Random.Bool())
                .RuleFor(x => x.SlowedDownByPeds, f => f.Random.Bool())
                .RuleFor(x => x.StayInCurrentLevel, f => f.Random.Bool())
                .RuleFor(x => x.StayInFastLane, f => f.Random.Bool())
                .RuleFor(x => x.IgnorePathFinding, f => f.Random.Bool())
                .RuleFor(x => x.Destination, f => Generator.Vector3(f))
                .RuleFor(x => x.PathFindNodesCount, f => f.Random.Short());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            AutoPilot x0 = GenerateTestObject(format);
            AutoPilot x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.CurrRouteNode, x1.CurrRouteNode);
            Assert.Equal(x0.NextRouteNode, x1.NextRouteNode);
            Assert.Equal(x0.PrevRouteNode, x1.PrevRouteNode);
            Assert.Equal(x0.TimeEnteredCurve, x1.TimeEnteredCurve);
            Assert.Equal(x0.TimeToSpendOnCurrentCurve, x1.TimeToSpendOnCurrentCurve);
            Assert.Equal(x0.CurrPathNodeInfo, x1.CurrPathNodeInfo);
            Assert.Equal(x0.NextPathNodeInfo, x1.NextPathNodeInfo);
            Assert.Equal(x0.PrevPathNodeInfo, x1.PrevPathNodeInfo);
            Assert.Equal(x0.AntiReverseTimer, x1.AntiReverseTimer);
            Assert.Equal(x0.TimeToStartMission, x1.TimeToStartMission);
            Assert.Equal(x0.PrevDirection, x1.PrevDirection);
            Assert.Equal(x0.CurrDirection, x1.CurrDirection);
            Assert.Equal(x0.NextDirection, x1.NextDirection);
            Assert.Equal(x0.CurrLane, x1.CurrLane);
            Assert.Equal(x0.NextLane, x1.NextLane);
            Assert.Equal(x0.DrivingStyle, x1.DrivingStyle);
            Assert.Equal(x0.Mission, x1.Mission);
            Assert.Equal(x0.TempAction, x1.TempAction);
            Assert.Equal(x0.TimeTempAction, x1.TimeTempAction);
            Assert.Equal(x0.MaxTrafficSpeed, x1.MaxTrafficSpeed);
            Assert.Equal(x0.CruiseSpeed, x1.CruiseSpeed);
            Assert.Equal(x0.SlowedDownByCars, x1.SlowedDownByCars);
            Assert.Equal(x0.SlowedDownByPeds, x1.SlowedDownByPeds);
            Assert.Equal(x0.StayInCurrentLevel, x1.StayInCurrentLevel);
            Assert.Equal(x0.StayInFastLane, x1.StayInFastLane);
            Assert.Equal(x0.IgnorePathFinding, x1.IgnorePathFinding);
            Assert.Equal(x0.Destination, x1.Destination);
            Assert.Equal(x0.PathFindNodesCount, x1.PathFindNodesCount);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            AutoPilot x0 = GenerateTestObject();
            AutoPilot x1 = new AutoPilot(x0);

            Assert.Equal(x0, x1);
        }
    }
}
