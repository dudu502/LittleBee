//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/10/9 18:09:54
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtSearchHostResult
{
    public byte __tag__ { get;private set;}

	public string connectKey{ get;private set;}
	public int hashCode{ get;private set;}
	   
    public PtSearchHostResult SetconnectKey(string value){connectKey=value; __tag__|=1<<0; return this;}
	public PtSearchHostResult SethashCode(int value){hashCode=value; __tag__|=1<<1; return this;}
	
    public bool HasconnectKey(){return (__tag__&(1<<0))!=0;}
	public bool HashashCode(){return (__tag__&(1<<1))!=0;}
	
    public static byte[] Write(PtSearchHostResult data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasconnectKey())buffer.WriteString(data.connectKey);
			if(data.HashashCode())buffer.WriteInt32(data.hashCode);
			
            return buffer.Getbuffer();
        }
    }

    public static PtSearchHostResult Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtSearchHostResult data = new PtSearchHostResult();
            data.__tag__ = buffer.ReadByte();
			if(data.HasconnectKey())data.connectKey = buffer.ReadString();
			if(data.HashashCode())data.hashCode = buffer.ReadInt32();
			
            return data;
        }       
    }
}
}
