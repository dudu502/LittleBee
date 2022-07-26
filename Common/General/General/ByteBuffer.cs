using System;
using System.IO;

public class ByteBuffer : IDisposable
{
    private MemoryStream ms;
    private readonly BinaryReader br;
    private readonly BinaryWriter bw;
    public void Dispose()
    {
        ms.Dispose();
        if (br != null) br.Dispose();
        if (bw != null) bw.Dispose();
    }
    public ByteBuffer(int capacity = 32)
    {
        ms = new MemoryStream(capacity);
        bw = new BinaryWriter(ms);
    }
    public ByteBuffer(byte[] bytes)
    {
        ms = new MemoryStream(bytes);
        br = new BinaryReader(ms);
    }
    public byte[] Getbuffer()
    {
        byte[] result = new byte[ms.Length];
        //Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, result.Length);
        ms.Seek(0, SeekOrigin.Begin);
        ms.Read(result, 0, result.Length);
        return result;
    }

    public ByteBuffer WriteInt16(short value)
    {
        bw.Write(value);
        return this;
    }
    public short ReadInt16()
    {
        return br.ReadInt16();
    }
    public ByteBuffer WriteInt32(int value)
    {
        bw.Write(value);
        return this;
    }
    public int ReadInt32()
    {
        return br.ReadInt32();
    }
    public ByteBuffer WriteUInt32(uint value)
    {
        bw.Write(value);
        return this;
    }
    public uint ReadUInt32()
    {
        return br.ReadUInt32();
    }
    public ByteBuffer WriteUInt16(ushort value)
    {
        bw.Write(value);
        return this;
    }
    public ushort ReadUInt16()
    {
        return br.ReadUInt16();
    }
    public ByteBuffer WriteString(string value)
    {
        WriteBytes(System.Text.Encoding.UTF8.GetBytes(value));
        return this;
    }
    public string ReadString()
    {
        return System.Text.Encoding.UTF8.GetString(ReadBytes());
    }
    public ByteBuffer WriteBytes(byte[] value)
    {
        int byteCount = value.Length;
        if(byteCount<=byte.MaxValue)
        {
            WriteByte(0);
            WriteByte((byte)byteCount);
        }
        else if(byteCount<=ushort.MaxValue)
        {
            WriteByte(1);
            WriteUInt16((ushort)byteCount);
        }
        else if(byteCount<= int.MaxValue)
        {
            WriteByte(2);
            WriteInt32(byteCount);
        }
        else
        {
            throw new Exception($"The Length of bytes must be less than [{int.MaxValue}],current length is "+byteCount);
        }
        bw.Write(value);
        return this;
    }
    public byte[] ReadBytes()
    {
        int byteCount = 0;
        byte type = ReadByte();
        if (type == 0)
            byteCount = ReadByte();
        else if (type == 1)
            byteCount = ReadUInt16();
        else if (type == 2)
            byteCount = ReadInt32();
        else
            throw new Exception($"The Length of bytes must be less than [{int.MaxValue}],current length is " + byteCount);
        return br.ReadBytes(byteCount);
    }
    public ByteBuffer WriteByte(byte value)
    {
        bw.Write(value);
        return this;
    }
    public byte ReadByte()
    {
        return br.ReadByte();
    }

    public ByteBuffer WriteBool(bool value)
    {
        bw.Write(value);
        return this;
    }
    public bool ReadBool()
    {
        return br.ReadBoolean();
    }
    public ByteBuffer WriteInt64(long value)
    {
        bw.Write(value);
        return this;
    }
    public long ReadInt64()
    {
        return br.ReadInt64();
    }
    public ByteBuffer WriteUInt64(ulong value)
    {
        bw.Write(value);
        return this;
    }
    public ulong ReadUInt64()
    {
        return br.ReadUInt64();
    }
    public ByteBuffer WriteFloat(float value)
    {
        bw.Write(value);
        return this;
    }
    public float ReadFloat()
    {
        return br.ReadSingle();
    }
    public ByteBuffer WriteDouble(double value)
    {
        bw.Write(value);
        return this;
    }

    public double ReadDouble()
    {
        return br.ReadDouble();
    }
    public ByteBuffer WriteCollection<T>(System.Collections.Generic.List<T> collection, Func<T, byte[]> func)
    {
        int count = collection.Count;
        WriteInt32(count);
        for (int i = 0; i < count; ++i)
            WriteBytes(func(collection[i]));
        return this;
    }
    public ByteBuffer WriteCollection<T>(System.Collections.Generic.List<T> collection, Action<T> action)
    {
        int count = collection.Count;
        WriteInt32(count);
        for (int i = 0; i < count; ++i)
            action(collection[i]);
        return this;
    }
    public System.Collections.Generic.List<T> ReadCollection<T>(Func<T> func)
    {
        System.Collections.Generic.List<T> collection = new System.Collections.Generic.List<T>();
        int count = ReadInt32();
        for (int i = 0; i < count; ++i)
            collection.Add(func());
        return collection;
    }
    public System.Collections.Generic.List<T> ReadCollection<T>(Func<byte[], T> func)
    {
        System.Collections.Generic.List<T> collection = new System.Collections.Generic.List<T>();
        int count = ReadInt32();
        for (int i = 0; i < count; ++i)
            collection.Add(func(ReadBytes()));
        return collection;
    }  
}

