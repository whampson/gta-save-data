using Bogus;
using TestFramework;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.LCS.Tests
{
    public class TestGarage : Base<Garage>
    {
        public override Garage GenerateTestObject(FileFormat format)
        {
            Faker<Garage> model = new Faker<Garage>()
                .RuleFor(x => x.Type, f => f.PickRandom<GarageType>())
                .RuleFor(x => x.State, f => f.PickRandom<GarageState>())
                .RuleFor(x => x.MaxCarsAllowed, f => f.Random.Byte())
                .RuleFor(x => x.ClosingWithoutTargetVehicle, f => f.Random.Bool())
                .RuleFor(x => x.Deactivated, f => f.Random.Bool())
                .RuleFor(x => x.ResprayHappened, f => f.Random.Bool())
                .RuleFor(x => x.Door1Pointer, f => f.Random.UInt())
                .RuleFor(x => x.Door2Pointer, f => f.Random.UInt())
                .RuleFor(x => x.Door1Handle, f => f.Random.Byte())
                .RuleFor(x => x.Door2Handle, f => f.Random.Byte())
                .RuleFor(x => x.IsDoor1Dummy, f => f.Random.Bool())
                .RuleFor(x => x.IsDoor2Dummy, f => f.Random.Bool())
                .RuleFor(x => x.RecreateDoorOnNextRefresh, f => f.Random.Bool())
                .RuleFor(x => x.RotatingDoor, f => f.Random.Bool())
                .RuleFor(x => x.CameraFollowsPlayer, f => f.Random.Bool())
                .RuleFor(x => x.Position, f => Generator.Vector3(f))
                .RuleFor(x => x.Rotation, f => Generator.Quaternion(f))
                .RuleFor(x => x.CeilingZ, f => f.Random.Float())
                .RuleFor(x => x.DoorRelated1, f => f.Random.Float())
                .RuleFor(x => x.DoorRelated2, f => f.Random.Float())
                .RuleFor(x => x.X1, f => f.Random.Float())
                .RuleFor(x => x.X2, f => f.Random.Float())
                .RuleFor(x => x.Y1, f => f.Random.Float())
                .RuleFor(x => x.Y2, f => f.Random.Float())
                .RuleFor(x => x.DoorOpenOffset, f => f.Random.Float())
                .RuleFor(x => x.DoorOpenMax, f => f.Random.Float())
                .RuleFor(x => x.Door1X, f => f.Random.Float())
                .RuleFor(x => x.Door1Y, f => f.Random.Float())
                .RuleFor(x => x.Door2X, f => f.Random.Float())
                .RuleFor(x => x.Door2Y, f => f.Random.Float())
                .RuleFor(x => x.Door1Z, f => f.Random.Float())
                .RuleFor(x => x.Door2Z, f => f.Random.Float())
                .RuleFor(x => x.Timer, f => f.Random.UInt());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            Garage x0 = GenerateTestObject(format);
            Garage x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.MaxCarsAllowed, x1.MaxCarsAllowed);
            Assert.Equal(x0.ClosingWithoutTargetVehicle, x1.ClosingWithoutTargetVehicle);
            Assert.Equal(x0.Deactivated, x1.Deactivated);
            Assert.Equal(x0.ResprayHappened, x1.ResprayHappened);
            Assert.Equal(x0.Door1Pointer, x1.Door1Pointer);
            Assert.Equal(x0.Door2Pointer, x1.Door2Pointer);
            Assert.Equal(x0.Door1Handle, x1.Door1Handle);
            Assert.Equal(x0.Door2Handle, x1.Door2Handle);
            Assert.Equal(x0.IsDoor1Dummy, x1.IsDoor1Dummy);
            Assert.Equal(x0.IsDoor2Dummy, x1.IsDoor2Dummy);
            Assert.Equal(x0.RecreateDoorOnNextRefresh, x1.RecreateDoorOnNextRefresh);
            Assert.Equal(x0.RotatingDoor, x1.RotatingDoor);
            Assert.Equal(x0.CameraFollowsPlayer, x1.CameraFollowsPlayer);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.Rotation, x1.Rotation);
            Assert.Equal(x0.CeilingZ, x1.CeilingZ);
            Assert.Equal(x0.DoorRelated1, x1.DoorRelated1);
            Assert.Equal(x0.DoorRelated2, x1.DoorRelated2);
            Assert.Equal(x0.X1, x1.X1);
            Assert.Equal(x0.X2, x1.X2);
            Assert.Equal(x0.Y1, x1.Y1);
            Assert.Equal(x0.Y2, x1.Y2);
            Assert.Equal(x0.DoorOpenOffset, x1.DoorOpenOffset);
            Assert.Equal(x0.DoorOpenMax, x1.DoorOpenMax);
            Assert.Equal(x0.Door1X, x1.Door1X);
            Assert.Equal(x0.Door1Y, x1.Door1Y);
            Assert.Equal(x0.Door2X, x1.Door2X);
            Assert.Equal(x0.Door2Y, x1.Door2Y);
            Assert.Equal(x0.Door1Z, x1.Door1Z);
            Assert.Equal(x0.Door2Z, x1.Door2Z);
            Assert.Equal(x0.Timer, x1.Timer);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
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
