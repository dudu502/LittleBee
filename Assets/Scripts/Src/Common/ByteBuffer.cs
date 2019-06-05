using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;

public class ByteBuffer : IDisposable
{
    //private byte[] bytes = new byte[0];
    //private int position = 0;
    //public ByteBuffer(byte[] value)
    //{
    //    source = value;
    //}
    //public ByteBuffer()
    //{

    //}

    //public byte[] Getbuffer() { return source; }
    //public void ResetPosition()
    //{
    //    position = 0;
    //}
    //public int GetPosition() { return position; }
    //public byte[] source
    //{
    //    get { return bytes; }
    //    set
    //    {
    //        bytes = value;
    //        ResetPosition();
    //    }
    //}
    //public ByteBuffer WriteInt32(int value)
    //{
    //    copy(BitConverter.GetBytes(value));
    //    return this;
    //}

    //public int ReadInt32()
    //{
    //    return BitConverter.ToInt32(get(4), 0);
    //}

    //public ByteBuffer WriteShort(short value)
    //{
    //    copy(BitConverter.GetBytes(value));
    //    return this;
    //}
    //public short ReadShort()
    //{
    //    return BitConverter.ToInt16(get(2), 0);
    //}

    //public ByteBuffer WriteString(string value)
    //{
    //    byte[] data = Encoding.UTF8.GetBytes(value);
    //    WriteInt32(data.Length);
    //    copy(data);
    //    return this;
    //}
    //public string ReadString()
    //{
    //    return Encoding.UTF8.GetString(get(ReadInt32()));
    //}

    //public ByteBuffer WriteBytes(byte[] value)
    //{
    //    WriteInt32(value.Length);
    //    copy(value);
    //    return this;
    //}
    //public byte[] ReadBytes()
    //{
    //    return get(ReadInt32());
    //}


    //public ByteBuffer WriteByte(byte value)
    //{
    //    copy(new byte[] { value });
    //    return this;
    //}
    //public byte ReadByte()
    //{
    //    return get(1)[0];
    //}
    //public ByteBuffer WriteBool(bool value)
    //{
    //    WriteByte(Convert.ToByte(value));
    //    return this;
    //}
    //public bool ReadBool()
    //{
    //    return Convert.ToBoolean(ReadByte());
    //}
    //public ByteBuffer WriteLong(long value)
    //{
    //    copy(BitConverter.GetBytes(value));
    //    return this;
    //}
    //public long ReadLong()
    //{
    //    return BitConverter.ToInt32(get(8), 0);
    //}

    //public ByteBuffer WriteFloat(float value)
    //{
    //    copy(BitConverter.GetBytes(value));
    //    return this;
    //}
    //public float ReadFloat()
    //{
    //    return BitConverter.ToSingle(get(4), 0);
    //}

    //private void copy(byte[] value)
    //{
    //    byte[] temps = new byte[bytes.Length + value.Length];
    //    Buffer.BlockCopy(bytes, 0, temps, 0, bytes.Length);
    //    Buffer.BlockCopy(value, 0, temps, position, value.Length);


    //    position += value.Length;
    //    bytes = temps;
    //    temps = null;
    //}
    //private byte[] get(int length)
    //{
    //    byte[] data = new byte[length];
    //    Buffer.BlockCopy(bytes, position, data, 0, length);
    //    position += length;
    //    return data;
    //}
    //public void Dispose()
    //{
    //    position = 0;
    //    bytes = null;
    //}

    BinaryWriter bw;
    BinaryReader br;
    MemoryStream ms = new MemoryStream();
    BinaryWriter Writer
    {
        get
        {
            if (bw == null) bw = new BinaryWriter(ms);
            return bw;
        }
    }
    BinaryReader Reader
    {
        get
        {
            if (br == null) br = new BinaryReader(ms);
            return br;
        }
    }

    public ByteBuffer(byte[] bytes)
    {
        Writer.Write(bytes);
        ms.Position = 0;
    }
    public ByteBuffer()
    {

    }

    public byte[] Getbuffer()
    {
        return ms.GetBuffer();
    }
    public ByteBuffer WriteInt32(int value)
    {
        Writer.Write(value);
        return this;
    }
    public ByteBuffer WriteByte(byte value)
    {
        Writer.Write(value);
        return this;
    }
    public byte ReadByte()
    {
        return Reader.ReadByte();
    }
    public int GetPosition()
    {
        return (int)ms.Position;
    }
    public ByteBuffer WriteBool(bool value)
    {
        Writer.Write(value);
        return this;
    }
    public bool ReadBool()
    {
        return Reader.ReadBoolean();
    }
    public int ReadInt32()
    {
        return Reader.ReadInt32();
    }
    public ByteBuffer WriteLong(long value)
    {
        Writer.Write(value);
        return this;
    }
    public long ReadLong()
    {
        return Reader.ReadInt64();
    }
    public ByteBuffer WriteFloat(float value)
    {
        Writer.Write(value);
        return this;
    }
    public float ReadFloat()
    {
        return Reader.ReadSingle();
    }
    public ByteBuffer WriteShort(short value)
    {
        Writer.Write(value);
        return this;
    }
    public short ReadShort()
    {
        return Reader.ReadInt16();
    }

    public ByteBuffer WriteString(string value)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        Writer.Write((int)bytes.Length);
        Writer.Write(bytes);

        return this;
    }
    public string ReadString()
    {
        int len = ReadInt32();
        byte[] buffer = new byte[len];
        buffer = Reader.ReadBytes(len);
        return Encoding.UTF8.GetString(buffer);
    }

    public ByteBuffer WriteBytes(byte[] value)
    {
        WriteInt32(value.Length);
        Writer.Write(value);
        return this;
    }
    public byte[] ReadBytes()
    {
        int len = ReadInt32();
        return Reader.ReadBytes(len);
    }


    public void Dispose()
    {
        ms.Close();
        ms.Dispose();
        ms = null;
        if (bw != null)
        {
            bw.Close();
            bw.Dispose();
            bw = null;
        }
        if (br != null)
        {
            br.Close();
            br.Dispose();
            bw = null;
        }
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

