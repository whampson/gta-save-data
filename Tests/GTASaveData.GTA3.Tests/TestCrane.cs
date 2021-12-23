using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestCrane : Base<Crane>
    {
        public override Crane GenerateTestObject(FileType format)
        {
            Faker<Crane> model = new Faker<Crane>()
                .RuleFor(x => x.Handle, f => f.Random.UInt())
                .RuleFor(x => x.HookHandle, f => f.Random.UInt())
                .RuleFor(x => x.AudioHandle, f => f.Random.Int())
                .RuleFor(x => x.PickupX1, f => f.Random.Float())
                .RuleFor(x => x.PickupX2, f => f.Random.Float())
                .RuleFor(x => x.PickupY1, f => f.Random.Float())
                .RuleFor(x => x.PickupY2, f => f.Random.Float())
                .RuleFor(x => x.DropoffTarget, f => Generator.Vector3(f))
                .RuleFor(x => x.DropoffHeading, f => f.Random.Float())
                .RuleFor(x => x.PickupAngle, f => f.Random.Float())
                .RuleFor(x => x.DropoffAngle, f => f.Random.Float())
                .RuleFor(x => x.PickupDistance, f => f.Random.Float())
                .RuleFor(x => x.DropoffDistance, f => f.Random.Float())
                .RuleFor(x => x.PickupHeight, f => f.Random.Float())
                .RuleFor(x => x.DropoffHeight, f => f.Random.Float())
                .RuleFor(x => x.HookAngle, f => f.Random.Float())
                .RuleFor(x => x.HookDistance, f => f.Random.Float())
                .RuleFor(x => x.HookHeight, f => f.Random.Float())
                .RuleFor(x => x.HookInitialPosition, f => Generator.Vector3(f))
                .RuleFor(x => x.HookCurrentPosition, f => Generator.Vector3(f))
                .RuleFor(x => x.HookVelocity, f => Generator.Vector2(f))
                .RuleFor(x => x.VehiclePickedUpHandle, f => f.Random.UInt())
                .RuleFor(x => x.TimeForNextCheck, f => f.Random.UInt())
                .RuleFor(x => x.Status, f => f.PickRandom<CraneStatus>())
                .RuleFor(x => x.State, f => f.PickRandom<CraneState>())
                .RuleFor(x => x.VehiclesCollected, f => f.Random.Byte())
                .RuleFor(x => x.IsCrusher, f => f.Random.Bool())
                .RuleFor(x => x.IsMilitaryCrane, f => f.Random.Bool())
                .RuleFor(x => x.WasMilitaryCrane, f => f.Random.Bool())
                .RuleFor(x => x.IsTop, f => f.Random.Bool());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileType format)
        {
            Crane x0 = GenerateTestObject(format);
            Crane x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Handle, x1.Handle);
            Assert.Equal(x0.HookHandle, x1.HookHandle);
            Assert.Equal(x0.AudioHandle, x1.AudioHandle);
            Assert.Equal(x0.PickupX1, x1.PickupX1);
            Assert.Equal(x0.PickupX2, x1.PickupX2);
            Assert.Equal(x0.PickupY1, x1.PickupY1);
            Assert.Equal(x0.PickupY2, x1.PickupY2);
            Assert.Equal(x0.DropoffTarget, x1.DropoffTarget);
            Assert.Equal(x0.DropoffHeading, x1.DropoffHeading);
            Assert.Equal(x0.PickupAngle, x1.PickupAngle);
            Assert.Equal(x0.DropoffAngle, x1.DropoffAngle);
            Assert.Equal(x0.PickupDistance, x1.PickupDistance);
            Assert.Equal(x0.DropoffDistance, x1.DropoffDistance);
            Assert.Equal(x0.PickupHeight, x1.PickupHeight);
            Assert.Equal(x0.DropoffHeight, x1.DropoffHeight);
            Assert.Equal(x0.HookAngle, x1.HookAngle);
            Assert.Equal(x0.HookDistance, x1.HookDistance);
            Assert.Equal(x0.HookHeight, x1.HookHeight);
            Assert.Equal(x0.HookInitialPosition, x1.HookInitialPosition);
            Assert.Equal(x0.HookCurrentPosition, x1.HookCurrentPosition);
            Assert.Equal(x0.HookVelocity, x1.HookVelocity);
            Assert.Equal(x0.VehiclePickedUpHandle, x1.VehiclePickedUpHandle);
            Assert.Equal(x0.TimeForNextCheck, x1.TimeForNextCheck);
            Assert.Equal(x0.Status, x1.Status);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.VehiclesCollected, x1.VehiclesCollected);
            Assert.Equal(x0.IsCrusher, x1.IsCrusher);
            Assert.Equal(x0.IsMilitaryCrane, x1.IsMilitaryCrane);
            Assert.Equal(x0.WasMilitaryCrane, x1.WasMilitaryCrane);
            Assert.Equal(x0.IsTop, x1.IsTop);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            Crane x0 = GenerateTestObject();
            Crane x1 = new Crane(x0);

            Assert.Equal(x0, x1);
        }
    }
}
