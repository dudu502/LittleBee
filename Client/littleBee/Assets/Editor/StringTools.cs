using System;
using UnityEditor.MemoryProfiler;

namespace MemoryProfilerWindow
{
    static class StringTools
    {
        public static string ReadString(BytesAndOffset bo, VirtualMachineInformation virtualMachineInformation)
        {
            var lengthPointer = bo.Add(virtualMachineInformation.objectHeaderSize);
            var length = lengthPointer.ReadInt32();
            var firstChar = lengthPointer.Add(4);

            return System.Text.Encoding.Unicode.GetString(firstChar.bytes, firstChar.offset, length * 2);
        }

        public static int ReadStringObjectSizeInBytes(BytesAndOffset bo, VirtualMachineInformation virtualMachineInformation)
        {
            var lengthPointer = bo.Add(virtualMachineInformation.objectHeaderSize);
            var length = lengthPointer.ReadInt32();

            return virtualMachineInformation.objectHeaderSize + /*lengthfield*/ 1 + (length * /*utf16=2bytes per char*/ 2) + /*2 zero terminators*/ 2;
        }
    }
}
