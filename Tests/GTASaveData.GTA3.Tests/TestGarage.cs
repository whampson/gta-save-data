using Bogus;
using GTASaveData.Common;
using GTASaveData.Core.Tests.Common;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestGarage : SerializableObjectTestBase<Garage>
    {
        public override Garage GenerateTestVector(FileFormat format)
        {
            Faker<Garage> model = new Faker<Garage>()
                .RuleFor(x => x.Type, f => f.PickRandom<GarageType>())
                .RuleFor(x => x.State, f => f.PickRandom<GarageState>())
                .RuleFor(x => x.ClosingWithoutTargetVehicle, f => f.Random.Bool())
                .RuleFor(x => x.Deactivated, f => f.Random.Bool())
                .RuleFor(x => x.ResprayHappened, f => f.Random.Bool())
                .RuleFor(x => x.TargetVehicle, f => f.PickRandom<VehicleModel>())
                .RuleFor(x => x.Door2Pointer, f => f.Random.UInt())
                .RuleFor(x => x.IsDoor1PoolIndex, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor2PoolIndex, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor1Object, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor2Object, f => f.Random.Bool())
                .RuleFor(x => x.Unknown1, f => f.Random.Byte())
                .RuleFor(x => x.IsRotatedDoor, f => f.Random.Bool())
                .RuleFor(x => x.CameraFollowsPlayer, f => f.Random.Bool())
                .RuleFor(x => x.Position, f => Generator.Generate<Rect3d, TestRect3d>())
                .RuleFor(x => x.DoorOpenMinZOffset, f => f.Random.Float())
                .RuleFor(x => x.DoorOpenMaxZOffset, f => f.Random.Float())
                .RuleFor(x => x.Door1XY, f => Generator.Generate<Vector2d, TestVector2d>())
                .RuleFor(x => x.Door2XY, f => Generator.Generate<Vector2d, TestVector2d>())
                .RuleFor(x => x.Door1Z, f => f.Random.Float())
                .RuleFor(x => x.Door2Z, f => f.Random.Float())
                .RuleFor(x => x.DoorLastOpenTime, f => f.Random.UInt())
                .RuleFor(x => x.CollectedCarsState, f => f.Random.Byte())
                .RuleFor(x => x.TargetVehiclePointer, f => f.Random.UInt())
                .RuleFor(x => x.Unknown3, f => f.Random.UInt())
                .RuleFor(x => x.Unknown4, f => Generator.Generate<StoredCar, TestStoredCar>());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Garage x0 = GenerateTestVector();
            Garage x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.ClosingWithoutTargetVehicle, x1.ClosingWithoutTargetVehicle);
            Assert.Equal(x0.Deactivated, x1.Deactivated);
            Assert.Equal(x0.ResprayHappened, x1.ResprayHappened);
            Assert.Equal(x0.TargetVehicle, x1.TargetVehicle);
            Assert.Equal(x0.Door2Pointer, x1.Door2Pointer);
            Assert.Equal(x0.IsDoor1PoolIndex, x1.IsDoor1PoolIndex);
            Assert.Equal(x0.IsDoor2PoolIndex, x1.IsDoor2PoolIndex);
            Assert.Equal(x0.IsDoor1Object, x1.IsDoor1Object);
            Assert.Equal(x0.IsDoor2Object, x1.IsDoor2Object);
            Assert.Equal(x0.Unknown1, x1.Unknown1);
            Assert.Equal(x0.IsRotatedDoor, x1.IsRotatedDoor);
            Assert.Equal(x0.CameraFollowsPlayer, x1.CameraFollowsPlayer);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.DoorOpenMinZOffset, x1.DoorOpenMinZOffset);
            Assert.Equal(x0.DoorOpenMaxZOffset, x1.DoorOpenMaxZOffset);
            Assert.Equal(x0.Door1XY, x1.Door1XY);
            Assert.Equal(x0.Door2XY, x1.Door2XY);
            Assert.Equal(x0.Door1Z, x1.Door1Z);
            Assert.Equal(x0.Door2Z, x1.Door2Z);
            Assert.Equal(x0.DoorLastOpenTime, x1.DoorLastOpenTime);
            Assert.Equal(x0.CollectedCarsState, x1.CollectedCarsState);
            Assert.Equal(x0.TargetVehiclePointer, x1.TargetVehiclePointer);
            Assert.Equal(x0.Unknown3, x1.Unknown3);
            Assert.Equal(x0.Unknown4, x1.Unknown4);
            Assert.Equal(x0, x1);
            Assert.Equal(140, data.Length);
        }
    }
}
