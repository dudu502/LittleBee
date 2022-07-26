using System;
using UnityEditor.MemoryProfiler;

namespace MemoryProfilerWindow
{
    static class ManagedHeapExtensions
    {
        public static BytesAndOffset Find(this MemorySection[] heap, UInt64 address, VirtualMachineInformation virtualMachineInformation)
        {
            foreach (var segment in heap)
                if (address >= segment.startAddress && address < (segment.startAddress + (ulong)segment.bytes.Length))
                    return new BytesAndOffset() { bytes = segment.bytes, offset = (int)(address - segment.startAddress), pointerSize = virtualMachineInformation.pointerSize };

            return new BytesAndOffset();
        }
    }
}
