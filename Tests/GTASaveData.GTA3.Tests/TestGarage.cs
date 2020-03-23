using Bogus;
using GTASaveData.Core.Tests.Types;
using GTASaveData.GTA3;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestGarage : SerializableObjectTestBase<Garage>
    {
        public override Garage GenerateTestObject(SaveFileFormat format)
        {
            Faker<Garage> model = new Faker<Garage>()
                .RuleFor(x => x.Type, f => f.PickRandom<GarageType>())
                .RuleFor(x => x.State, f => f.PickRandom<GarageState>())
                .RuleFor(x => x.Field02h, f => f.Random.Byte())
                .RuleFor(x => x.ClosingWithoutTargetVehicle, f => f.Random.Bool())
                .RuleFor(x => x.Deactivated, f => f.Random.Bool())
                .RuleFor(x => x.ResprayHappened, f => f.Random.Bool())
                .RuleFor(x => x.Field06h, f => f.Random.Byte())
                .RuleFor(x => x.Field07h, f => f.Random.Byte())
                .RuleFor(x => x.TargetModelIndex, f => f.Random.Int())
                .RuleFor(x => x.Door1Pointer, f => f.Random.UInt())
                .RuleFor(x => x.Door2Pointer, f => f.Random.UInt())
                .RuleFor(x => x.IsDoor1PoolIndex, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor2PoolIndex, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor1Object, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor2Object, f => f.Random.Bool())
                .RuleFor(x => x.Field24h, f => f.Random.Byte())
                .RuleFor(x => x.IsRotatedDoor, f => f.Random.Bool())
                .RuleFor(x => x.CameraFollowsPlayer, f => f.Random.Bool())
                .RuleFor(x => x.Field27h, f => f.Random.Byte())
                .RuleFor(x => x.PositionInf, f => Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.PositionSup, f => Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.DoorOpenMinZOffset, f => f.Random.Float())
                .RuleFor(x => x.DoorOpenMaxZOffset, f => f.Random.Float())
                .RuleFor(x => x.Door1Pos, f => Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.Door2Pos, f => Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.DoorLastOpenTime, f => f.Random.UInt())
                .RuleFor(x => x.CollectedCarsState, f => f.Random.Byte())
                .RuleFor(x => x.Field89h, f => f.Random.Byte())
                .RuleFor(x => x.Field90h, f => f.Random.Byte())
                .RuleFor(x => x.Field91h, f => f.Random.Byte())
                .RuleFor(x => x.TargetVehiclePointer, f => f.Random.UInt())
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
            Assert.Equal(x0.Field06h, x1.Field06h);
            Assert.Equal(x0.Field07h, x1.Field07h);
            Assert.Equal(x0.TargetModelIndex, x1.TargetModelIndex);
            Assert.Equal(x0.Door1Pointer, x1.Door1Pointer);
            Assert.Equal(x0.Door2Pointer, x1.Door2Pointer);
            Assert.Equal(x0.IsDoor1PoolIndex, x1.IsDoor1PoolIndex);
            Assert.Equal(x0.IsDoor2PoolIndex, x1.IsDoor2PoolIndex);
            Assert.Equal(x0.IsDoor1Object, x1.IsDoor1Object);
            Assert.Equal(x0.IsDoor2Object, x1.IsDoor2Object);
            Assert.Equal(x0.Field24h, x1.Field24h);
            Assert.Equal(x0.IsRotatedDoor, x1.IsRotatedDoor);
            Assert.Equal(x0.CameraFollowsPlayer, x1.CameraFollowsPlayer);
            Assert.Equal(x0.Field27h, x1.Field27h);
            Assert.Equal(x0.PositionInf, x1.PositionInf);
            Assert.Equal(x0.PositionSup, x1.PositionSup);
            Assert.Equal(x0.DoorOpenMinZOffset, x1.DoorOpenMinZOffset);
            Assert.Equal(x0.DoorOpenMaxZOffset, x1.DoorOpenMaxZOffset);
            Assert.Equal(x0.Door1Pos, x1.Door1Pos);
            Assert.Equal(x0.Door2Pos, x1.Door2Pos);
            Assert.Equal(x0.DoorLastOpenTime, x1.DoorLastOpenTime);
            Assert.Equal(x0.CollectedCarsState, x1.CollectedCarsState);
            Assert.Equal(x0.Field89h, x1.Field89h);
            Assert.Equal(x0.Field90h, x1.Field90h);
            Assert.Equal(x0.Field91h, x1.Field91h);
            Assert.Equal(x0.TargetVehiclePointer, x1.TargetVehiclePointer);
            Assert.Equal(x0.Field96h, x1.Field96h);
            Assert.Equal(x0.StoredCar, x1.StoredCar);
            Assert.Equal(x0, x1);
            Assert.Equal(SizeOf<Garage>(), data.Length);
        }
    }
}
