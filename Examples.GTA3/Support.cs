using GTASaveData;
using GTASaveData.GTA3;
using System;
using System.IO;
using System.Linq;

namespace Examples.GTA3
{
    public static class Support
    {
        public static string GetSaveDir()
        {
            string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return $@"{myDocs}\GTA3 User Files";
        }

        public static string GetSaveDirDE()
        {
            string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string profiles = $@"{myDocs}\Rockstar Games\GTA III Definitive Edition\Profiles";
            return Directory.EnumerateDirectories(profiles).FirstOrDefault();
        }

        public static string GetSaveSlot(int slotNum)
        {
            string userFiles = GetSaveDir();

            return slotNum >= 1 && slotNum <= 8
                ? $@"{userFiles}\GTA3sf{slotNum}.b"
                : throw new ArgumentOutOfRangeException(nameof(slotNum), "Slot number cannot be less than 1 or greater than 8.");
        }

        public static string GetSaveSlotDE(int slotNum)
        {
            string saveDir = GetSaveDirDE();

            return slotNum >= 1 && slotNum <= 9
                ? $@"{saveDir}\GTA3sf{slotNum}.sav"
                : throw new ArgumentOutOfRangeException(nameof(slotNum), "Slot number cannot be less than 1 or greater than 9.");
        }

        public static string GetFileFromTestData(string name, FileType platform)
        {
            return TestFramework.TestData.GetTestDataPath(Game.GTA3, platform, name);
        }
    }
}
