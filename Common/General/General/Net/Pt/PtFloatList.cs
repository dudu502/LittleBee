//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/11/30 17:37:32
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtFloatList
{
    public byte __tag__ { get;private set;}

	public List<float> Elements{ get;private set;}
	   
    public PtFloatList SetElements(List<float> value){Elements=value; __tag__|=1; return this;}
	
    public bool HasElements(){return (__tag__&1)==1;}
	
    public static byte[] Write(PtFloatList data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasElements())buffer.WriteCollection(data.Elements,(element)=>buffer.WriteFloat(element));
			
            return buffer.Getbuffer();
        }
    }

    public static PtFloatList Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtFloatList data = new PtFloatList();
            data.__tag__ = buffer.ReadByte();
			if(data.HasElements())data.Elements = buffer.ReadCollection( ()=> buffer.ReadFloat());
			
            return data;
        }       
    }
}
}
