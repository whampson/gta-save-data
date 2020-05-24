using Bogus;
using GTASaveData.Core.Tests.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestBoat : Base<Boat>
    {
        public override Boat GenerateTestObject(DataFormat format)
        {
            Faker<Boat> model = new Faker<Boat>()
                .RuleFor(x => x.ModelIndex, f => f.Random.Short())
                .RuleFor(x => x.Handle, f => f.Random.Int())
                .RuleFor(x => x.Matrix, f => TestMatrix.GenerateRandom(f))
                .RuleFor(x => x.EntityType, f => f.PickRandom<EntityType>())
                .RuleFor(x => x.EntityStatus, f => f.PickRandom<EntityStatus>())
                .RuleFor(x => x.EntityFlags, f => f.PickRandom<EntityFlags>())
                .RuleFor(x => x.AutoPilot, f => Generator.Generate<AutoPilot, TestAutoPilot>())
                .RuleFor(x => x.Color1, f => f.Random.Byte())
                .RuleFor(x => x.Color2, f => f.Random.Byte())
                .RuleFor(x => x.AlarmState, f => f.Random.Short())
                .RuleFor(x => x.MaxNumPassengers, f => f.Random.Byte())
                .RuleFor(x => x.Field1D0h, f => f.Random.Float())
                .RuleFor(x => x.Field1D4h, f => f.Random.Float())
                .RuleFor(x => x.Field1D8h, f => f.Random.Float())
                .RuleFor(x => x.Field1DCh, f => f.Random.Float())
                .RuleFor(x => x.SteerAngle, f => f.Random.Float())
                .RuleFor(x => x.GasPedal, f => f.Random.Float())
                .RuleFor(x => x.BrakePedal, f => f.Random.Float())
                .RuleFor(x => x.VehicleCreatedBy, f => f.Random.Byte())
                .RuleFor(x => x.IsLawEnforcer, f => f.Random.Bool())
                .RuleFor(x => x.IsLocked, f => f.Random.Bool())
                .RuleFor(x => x.IsEngineOn, f => f.Random.Bool())
                .RuleFor(x => x.IsHandbrakeOn, f => f.Random.Bool())
                .RuleFor(x => x.LightsOn, f => f.Random.Bool())
                .RuleFor(x => x.HasFreebies, f => f.Random.Bool())
                .RuleFor(x => x.Health, f => f.Random.Float())
                .RuleFor(x => x.CurrentGear, f => f.Random.Byte())
                .RuleFor(x => x.ChangeGearTime, f => f.Random.Float())
                .RuleFor(x => x.TimeOfDeath, f => f.Random.UInt())
                .RuleFor(x => x.BombTimer, f => f.Random.Short())
                .RuleFor(x => x.DoorLock, f => f.PickRandom<CarLock>());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(DataFormat format)
        {
            Boat x0 = GenerateTestObject(format);
            Boat x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(VehicleType.Boat, x1.Type);
            Assert.Equal(x0.Matrix, x1.Matrix);
            Assert.Equal(x0.EntityType, x1.EntityType);
            Assert.Equal(x0.EntityStatus, x1.EntityStatus);
            Assert.Equal(x0.EntityFlags, x1.EntityFlags);
            Assert.Equal(x0.AutoPilot, x1.AutoPilot);
            Assert.Equal(x0.Color1, x1.Color1);
            Assert.Equal(x0.Color2, x1.Color2);
            Assert.Equal(x0.AlarmState, x1.AlarmState);
            Assert.Equal(x0.MaxNumPassengers, x1.MaxNumPassengers);
            Assert.Equal(x0.Field1D0h, x1.Field1D0h);
            Assert.Equal(x0.Field1D4h, x1.Field1D4h);
            Assert.Equal(x0.Field1D8h, x1.Field1D8h);
            Assert.Equal(x0.Field1DCh, x1.Field1DCh);
            Assert.Equal(x0.SteerAngle, x1.SteerAngle);
            Assert.Equal(x0.GasPedal, x1.GasPedal);
            Assert.Equal(x0.BrakePedal, x1.BrakePedal);
            Assert.Equal(x0.VehicleCreatedBy, x1.VehicleCreatedBy);
            Assert.Equal(x0.IsLawEnforcer, x1.IsLawEnforcer);
            Assert.Equal(x0.IsLocked, x1.IsLocked);
            Assert.Equal(x0.IsEngineOn, x1.IsEngineOn);
            Assert.Equal(x0.IsHandbrakeOn, x1.IsHandbrakeOn);
            Assert.Equal(x0.LightsOn, x1.LightsOn);
            Assert.Equal(x0.HasFreebies, x1.HasFreebies);
            Assert.Equal(x0.Health, x1.Health);
            Assert.Equal(x0.CurrentGear, x1.CurrentGear);
            Assert.Equal(x0.ChangeGearTime, x1.ChangeGearTime);
            Assert.Equal(x0.TimeOfDeath, x1.TimeOfDeath);
            Assert.Equal(x0.BombTimer, x1.BombTimer);
            Assert.Equal(x0.DoorLock, x1.DoorLock);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(format), data.Length);
        }
    }
}