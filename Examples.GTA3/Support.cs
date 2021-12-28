using GTASaveData;
using GTASaveData.GTA3;
using System;

namespace Examples.GTA3
{
    public static class Support
    {
        public static string GetUserFilesDir()
        {
            string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return $@"{myDocs}\GTA3 User Files";
        }

        public static string GetSaveSlot(int slotNum)
        {
            string userFiles = GetUserFilesDir();

            return slotNum >= 1 && slotNum <= 8
                ? $@"{userFiles}\GTA3sf{slotNum}.b"
                : throw new ArgumentOutOfRangeException(nameof(slotNum), "Slot number cannot be less than 1 or greater than 8.");
        }

        public static string GetFileFromTestData(string name, FileType platform)
        {
            return TestFramework.TestData.GetTestDataPath(Game.GTA3, platform, name);
        }
    }
}
