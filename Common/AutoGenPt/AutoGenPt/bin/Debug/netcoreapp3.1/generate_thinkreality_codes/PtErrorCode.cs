//Template auto generator:[AutoGenPt] v1.0
//Creation time:2021/6/30 11:58:47
using System;
using System.Collections;
using System.Collections.Generic;
namespace Lenovo.XR.Development.Net.Pt
{
public class PtErrorCode
{
    public byte __tag__ { get;private set;}

	public int Id{ get;private set;}
	   
    public PtErrorCode SetId(int value){Id=value; __tag__|=1; return this;}
	
    public bool HasId(){return (__tag__&1)==1;}
	
    public static byte[] Write(PtErrorCode data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasId())buffer.WriteInt32(data.Id);
			
            return buffer.Getbuffer();
        }
    }

    public static PtErrorCode Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtErrorCode data = new PtErrorCode();
            data.__tag__ = buffer.ReadByte();
			if(data.HasId())data.Id = buffer.ReadInt32();
			
            return data;
        }       
    }
}
}
