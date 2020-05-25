using GTASaveData;
using System;
using System.IO;

namespace TestFramework
{
    public static class TestData
    {
        public static string GetTestDataPath(GameType game, SaveDataFormat fileFormat, string fileName)
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../..");
            string filePath = string.Format("TestData/{0}/{1}/{2}", game, fileFormat.FormatId, fileName);
            
            return Path.GetFullPath(Path.Combine(basePath, filePath));
        }
    }
}
