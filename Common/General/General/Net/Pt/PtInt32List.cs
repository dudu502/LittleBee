//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/8/21 16:54:31
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtInt32List
{
    public byte __tag__ { get;private set;}

	public List<int> Elements{ get;private set;}
	   
    public PtInt32List SetElements(List<int> value){Elements=value; __tag__|=1<<0; return this;}
	
    public bool HasElements(){return (__tag__&(1<<0))!=0;}
	
    public static byte[] Write(PtInt32List data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasElements())buffer.WriteCollection(data.Elements,(element)=>buffer.WriteInt32(element));
			
            return buffer.GetRawBytes();
        }
    }

    public static PtInt32List Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtInt32List data = new PtInt32List();
            data.__tag__ = buffer.ReadByte();
			if(data.HasElements())data.Elements = buffer.ReadCollection( ()=> buffer.ReadInt32());
			
            return data;
        }       
    }
}
}
