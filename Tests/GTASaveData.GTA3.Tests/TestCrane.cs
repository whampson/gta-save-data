using Bogus;
using GTASaveData.Core.Tests.Types;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestCrane : Base<Crane>
    {
        public override Crane GenerateTestObject(DataFormat format)
        {
            Faker<Crane> model = new Faker<Crane>()
                .RuleFor(x => x.CraneEntityPointer, f => f.Random.UInt())
                .RuleFor(x => x.HookPointer, f => f.Random.UInt())
                .RuleFor(x => x.AudioEntity, f => f.Random.Int())
                .RuleFor(x => x.PickupX1, f => f.Random.Float())
                .RuleFor(x => x.PickupX2, f => f.Random.Float())
                .RuleFor(x => x.PickupY1, f => f.Random.Float())
                .RuleFor(x => x.PickupY2, f => f.Random.Float())
                .RuleFor(x => x.DropoffTarget, Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.DropoffHeading, f => f.Random.Float())
                .RuleFor(x => x.PickupAngle, f => f.Random.Float())
                .RuleFor(x => x.DropoffAngle, f => f.Random.Float())
                .RuleFor(x => x.PickupDistance, f => f.Random.Float())
                .RuleFor(x => x.DropoffDistance, f => f.Random.Float())
                .RuleFor(x => x.PickupHeight, f => f.Random.Float())
                .RuleFor(x => x.DropoffHeight, f => f.Random.Float())
                .RuleFor(x => x.HookAngle, f => f.Random.Float())
                .RuleFor(x => x.HookOffset, f => f.Random.Float())
                .RuleFor(x => x.HookHeight, f => f.Random.Float())
                .RuleFor(x => x.HookInitialPosition, Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.HookCurrentPosition, Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.HookVelocity, f => Generator.Generate<Vector2D, TestVector2D>())
                .RuleFor(x => x.VehiclePickedUpPointer, f => f.Random.UInt())
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

        [Fact]
        public void Serialization()
        {
            Crane x0 = GenerateTestObject();
            Crane x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.CraneEntityPointer, x1.CraneEntityPointer);
            Assert.Equal(x0.HookPointer, x1.HookPointer);
            Assert.Equal(x0.AudioEntity, x1.AudioEntity);
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
            Assert.Equal(x0.HookOffset, x1.HookOffset);
            Assert.Equal(x0.HookHeight, x1.HookHeight);
            Assert.Equal(x0.HookInitialPosition, x1.HookInitialPosition);
            Assert.Equal(x0.HookCurrentPosition, x1.HookCurrentPosition);
            Assert.Equal(x0.HookVelocity, x1.HookVelocity);
            Assert.Equal(x0.VehiclePickedUpPointer, x1.VehiclePickedUpPointer);
            Assert.Equal(x0.TimeForNextCheck, x1.TimeForNextCheck);
            Assert.Equal(x0.Status, x1.Status);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.VehiclesCollected, x1.VehiclesCollected);
            Assert.Equal(x0.IsCrusher, x1.IsCrusher);
            Assert.Equal(x0.IsMilitaryCrane, x1.IsMilitaryCrane);
            Assert.Equal(x0.WasMilitaryCrane, x1.WasMilitaryCrane);
            Assert.Equal(x0.IsTop, x1.IsTop);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
