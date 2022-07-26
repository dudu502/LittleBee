//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/10/9 18:09:54
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtHeartBeat
{
    public byte __tag__ { get;private set;}

	public long Ts{ get;private set;}
	   
    public PtHeartBeat SetTs(long value){Ts=value; __tag__|=1<<0; return this;}
	
    public bool HasTs(){return (__tag__&(1<<0))!=0;}
	
    public static byte[] Write(PtHeartBeat data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasTs())buffer.WriteInt64(data.Ts);
			
            return buffer.Getbuffer();
        }
    }

    public static PtHeartBeat Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtHeartBeat data = new PtHeartBeat();
            data.__tag__ = buffer.ReadByte();
			if(data.HasTs())data.Ts = buffer.ReadInt64();
			
            return data;
        }       
    }
}
}
