using Bogus;
using GTASaveData.Types;
using TestFramework;

namespace GTASaveData.Core.Tests.Types
{
    public class TestViewMatrix : TestBase<ViewMatrix>
    {
        public static ViewMatrix GenerateRandom(Faker f)
        {
            ViewMatrix m = ViewMatrix.Identity;
            m.Position = Generator.Vector3D(f);

            return m;
        }
    }
}
