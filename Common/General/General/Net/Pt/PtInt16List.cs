//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/8/21 16:54:31
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtInt16List
{
    public byte __tag__ { get;private set;}

	public List<short> Elements{ get;private set;}
	   
    public PtInt16List SetElements(List<short> value){Elements=value; __tag__|=1<<0; return this;}
	
    public bool HasElements(){return (__tag__&(1<<0))!=0;}
	
    public static byte[] Write(PtInt16List data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasElements())buffer.WriteCollection(data.Elements,(element)=>buffer.WriteInt16(element));
			
            return buffer.GetRawBytes();
        }
    }

    public static PtInt16List Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtInt16List data = new PtInt16List();
            data.__tag__ = buffer.ReadByte();
			if(data.HasElements())data.Elements = buffer.ReadCollection( ()=> buffer.ReadInt16());
			
            return data;
        }       
    }
}
}
