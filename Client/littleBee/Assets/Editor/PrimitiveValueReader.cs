using System;
using UnityEditor.MemoryProfiler;
using UnityEngine;

namespace MemoryProfilerWindow
{
    class PrimitiveValueReader
    {
        private readonly VirtualMachineInformation _virtualMachineInformation;
        private readonly MemorySection[] _heapSections;

        public PrimitiveValueReader(VirtualMachineInformation virtualMachineInformation, MemorySection[] heapSections)
        {
            _virtualMachineInformation = virtualMachineInformation;
            _heapSections = heapSections;
        }

        public System.Int32 ReadInt32(BytesAndOffset bo)
        {
            return BitConverter.ToInt32(bo.bytes, bo.offset);
        }

        public System.UInt32 ReadUInt32(BytesAndOffset bo)
        {
            return BitConverter.ToUInt32(bo.bytes, bo.offset);
        }

        public System.Int64 ReadInt64(BytesAndOffset bo)
        {
            return BitConverter.ToInt64(bo.bytes, bo.offset);
        }

        public System.UInt64 ReadUInt64(BytesAndOffset bo)
        {
            return BitConverter.ToUInt64(bo.bytes, bo.offset);
        }

        public System.Int16 ReadInt16(BytesAndOffset bo)
        {
            return BitConverter.ToInt16(bo.bytes, bo.offset);
        }

        public System.UInt16 ReadUInt16(BytesAndOffset bo)
        {
            return BitConverter.ToUInt16(bo.bytes, bo.offset);
        }

        public System.Byte ReadByte(BytesAndOffset bo)
        {
            return bo.bytes[bo.offset];
        }

        public System.SByte ReadSByte(BytesAndOffset bo)
        {
            return (System.SByte)bo.bytes[bo.offset];
        }

        public System.Boolean ReadBool(BytesAndOffset bo)
        {
            return ReadByte(bo) != 0;
        }

        public UInt64 ReadPointer(BytesAndOffset bo)
        {
            if (_virtualMachineInformation.pointerSize == 4)
                return ReadUInt32(bo);
            else
                return ReadUInt64(bo);
        }

        public UInt64 ReadPointer(UInt64 address)
        {
            return ReadPointer(_heapSections.Find(address, _virtualMachineInformation));
        }

        public Char ReadChar(BytesAndOffset bytesAndOffset)
        {
            return System.Text.Encoding.Unicode.GetChars(bytesAndOffset.bytes, bytesAndOffset.offset, 2)[0];
        }

        public System.Single ReadSingle(BytesAndOffset bytesAndOffset)
        {
            return BitConverter.ToSingle(bytesAndOffset.bytes, bytesAndOffset.offset);
        }

        public System.Double ReadDouble(BytesAndOffset bytesAndOffset)
        {
            return BitConverter.ToDouble(bytesAndOffset.bytes, bytesAndOffset.offset);
        }
    }
}
