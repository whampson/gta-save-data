﻿using System;

namespace GTASaveData.Serialization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SizeAttribute : Attribute
    {
        public int Size { get; set; }

        public SizeAttribute(int size)
        {
            Size = size;
        }
    }
}
