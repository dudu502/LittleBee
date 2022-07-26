
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtSearchHostResult
{
    public string connectKey;
	public int hashCode;
	    
    public static byte[] Write(PtSearchHostResult data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteString(data.connectKey);
			buffer.WriteInt32(data.hashCode);
			
            return buffer.Getbuffer();
        }
    }
    public static PtSearchHostResult Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtSearchHostResult data = new PtSearchHostResult();
            data.connectKey = buffer.ReadString();
			data.hashCode = buffer.ReadInt32();
			
            return data;
        }       
    }
}
}
