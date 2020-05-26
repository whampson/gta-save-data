using GTASaveData;
using System;
using System.Collections.Generic;

namespace TestApp
{
    public class FileTypeListEventArgs : EventArgs
    {
        public IEnumerable<FileFormat> FileTypes { get; }

        public FileTypeListEventArgs(IEnumerable<FileFormat> fileTypes)
        {
            FileTypes = fileTypes;
        }
    }
}
