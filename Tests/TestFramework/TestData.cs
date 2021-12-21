using GTASaveData;
using System;
using System.IO;

namespace TestFramework
{
    public static class TestData
    {
        public static string GetTestDataPath(Game game, FileFormat fileFormat, string fileName)
        {
            string asmPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            int cutIndex = asmPath.IndexOf(nameof(GTASaveData)) + nameof(GTASaveData).Length;
            string basePath = asmPath.Substring(0, cutIndex);
            string filePath = Path.GetFullPath(@$"{basePath}\Tests\TestData\{game}\{fileFormat.Id}\{fileName}");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            return filePath;
        }
    }
}
