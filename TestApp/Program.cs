using GTASaveData;
using GTASaveData.GTA3;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            //string savePS20 = homeDir + @"\Desktop\gta\Saves\gta3\ps2\2 'PATRIOT PLAYGROUND'";
            //string savePS21 = homeDir + @"\Desktop\gta\Saves\gta3\ps2_au\3 'SAM'";
            //string savePS22 = homeDir + @"\Desktop\gta\Saves\gta3\ps2_jp\6_LM2";
            //string savePC0 = homeDir + @"\Documents\GTA3 User Files\GTA3sf1.b";

            string inPath = @"..\..\TestData\PC\2_AS3";
            string outPath = homeDir + @"\Documents\GTA3 User Files\apitest.b";
            SystemType system = SystemType.PC;

            Console.WriteLine("Loading file...");
            SaveData gta3Save = SaveData.Load(inPath, system);
            Console.WriteLine(gta3Save);
            Console.ReadKey();

            gta3Save.Store(outPath, system);
            Console.WriteLine("File saved.");
            Console.ReadKey();
        }
    }
}