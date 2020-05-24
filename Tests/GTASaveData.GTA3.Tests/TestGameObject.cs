using Bogus;
using GTASaveData.Core.Tests.Types;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGameObject : Base<GameObject>
    {
        public override GameObject GenerateTestObject(DataFormat format)
        {
            Faker<GameObject> model = new Faker<GameObject>()
                .RuleFor(x => x.ModelIndex, f => f.Random.Short())
                .RuleFor(x => x.Handle, f => f.Random.Int())
                .RuleFor(x => x.Matrix, f => TestMatrix.GenerateRandom(f))
                .RuleFor(x => x.UprootLimit, f => f.Random.Float())
                .RuleFor(x => x.ObjectMatrix, f => TestMatrix.GenerateRandom(f))
                .RuleFor(x => x.CreatedBy, f => f.Random.Byte())
                .RuleFor(x => x.IsPickup, f => f.Random.Bool())
                .RuleFor(x => x.IsPickupInShop, f => f.Random.Bool())
                .RuleFor(x => x.IsPickupOutOfStock, f => f.Random.Bool())
                .RuleFor(x => x.IsGlassCracked, f => f.Random.Bool())
                .RuleFor(x => x.IsGlassBroken, f => f.Random.Bool())
                .RuleFor(x => x.HasBeenDamaged, f => f.Random.Bool())
                .RuleFor(x => x.UseCarColors, f => f.Random.Bool())
                .RuleFor(x => x.CollisionDamageMultiplier, f => f.Random.Float())
                .RuleFor(x => x.CollisionDamageEffect, f => f.Random.Byte())
                .RuleFor(x => x.SpecialCollisionResponseCases, f => f.Random.Byte())
                .RuleFor(x => x.EndOfLifeTime, f => f.Random.UInt())
                .RuleFor(x => x.EntityFlags, f => f.PickRandom<EntityFlags>());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            GameObject x0 = GenerateTestObject();
            GameObject x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.ModelIndex, x1.ModelIndex);
            Assert.Equal(x0.Handle, x1.Handle);
            Assert.Equal(x0.Matrix, x1.Matrix);
            Assert.Equal(x0.UprootLimit, x1.UprootLimit);
            Assert.Equal(x0.ObjectMatrix, x1.ObjectMatrix);
            Assert.Equal(x0.CreatedBy, x1.CreatedBy);
            Assert.Equal(x0.IsPickup, x1.IsPickup);
            Assert.Equal(x0.IsPickupInShop, x1.IsPickupInShop);
            Assert.Equal(x0.IsPickupOutOfStock, x1.IsPickupOutOfStock);
            Assert.Equal(x0.IsGlassCracked, x1.IsGlassCracked);
            Assert.Equal(x0.IsGlassBroken, x1.IsGlassBroken);
            Assert.Equal(x0.HasBeenDamaged, x1.HasBeenDamaged);
            Assert.Equal(x0.UseCarColors, x1.UseCarColors);
            Assert.Equal(x0.CollisionDamageMultiplier, x1.CollisionDamageMultiplier);
            Assert.Equal(x0.CollisionDamageEffect, x1.CollisionDamageEffect);
            Assert.Equal(x0.SpecialCollisionResponseCases, x1.SpecialCollisionResponseCases);
            Assert.Equal(x0.EndOfLifeTime, x1.EndOfLifeTime);
            Assert.Equal(x0.EntityFlags, x1.EntityFlags);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
