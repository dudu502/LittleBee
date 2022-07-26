//the idea is to only expose the very very lowest level data in the unity api. this data is uncrawled.
//you get the raw heap bytes, you get typedescriptions, and an overview of native objects, and that's it.
//
//the crawler, the higher level "c# idiomy api", and the UI window that uses that api
//we can develop in an opensource github project, and iterate
//on outside of the unity release trains.

/*
using System;

namespace UnityEditor.Profiler.Memory
{
    [Serializable] //note: this snapshot is completely serializable by unity's serializer.
    public class PackedMemorySnapshot
    {
        public PackedNativeUnityEngineObject[] nativeObjects;
        public PackedGCHandle[] gcHandles;
        public Connection[] connections;

        public ManagedHeap managedHeap;
        public TypeDescription[] typeDescriptions;
        public string[] classIDNames;
    }

    [Serializable]
    public struct PackedNativeUnityEngineObject
    {
        public string name;
        public int instanceID;
        public int size;
        public int classID;
    }

    [Serializable]
    public struct PackedGCHandle
    {
        public UInt64 target;
    }

    [Serializable]
    public struct Connection
    {
        //these indices index into an imaginary array that is the concatenation of snapshot.nativeObject + snapshot.gcHandles snapshot.
        public int from;
        public int to;
    }

    [Serializable]
    public class ManagedHeap
    {
        public HeapSegment[] segments;
        public VirtualMachineInformation virtualMachineInformation;
    }

    [Serializable]
    public class HeapSegment
    {
        public byte[] bytes;
        public UInt64 startAddress;
    }

    [Serializable]
    public struct VirtualMachineInformation
    {
        public int pointerSize;
        public int objectHeaderSize;
        public int arrayHeaderSize;
        public int arrayBoundsOffsetInHeader;
        public int arraySizeOffsetInHeader;
        public int allocationGranularity;
    };

    [Serializable]
    public class TypeDescription
    {
        public string name;
        public string fullname;
        public int @namespace;
        public int assembly;
        public FieldDescription[] fields;
        public byte[] staticFieldBytes;
        public int baseOrElementTypeIndex;
        public int size;
        public UInt64 typeInfoAddress;
        public int typeIndex;

        public bool IsValueType
        {
            get { return (flags & TypeFlags.kValueType) != 0; }
        }

        public bool IsArray
        {
            get { return (flags & TypeFlags.kArray) != 0; }
        }

        public int ArrayRank
        {
            get { return (int) (flags & TypeFlags.kArrayRankMask) >> 16; }
        }

        private TypeFlags flags;

        private enum TypeFlags
        {
            kNone = 0,
            kValueType = 1 << 0,
            kArray = 1 << 1,
            kArrayRankMask = unchecked((int) 0xFFFF0000)
        };
    }

    [Serializable]
    public class FieldDescription
    {
        public string name;
        public int offset;
        public int typeIndex;
        public bool isStatic;
    }
}*/
