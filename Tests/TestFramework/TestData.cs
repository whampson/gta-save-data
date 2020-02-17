using GTASaveData;
using GTASaveData.Serialization;
using System;
using System.IO;

namespace TestFramework
{
    public static class TestData
    {
        public static string GetTestDataPath(Game game, FileFormat fileFormat, string fileName)
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../..");
            string filePath = string.Format("TestData/{0}/{1}/{2}", game, fileFormat.Name, fileName);
            
            return Path.GetFullPath(Path.Combine(basePath, filePath));
        }
    }
}
