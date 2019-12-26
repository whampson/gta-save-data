using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestGarage
        : SaveDataObjectTestBase<Garage>
    {
        public override Garage GenerateTestVector(SystemType system)
        {
            Faker<Garage> model = new Faker<Garage>()
                .RuleFor(x => x.Type, f => f.PickRandom<GarageType>())
                .RuleFor(x => x.State, f => f.PickRandom<GarageState>())
                .RuleFor(x => x.ClosingWithoutTargetVehicle, f => f.Random.Bool())
                .RuleFor(x => x.Deactivated, f => f.Random.Bool())
                .RuleFor(x => x.ResprayHappened, f => f.Random.Bool())
                .RuleFor(x => x.TargetVehicle, f => f.Random.Int())
                .RuleFor(x => x.Door1Pointer, f => f.Random.UInt())
                .RuleFor(x => x.Door2Pointer, f => f.Random.UInt())
                .RuleFor(x => x.IsDoor1PoolIndex, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor2PoolIndex, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor1Object, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor2Object, f => f.Random.Bool())
                .RuleFor(x => x.Unknown1, f => f.Random.Byte())
                .RuleFor(x => x.IsRotatedDoor, f => f.Random.Bool())
                .RuleFor(x => x.CameraFollowsPlayer, f => f.Random.Bool())
                .RuleFor(x => x.Position, f => TestHelper.Generate<Box3d, TestBox3d>())
                .RuleFor(x => x.DoorOpenMinZOffset, f => f.Random.Float())
                .RuleFor(x => x.DoorOpenMaxZOffset, f => f.Random.Float())
                .RuleFor(x => x.Door1XY, f => TestHelper.Generate<Vector2d, TestVector2d>())
                .RuleFor(x => x.Door2XY, f => TestHelper.Generate<Vector2d, TestVector2d>())
                .RuleFor(x => x.Door1Z, f => f.Random.Float())
                .RuleFor(x => x.Door2Z, f => f.Random.Float())
                .RuleFor(x => x.DoorLastOpenTime, f => f.Random.UInt())
                .RuleFor(x => x.CollectedCarsState, f => f.Random.Byte())
                .RuleFor(x => x.TargetVehiclePointer, f => f.Random.UInt())
                .RuleFor(x => x.Unknown3, f => f.Random.UInt())
                .RuleFor(x => x.Unknown4, f => TestHelper.Generate<StoredCar, TestStoredCar>());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Garage x0 = GenerateTestVector();
            Garage x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(140, data.Length);
        }
    }
}
