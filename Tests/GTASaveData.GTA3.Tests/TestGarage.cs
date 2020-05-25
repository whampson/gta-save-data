using Bogus;
using TestFramework;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3.Tests
{
    public class TestGarage : Base<Garage>
    {
        public override Garage GenerateTestObject(SaveDataFormat format)
        {
            Faker<Garage> model = new Faker<Garage>()
                .RuleFor(x => x.Type, f => f.PickRandom<GarageType>())
                .RuleFor(x => x.State, f => f.PickRandom<GarageState>())
                .RuleFor(x => x.Field02h, f => f.Random.Bool())
                .RuleFor(x => x.ClosingWithoutTargetVehicle, f => f.Random.Bool())
                .RuleFor(x => x.Deactivated, f => f.Random.Bool())
                .RuleFor(x => x.ResprayHappened, f => f.Random.Bool())
                .RuleFor(x => x.TargetModelIndex, f => f.Random.Int())
                .RuleFor(x => x.Door1Pointer, f => f.Random.UInt())
                .RuleFor(x => x.Door2Pointer, f => f.Random.UInt())
                .RuleFor(x => x.Door1Handle, f => f.Random.Byte())
                .RuleFor(x => x.Door2Handle, f => f.Random.Byte())
                .RuleFor(x => x.IsDoor1Dummy, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor2Dummy, f => f.Random.Bool())
                .RuleFor(x => x.RecreateDoorOnNextRefresh, f => f.Random.Bool())
                .RuleFor(x => x.RotatedDoor, f => f.Random.Bool())
                .RuleFor(x => x.CameraFollowsPlayer, f => f.Random.Bool())
                .RuleFor(x => x.PositionMin, f => Generator.Vector3D(f))
                .RuleFor(x => x.PositionMax, f => Generator.Vector3D(f))
                .RuleFor(x => x.DoorOpenOffset, f => f.Random.Float())
                .RuleFor(x => x.DoorOpenMax, f => f.Random.Float())
                .RuleFor(x => x.Door1Position, f => Generator.Vector3D(f))
                .RuleFor(x => x.Door2Position, f => Generator.Vector3D(f))
                .RuleFor(x => x.DoorLastOpenTime, f => f.Random.UInt())
                .RuleFor(x => x.CollectedCarsState, f => f.Random.Byte())
                .RuleFor(x => x.TargetCarPointer, f => f.Random.UInt())
                .RuleFor(x => x.Field96h, f => f.Random.Int())
                .RuleFor(x => x.StoredCar, f => Generator.Generate<StoredCar, TestStoredCar>());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            Garage x0 = GenerateTestObject();
            Garage x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.Field02h, x1.Field02h);
            Assert.Equal(x0.ClosingWithoutTargetVehicle, x1.ClosingWithoutTargetVehicle);
            Assert.Equal(x0.Deactivated, x1.Deactivated);
            Assert.Equal(x0.ResprayHappened, x1.ResprayHappened);
            Assert.Equal(x0.TargetModelIndex, x1.TargetModelIndex);
            Assert.Equal(x0.Door1Pointer, x1.Door1Pointer);
            Assert.Equal(x0.Door2Pointer, x1.Door2Pointer);
            Assert.Equal(x0.Door1Handle, x1.Door1Handle);
            Assert.Equal(x0.Door2Handle, x1.Door2Handle);
            Assert.Equal(x0.IsDoor1Dummy, x1.IsDoor1Dummy);
            Assert.Equal(x0.IsDoor2Dummy, x1.IsDoor2Dummy);
            Assert.Equal(x0.RecreateDoorOnNextRefresh, x1.RecreateDoorOnNextRefresh);
            Assert.Equal(x0.RotatedDoor, x1.RotatedDoor);
            Assert.Equal(x0.CameraFollowsPlayer, x1.CameraFollowsPlayer);
            Assert.Equal(x0.PositionMin, x1.PositionMin);
            Assert.Equal(x0.PositionMax, x1.PositionMax);
            Assert.Equal(x0.DoorOpenOffset, x1.DoorOpenOffset);
            Assert.Equal(x0.DoorOpenMax, x1.DoorOpenMax);
            Assert.Equal(x0.Door1Position, x1.Door1Position);
            Assert.Equal(x0.Door2Position, x1.Door2Position);
            Assert.Equal(x0.DoorLastOpenTime, x1.DoorLastOpenTime);
            Assert.Equal(x0.CollectedCarsState, x1.CollectedCarsState);
            Assert.Equal(x0.TargetCarPointer, x1.TargetCarPointer);
            Assert.Equal(x0.Field96h, x1.Field96h);
            Assert.Equal(x0.StoredCar, x1.StoredCar);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
