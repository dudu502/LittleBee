//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/8/21 16:54:31
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtUInt32List
{
    public byte __tag__ { get;private set;}

	public List<uint> Elements{ get;private set;}
	   
    public PtUInt32List SetElements(List<uint> value){Elements=value; __tag__|=1<<0; return this;}
	
    public bool HasElements(){return (__tag__&(1<<0))!=0;}
	
    public static byte[] Write(PtUInt32List data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasElements())buffer.WriteCollection(data.Elements,(element)=>buffer.WriteUInt32(element));
			
            return buffer.GetRawBytes();
        }
    }

    public static PtUInt32List Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtUInt32List data = new PtUInt32List();
            data.__tag__ = buffer.ReadByte();
			if(data.HasElements())data.Elements = buffer.ReadCollection( ()=> buffer.ReadUInt32());
			
            return data;
        }       
    }
}
}
