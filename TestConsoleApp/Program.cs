using GTASaveData;
using GTASaveData.GTA3;
using System;
using System.IO;
using TestFramework;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = Game.GTA3;
            var plat = GTA3Save.FileTypes.PC;
            var name = "JM4";
            var path = TestData.GetTestDataPath(game, plat, name);
            var save = GTA3Save.Load(path);
            if (save == null)
            {
                Console.WriteLine("File not found: " + path);
                return;
            }

            {
                var simp = save.SimpleVars;
                Console.WriteLine(save.SimpleVars.ToJsonString());
            }
        }
    }
}
