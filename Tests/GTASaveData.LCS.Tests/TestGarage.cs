using Bogus;
using TestFramework;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.LCS.Tests
{
    public class TestGarage : Base<Garage>
    {
        public override Garage GenerateTestObject(LCSSaveParams p)
        {
            Faker<Garage> model = new Faker<Garage>()
                .RuleFor(x => x.Type, f => f.PickRandom<GarageType>())
                .RuleFor(x => x.State, f => f.PickRandom<GarageState>())
                .RuleFor(x => x.MaxStoredCars, f => f.Random.Byte())
                .RuleFor(x => x.UnusedField, f => f.Random.Byte())
                .RuleFor(x => x.ClosingWithoutTargetCar, f => f.Random.Bool())
                .RuleFor(x => x.Deactivated, f => f.Random.Bool())
                .RuleFor(x => x.ResprayHappened, f => f.Random.Bool())
                .RuleFor(x => x.TargetModelIndex, f => f.Random.Int())
                .RuleFor(x => x.Door1Pointer, f => (p.FileType.IsiOS || p.FileType.IsAndroid) ? f.Random.Long() : f.Random.Int())
                .RuleFor(x => x.Door2Pointer, f => (p.FileType.IsiOS || p.FileType.IsAndroid) ? f.Random.Long() : f.Random.Int())
                .RuleFor(x => x.Door1PoolIndex, f => f.Random.Byte())
                .RuleFor(x => x.Door2PoolIndex, f => f.Random.Byte())
                .RuleFor(x => x.Door1IsDummy, f => f.Random.Bool())
                .RuleFor(x => x.Door2IsDummy, f => f.Random.Bool())
                .RuleFor(x => x.RecreateDoorsOnNextRefresh, f => f.Random.Bool())
                .RuleFor(x => x.RotatingDoor, f => f.Random.Bool())
                .RuleFor(x => x.CameraFollowsPlayer, f => f.Random.Bool())
                .RuleFor(x => x.Corner1, f => Generator.Vector2(f))
                .RuleFor(x => x.InfZ, f => f.Random.Float())
                .RuleFor(x => x.Dir1, f => Generator.Vector2(f))
                .RuleFor(x => x.Dir2, f => Generator.Vector2(f))
                .RuleFor(x => x.SupZ, f => f.Random.Float())
                .RuleFor(x => x.ScriptGaragePos, f => Generator.Vector3(f))
                .RuleFor(x => x.ScriptGarageAngle, f => f.Random.Float())
                .RuleFor(x => x.Unknown1, f => f.Random.Int())
                .RuleFor(x => x.Unknown2, f => f.Random.Int())
                .RuleFor(x => x.Unknown3, f => f.Random.Int())
                .RuleFor(x => x.Unknown4, f => (p.FileType.IsPSP) ? f.Random.Int() : 0)
                .RuleFor(x => x.Dir1Len, f => f.Random.Float())
                .RuleFor(x => x.Dir2Len, f => f.Random.Float())
                .RuleFor(x => x.InfX, f => f.Random.Float())
                .RuleFor(x => x.SupX, f => f.Random.Float())
                .RuleFor(x => x.InfY, f => f.Random.Float())
                .RuleFor(x => x.SupY, f => f.Random.Float())
                .RuleFor(x => x.TimeCrusherCraneActivated, f => f.Random.Int())
                .RuleFor(x => x.ScriptTargetCar, f => (!p.FileType.IsiOS && !p.FileType.IsAndroid) ? f.Random.Int() : 0)
                .RuleFor(x => x.DoorPos, f => f.Random.Float())
                .RuleFor(x => x.DoorHeight, f => f.Random.Float())
                .RuleFor(x => x.Door1X, f => f.Random.Float())
                .RuleFor(x => x.Door1Y, f => f.Random.Float())
                .RuleFor(x => x.Door2X, f => f.Random.Float())
                .RuleFor(x => x.Door2Y, f => f.Random.Float())
                .RuleFor(x => x.Door1Z, f => f.Random.Float())
                .RuleFor(x => x.Door2Z, f => f.Random.Float())
                .RuleFor(x => x.TimeToStartAction, f => f.Random.Int())
                .RuleFor(x => x.CollectedCarState, f => (p.FileType.IsPS2) ? f.Random.Int() : f.Random.Byte())
                .RuleFor(x => x.TargetCarPointer, f => (p.FileType.IsiOS || p.FileType.IsAndroid) ? f.Random.Long() : f.Random.Int())
                .RuleFor(x => x.StoredCar, f => Generator.Generate<StoredCar, TestStoredCar, LCSSaveParams>(p))
                .RuleFor(x => x.Initialized, f => f.Random.Bool())
                .RuleFor(x => x.MultiplayerCar, f => (p.FileType.IsPSP) ? f.Random.Int() : 0)
                .RuleFor(x => x.ScriptGarageAcceptedCar, f => f.Random.Bool())
                .RuleFor(x => x.Locked, f => f.Random.Bool())
                .RuleFor(x => x.ScriptGarageState, f => (p.FileType.IsPS2) ? f.Random.Int() : f.Random.Byte())
                .RuleFor(x => x.ScriptGarageStateChanging, f => f.Random.Bool())
            ;

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(LCSSaveParams p)
        {
            Garage x0 = GenerateTestObject(p);
            Garage x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.MaxStoredCars, x1.MaxStoredCars);
            Assert.Equal(x0.UnusedField, x1.UnusedField);
            Assert.Equal(x0.ClosingWithoutTargetCar, x1.ClosingWithoutTargetCar);
            Assert.Equal(x0.Deactivated, x1.Deactivated);
            Assert.Equal(x0.ResprayHappened, x1.ResprayHappened);
            Assert.Equal(x0.TargetModelIndex, x1.TargetModelIndex);
            Assert.Equal(x0.Door1Pointer, x1.Door1Pointer);
            Assert.Equal(x0.Door2Pointer, x1.Door2Pointer);
            Assert.Equal(x0.Door1PoolIndex, x1.Door1PoolIndex);
            Assert.Equal(x0.Door2PoolIndex, x1.Door2PoolIndex);
            Assert.Equal(x0.Door1IsDummy, x1.Door1IsDummy);
            Assert.Equal(x0.Door2IsDummy, x1.Door2IsDummy);
            Assert.Equal(x0.RecreateDoorsOnNextRefresh, x1.RecreateDoorsOnNextRefresh);
            Assert.Equal(x0.RotatingDoor, x1.RotatingDoor);
            Assert.Equal(x0.CameraFollowsPlayer, x1.CameraFollowsPlayer);
            Assert.Equal(x0.Corner1, x1.Corner1);
            Assert.Equal(x0.InfZ, x1.InfZ);
            Assert.Equal(x0.Dir1, x1.Dir1);
            Assert.Equal(x0.Dir2, x1.Dir2);
            Assert.Equal(x0.SupZ, x1.SupZ);
            Assert.Equal(x0.ScriptGaragePos, x1.ScriptGaragePos);
            Assert.Equal(x0.ScriptGarageAngle, x1.ScriptGarageAngle);
            Assert.Equal(x0.Unknown1, x1.Unknown1);
            Assert.Equal(x0.Unknown2, x1.Unknown2);
            Assert.Equal(x0.Unknown3, x1.Unknown3);
            Assert.Equal(x0.Unknown4, x1.Unknown4);
            Assert.Equal(x0.Dir1Len, x1.Dir1Len);
            Assert.Equal(x0.Dir2Len, x1.Dir2Len);
            Assert.Equal(x0.InfX, x1.InfX);
            Assert.Equal(x0.SupX, x1.SupX);
            Assert.Equal(x0.InfY, x1.InfY);
            Assert.Equal(x0.SupY, x1.SupY);
            Assert.Equal(x0.TimeCrusherCraneActivated, x1.TimeCrusherCraneActivated);
            Assert.Equal(x0.ScriptTargetCar, x1.ScriptTargetCar);
            Assert.Equal(x0.DoorPos, x1.DoorPos);
            Assert.Equal(x0.DoorHeight, x1.DoorHeight);
            Assert.Equal(x0.Door1X, x1.Door1X);
            Assert.Equal(x0.Door1Y, x1.Door1Y);
            Assert.Equal(x0.Door2X, x1.Door2X);
            Assert.Equal(x0.Door2Y, x1.Door2Y);
            Assert.Equal(x0.Door1Z, x1.Door1Z);
            Assert.Equal(x0.Door2Z, x1.Door2Z);
            Assert.Equal(x0.TimeToStartAction, x1.TimeToStartAction);
            Assert.Equal(x0.CollectedCarState, x1.CollectedCarState);
            Assert.Equal(x0.TargetCarPointer, x1.TargetCarPointer);
            Assert.Equal(x0.StoredCar, x1.StoredCar);
            Assert.Equal(x0.Initialized, x1.Initialized);
            Assert.Equal(x0.MultiplayerCar, x1.MultiplayerCar);
            Assert.Equal(x0.ScriptGarageAcceptedCar, x1.ScriptGarageAcceptedCar);
            Assert.Equal(x0.Locked, x1.Locked);
            Assert.Equal(x0.ScriptGarageState, x1.ScriptGarageState);
            Assert.Equal(x0.ScriptGarageStateChanging, x1.ScriptGarageStateChanging);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            Garage x0 = GenerateTestObject();
            Garage x1 = new Garage(x0);

            Assert.Equal(x0, x1);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
