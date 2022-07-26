using System;

namespace MemoryProfilerWindow
{
    [Serializable]
    public class PackedManagedObject
    {
        public UInt64 address;
        public int typeIndex;
        public int size;
    }
}
