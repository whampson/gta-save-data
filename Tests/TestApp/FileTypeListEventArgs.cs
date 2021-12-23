using GTASaveData;
using System;
using System.Collections.Generic;

namespace TestApp
{
    public class FileTypeListEventArgs : EventArgs
    {
        public IEnumerable<FileType> FileTypes { get; }

        public FileTypeListEventArgs(IEnumerable<FileType> fileTypes)
        {
            FileTypes = fileTypes;
        }
    }
}
