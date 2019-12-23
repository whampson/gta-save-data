using GTASaveData;
using GTASaveData.GTA3;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string inPath = @"..\..\TestData\PC\2_AS3";
            string outPath = homeDir + @"\Documents\GTA3 User Files\apitest.b";
            SystemType system = SystemType.PC;

            Console.WriteLine("Loading file...");
            GTA3SaveData gta3Save = GTA3SaveData.Load(inPath, system);
            gta3Save.FilePaddingMode = PaddingMode.Sequence;
            gta3Save.FilePaddingSequence = Encoding.ASCII.GetBytes("MODIFIED SAVE ");
            gta3Save.SimpleVars.LastMissionPassedName = "FUCKWIT";
            Console.WriteLine(gta3Save);
            Console.ReadKey();

            gta3Save.Store(outPath, system);
            Console.WriteLine("File saved.");
            Console.ReadKey();
        }
    }
}