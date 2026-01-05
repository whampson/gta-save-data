using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGarage : Base<Garage>
    {
        public override Garage GenerateTestObject(GTA3SaveParams p)
        {
            Faker<Garage> model = new Faker<Garage>()
                .RuleFor(x => x.Type, f => f.PickRandom<GarageType>())
                .RuleFor(x => x.State, f => f.PickRandom<GarageState>())
                .RuleFor(x => x.Field02h, f => f.Random.Bool())
                .RuleFor(x => x.ClosingWithoutTargetCar, f => f.Random.Bool())
                .RuleFor(x => x.Deactivated, f => f.Random.Bool())
                .RuleFor(x => x.ResprayHappened, f => f.Random.Bool())
                .RuleFor(x => x.TargetModelIndex, f => f.Random.Int())
                .RuleFor(x => x.Door1Pointer, f => f.Random.UInt())
                .RuleFor(x => x.Door2Pointer, f => f.Random.UInt())
                .RuleFor(x => x.Door1Handle, f => f.Random.Byte())
                .RuleFor(x => x.Door2Handle, f => f.Random.Byte())
                .RuleFor(x => x.Door1IsDummy, f => f.Random.Bool())
                .RuleFor(x => x.Door2IsDummy, f => f.Random.Bool())
                .RuleFor(x => x.RecreateDoorOnNextRefresh, f => f.Random.Bool())
                .RuleFor(x => x.RotatingDoor, f => f.Random.Bool())
                .RuleFor(x => x.CameraFollowsPlayer, f => f.Random.Bool())
                .RuleFor(x => x.X1, f => f.Random.Float())
                .RuleFor(x => x.X2, f => f.Random.Float())
                .RuleFor(x => x.Y1, f => f.Random.Float())
                .RuleFor(x => x.Y2, f => f.Random.Float())
                .RuleFor(x => x.Z1, f => f.Random.Float())
                .RuleFor(x => x.Z2, f => f.Random.Float())
                .RuleFor(x => x.DoorPosition, f => f.Random.Float())
                .RuleFor(x => x.DoorHeight, f => f.Random.Float())
                .RuleFor(x => x.Door1X, f => f.Random.Float())
                .RuleFor(x => x.Door1Y, f => f.Random.Float())
                .RuleFor(x => x.Door2X, f => f.Random.Float())
                .RuleFor(x => x.Door2Y, f => f.Random.Float())
                .RuleFor(x => x.Door1Z, f => f.Random.Float())
                .RuleFor(x => x.Door2Z, f => f.Random.Float())
                .RuleFor(x => x.Timer, f => f.Random.UInt())
                .RuleFor(x => x.CollectedCarsState, f => f.Random.Byte())
                .RuleFor(x => x.TargetCarPointer, f => f.Random.UInt())
                .RuleFor(x => x.Field96h, f => f.Random.Int())
                .RuleFor(x => x.StoredCar, f => Generator.Generate<StoredCar, TestStoredCar, GTA3SaveParams>(p));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Garage x0 = GenerateTestObject(p);
            Garage x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.Field02h, x1.Field02h);
            Assert.Equal(x0.ClosingWithoutTargetCar, x1.ClosingWithoutTargetCar);
            Assert.Equal(x0.Deactivated, x1.Deactivated);
            Assert.Equal(x0.ResprayHappened, x1.ResprayHappened);
            Assert.Equal(x0.TargetModelIndex, x1.TargetModelIndex);
            Assert.Equal(x0.Door1Pointer, x1.Door1Pointer);
            Assert.Equal(x0.Door2Pointer, x1.Door2Pointer);
            Assert.Equal(x0.Door1Handle, x1.Door1Handle);
            Assert.Equal(x0.Door2Handle, x1.Door2Handle);
            Assert.Equal(x0.Door1IsDummy, x1.Door1IsDummy);
            Assert.Equal(x0.Door2IsDummy, x1.Door2IsDummy);
            Assert.Equal(x0.RecreateDoorOnNextRefresh, x1.RecreateDoorOnNextRefresh);
            Assert.Equal(x0.RotatingDoor, x1.RotatingDoor);
            Assert.Equal(x0.CameraFollowsPlayer, x1.CameraFollowsPlayer);
            Assert.Equal(x0.X1, x1.X1);
            Assert.Equal(x0.X2, x1.X2);
            Assert.Equal(x0.Y1, x1.Y1);
            Assert.Equal(x0.Y2, x1.Y2);
            Assert.Equal(x0.Z1, x1.Z1);
            Assert.Equal(x0.Z2, x1.Z2);
            Assert.Equal(x0.DoorPosition, x1.DoorPosition);
            Assert.Equal(x0.DoorHeight, x1.DoorHeight);
            Assert.Equal(x0.Door1X, x1.Door1X);
            Assert.Equal(x0.Door1Y, x1.Door1Y);
            Assert.Equal(x0.Door2X, x1.Door2X);
            Assert.Equal(x0.Door2Y, x1.Door2Y);
            Assert.Equal(x0.Door1Z, x1.Door1Z);
            Assert.Equal(x0.Door2Z, x1.Door2Z);
            Assert.Equal(x0.Timer, x1.Timer);
            Assert.Equal(x0.CollectedCarsState, x1.CollectedCarsState);
            Assert.Equal(x0.TargetCarPointer, x1.TargetCarPointer);
            Assert.Equal(x0.Field96h, x1.Field96h);
            Assert.Equal(x0.StoredCar, x1.StoredCar);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Garage x0 = GenerateTestObject(p);
            Garage x1 = new Garage(x0);

            Assert.Equal(x0, x1);
        }
    }
}
