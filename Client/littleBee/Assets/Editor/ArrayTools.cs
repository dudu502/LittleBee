using System;
using UnityEditor.MemoryProfiler;

namespace MemoryProfilerWindow
{
    static class ArrayTools
    {
        public static int ReadArrayLength(MemorySection[] heap, UInt64 address, TypeDescription arrayType, VirtualMachineInformation virtualMachineInformation)
        {
            var bo = heap.Find(address, virtualMachineInformation);

            var bounds = bo.Add(virtualMachineInformation.arrayBoundsOffsetInHeader).ReadPointer();

            if (bounds == 0)
#if UNITY_2017_2_OR_NEWER
                return (int)bo.Add(virtualMachineInformation.arraySizeOffsetInHeader).ReadPointer();
#else
                return bo.Add(virtualMachineInformation.arraySizeOffsetInHeader).ReadInt32();
#endif

            var cursor = heap.Find(bounds, virtualMachineInformation);
            int length = 1;
            for (int i = 0; i != arrayType.arrayRank; i++)
            {
#if UNITY_2017_2_OR_NEWER
                length *= (int)cursor.ReadPointer();
                cursor = cursor.Add(virtualMachineInformation.pointerSize == 4 ? 8 : 16);
#else
                length *= cursor.ReadInt32();
                cursor = cursor.Add(8);
#endif
            }
            return length;
        }

        public static int ReadArrayObjectSizeInBytes(MemorySection[] heap, UInt64 address, TypeDescription arrayType, TypeDescription[] typeDescriptions, VirtualMachineInformation virtualMachineInformation)
        {
            var arrayLength = ArrayTools.ReadArrayLength(heap, address, arrayType, virtualMachineInformation);
            var elementType = typeDescriptions[arrayType.baseOrElementTypeIndex];
            var elementSize = elementType.isValueType ? elementType.size : virtualMachineInformation.pointerSize;
            return virtualMachineInformation.arrayHeaderSize + elementSize * arrayLength;
        }
    }
}
