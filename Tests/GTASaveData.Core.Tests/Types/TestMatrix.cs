using Bogus;
using GTASaveData.Types;
using TestFramework;

namespace GTASaveData.Core.Tests.Types
{
    public class TestMatrix : TestBase<Matrix>
    {
        public static Matrix GenerateRandom(Faker f)
        {
            Matrix m = Matrix.Identity;
            m.Position = Generator.Vector3D(f);

            return m;
        }
    }
}
