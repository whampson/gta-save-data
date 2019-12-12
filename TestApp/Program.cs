using GTASaveData.Common;
using GTASaveData.GTA3;
using System;
using System.Runtime.Serialization;

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

            SaveData gta3Save = SaveData.Load(@".\TestData\PS2JP\1_LM1", SystemType.PS2JP);
            Console.WriteLine("Save name: " + gta3Save.SimpleVars.LastMissionPassedName);
            Console.WriteLine("MAIN size: " + gta3Save.Scripts.MainScriptSize);
            Console.WriteLine("Camera pos: " + gta3Save.SimpleVars.CameraPosition);
            Console.WriteLine("Global timer: " + gta3Save.SimpleVars.LastClockTick);
            Console.WriteLine("Weather timer: " + gta3Save.SimpleVars.TimeInMilliseconds);
            Console.WriteLine("Game time: {0}:{1}", gta3Save.SimpleVars.GameClockHours, gta3Save.SimpleVars.GameClockMinutes);
            Console.WriteLine("Current weather: " + gta3Save.SimpleVars.NewWeatherType);
            Console.ReadKey();
            gta3Save.Store(homeDir + @"\Documents\GTA3 User Files\apitest.b");
        }

        private static void BuildingSwaps_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            Console.WriteLine("Item changed!");
        }

        private static void BuildingSwaps_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("Collection changed!");
        }
    }
}
