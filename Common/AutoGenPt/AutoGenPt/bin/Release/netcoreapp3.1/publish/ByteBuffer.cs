using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;

public class ByteBuffer : IDisposable
{
    public MemoryStream ms;
    private readonly BinaryReader br;
    private readonly BinaryWriter bw;
    public void Dispose()
    {
        ms.Dispose();
        if (br != null) br.Dispose();
        if (bw != null) bw.Dispose();
    }
    public ByteBuffer()
    {
        ms = new MemoryStream(32);
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
        Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, result.Length);
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
        byte[] data = Encoding.UTF8.GetBytes(value);
        WriteBytes(data);
        return this;
    }
    public string ReadString()
    {
        byte[] data = ReadBytes();
        return Encoding.UTF8.GetString(data);
    }
    public ByteBuffer WriteBytes(byte[] value)
    {
        int byteCount = value.Length;
        if(byteCount<byte.MaxValue)
        {
            WriteByte(0);
            WriteByte((byte)byteCount);
        }
        else if(byteCount<ushort.MaxValue)
        {
            WriteByte(1);
            WriteUInt16((ushort)byteCount);
        }
        else
        {
            throw new Exception($"The Length of bytes must be less than [{ushort.MaxValue}],current length is "+byteCount);
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
        else
            byteCount = ReadUInt16();
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
        WriteByte(Convert.ToByte(value));
        return this;
    }
    public bool ReadBool()
    {
        return Convert.ToBoolean(ReadByte());
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

    public ByteBuffer WriteComplexCollection<T>(System.Collections.Generic.List<T> collection, Func<T, byte[]> forEach)
    {
        int count = collection.Count;
        WriteInt32(count);
        for (int i = 0; i < count; ++i)
            WriteBytes(forEach(collection[i]));
        return this;
    }
    public ByteBuffer WriteSimpleCollection<T>(System.Collections.Generic.List<T> collection, Action<T> forEach)
    {
        int count = collection.Count;
        WriteInt32(count);
        for (int i = 0; i < count; ++i)
            forEach(collection[i]);
        return this;
    }
    public System.Collections.Generic.List<T> ReadSimpleCollection<T>(Func<T> forEach)
    {
        System.Collections.Generic.List<T> collection = new System.Collections.Generic.List<T>();
        int count = ReadInt32();
        for (int i = 0; i < count; ++i)
            collection.Add(forEach());
        return collection;
    }
    public System.Collections.Generic.List<T> ReadComplexCollection<T>(Func<byte[], T> forEach)
    {
        System.Collections.Generic.List<T> collection = new System.Collections.Generic.List<T>();
        int count = ReadInt32();
        for (int i = 0; i < count; ++i)
            collection.Add(forEach(ReadBytes()));
        return collection;
    }

    //压缩字节
    //1.创建压缩的数据流 
    //2.设定compressStream为存放被压缩的文件流,并设定为压缩模式
    //3.将需要压缩的字节写到被压缩的文件流
    public static byte[] CompressBytes(byte[] bytes)
    {
        using (MemoryStream compressStream = new MemoryStream())
        {
            using (var zipStream = new GZipStream(compressStream, CompressionMode.Compress))
                zipStream.Write(bytes, 0, bytes.Length);
            return compressStream.ToArray();
        }
    }
    //解压缩字节
    //1.创建被压缩的数据流
    //2.创建zipStream对象，并传入解压的文件流
    //3.创建目标流
    //4.zipStream拷贝到目标流
    //5.返回目标流输出字节
    public static byte[] Decompress(byte[] bytes)
    {
        using (var compressStream = new MemoryStream(bytes))
        {
            using (var zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
            {
                using (var resultStream = new MemoryStream())
                {
                    zipStream.CopyTo(resultStream);
                    return resultStream.ToArray();
                }
            }
        }
    }


}

