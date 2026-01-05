using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestParticleObject : Base<ParticleObject>
    {
        public override ParticleObject GenerateTestObject(GTA3SaveParams p)
        {
            Faker<ParticleObject> model = new Faker<ParticleObject>()
                .RuleFor(x => x.Position, f => Generator.Vector3(f))
                .RuleFor(x => x.NextParticleObjectPointer, f => f.Random.UInt())
                .RuleFor(x => x.PrevParticleObjectPointer, f => f.Random.UInt())
                .RuleFor(x => x.ParticlePointer, f => f.Random.UInt())
                .RuleFor(x => x.Timer, f => f.Random.UInt())
                .RuleFor(x => x.Type, f => f.PickRandom<ParticleObjectType>())
                .RuleFor(x => x.ParticleType, f => f.PickRandom<ParticleType>())
                .RuleFor(x => x.NumEffectCycles, f => f.Random.Byte())
                .RuleFor(x => x.SkipFrames, f => f.Random.Byte())
                .RuleFor(x => x.FrameCounter, f => f.Random.UShort())
                .RuleFor(x => x.State, f => f.PickRandom<ParticleObjectState>())
                .RuleFor(x => x.Target, f => Generator.Vector3(f))
                .RuleFor(x => x.Spread, f => f.Random.Float())
                .RuleFor(x => x.Size, f => f.Random.Float())
                .RuleFor(x => x.Color, f => f.Random.UInt())
                .RuleFor(x => x.DestroyWhenFar, f => f.Random.Bool())
                .RuleFor(x => x.CreationChance, f => f.Random.SByte())
                .RuleFor(x => x.Unknown, f => (p.FileType.IsPS2) ? f.Random.Int() : default);

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            ParticleObject x0 = GenerateTestObject(p);
            ParticleObject x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.NextParticleObjectPointer, x1.NextParticleObjectPointer);
            Assert.Equal(x0.PrevParticleObjectPointer, x1.PrevParticleObjectPointer);
            Assert.Equal(x0.ParticlePointer, x1.ParticlePointer);
            Assert.Equal(x0.Timer, x1.Timer);
            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.ParticleType, x1.ParticleType);
            Assert.Equal(x0.NumEffectCycles, x1.NumEffectCycles);
            Assert.Equal(x0.SkipFrames, x1.SkipFrames);
            Assert.Equal(x0.FrameCounter, x1.FrameCounter);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.Target, x1.Target);
            Assert.Equal(x0.Spread, x1.Spread);
            Assert.Equal(x0.Size, x1.Size);
            Assert.Equal(x0.Color, x1.Color);
            Assert.Equal(x0.DestroyWhenFar, x1.DestroyWhenFar);
            Assert.Equal(x0.CreationChance, x1.CreationChance);
            Assert.Equal(x0.Unknown, x1.Unknown);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            ParticleObject x0 = GenerateTestObject(p);
            ParticleObject x1 = new ParticleObject(x0);

            Assert.Equal(x0, x1);
        }
    }
}
