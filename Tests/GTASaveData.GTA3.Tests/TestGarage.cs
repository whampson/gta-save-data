using Bogus;
using GTASaveData.Core.Tests.Types;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGarage : Base<Garage>
    {
        public override Garage GenerateTestObject(DataFormat format)
        {
            Faker<Garage> model = new Faker<Garage>()
                .RuleFor(x => x.Type, f => f.PickRandom<GarageType>())
                .RuleFor(x => x.State, f => f.PickRandom<GarageState>())
                .RuleFor(x => x.Field02h, f => f.Random.Byte())
                .RuleFor(x => x.ClosingWithoutTargetVehicle, f => f.Random.Bool())
                .RuleFor(x => x.Deactivated, f => f.Random.Bool())
                .RuleFor(x => x.ResprayHappened, f => f.Random.Bool())
                .RuleFor(x => x.TargetModelIndex, f => f.Random.Int())
                .RuleFor(x => x.Door1Pointer, f => f.Random.UInt())
                .RuleFor(x => x.Door2Pointer, f => f.Random.UInt())
                .RuleFor(x => x.Door1PoolIndex, f => f.Random.Byte())
                .RuleFor(x => x.Door2PoolIndex, f => f.Random.Byte())
                .RuleFor(x => x.IsDoor1Object, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor2Object, f => f.Random.Bool())
                .RuleFor(x => x.RecreateDoorOnNextRefresh, f => f.Random.Bool())
                .RuleFor(x => x.RotatedDoor, f => f.Random.Bool())
                .RuleFor(x => x.CameraFollowsPlayer, f => f.Random.Bool())
                .RuleFor(x => x.Field27h, f => f.Random.Byte())
                .RuleFor(x => x.Position1, f => Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.Position2, f => Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.DoorPosition, f => f.Random.Float())
                .RuleFor(x => x.DoorHeight, f => f.Random.Float())
                .RuleFor(x => x.Door1Pos, f => Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.Door2Pos, f => Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.DoorLastOpenTime, f => f.Random.UInt())
                .RuleFor(x => x.CollectedCarsState, f => f.Random.Byte())
                .RuleFor(x => x.TargetCarPointer, f => f.Random.UInt())
                .RuleFor(x => x.Field96h, f => f.Random.Int())
                .RuleFor(x => x.StoredCar, f => Generator.Generate<StoredCar, TestStoredCar>());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
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
            Assert.Equal(x0.Door1PoolIndex, x1.Door1PoolIndex);
            Assert.Equal(x0.Door2PoolIndex, x1.Door2PoolIndex);
            Assert.Equal(x0.IsDoor1Object, x1.IsDoor1Object);
            Assert.Equal(x0.IsDoor2Object, x1.IsDoor2Object);
            Assert.Equal(x0.RecreateDoorOnNextRefresh, x1.RecreateDoorOnNextRefresh);
            Assert.Equal(x0.RotatedDoor, x1.RotatedDoor);
            Assert.Equal(x0.CameraFollowsPlayer, x1.CameraFollowsPlayer);
            Assert.Equal(x0.Field27h, x1.Field27h);
            Assert.Equal(x0.Position1, x1.Position1);
            Assert.Equal(x0.Position2, x1.Position2);
            Assert.Equal(x0.DoorPosition, x1.DoorPosition);
            Assert.Equal(x0.DoorHeight, x1.DoorHeight);
            Assert.Equal(x0.Door1Pos, x1.Door1Pos);
            Assert.Equal(x0.Door2Pos, x1.Door2Pos);
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
