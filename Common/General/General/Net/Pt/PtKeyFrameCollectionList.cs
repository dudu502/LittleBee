//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/9/12 14:42:01
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtKeyFrameCollectionList
{
    public byte __tag__ { get;private set;}

	public int CurrentFrameIndex{ get;private set;}
	public List<PtKeyFrameCollection> Elements{ get;private set;}
	   
    public PtKeyFrameCollectionList SetCurrentFrameIndex(int value){CurrentFrameIndex=value; __tag__|=1<<0; return this;}
	public PtKeyFrameCollectionList SetElements(List<PtKeyFrameCollection> value){Elements=value; __tag__|=1<<1; return this;}
	
    public bool HasCurrentFrameIndex(){return (__tag__&(1<<0))!=0;}
	public bool HasElements(){return (__tag__&(1<<1))!=0;}
	
    public static byte[] Write(PtKeyFrameCollectionList data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasCurrentFrameIndex())buffer.WriteInt32(data.CurrentFrameIndex);
			if(data.HasElements())buffer.WriteCollection(data.Elements,(element)=>PtKeyFrameCollection.Write(element));
			
            return buffer.Getbuffer();
        }
    }

    public static PtKeyFrameCollectionList Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtKeyFrameCollectionList data = new PtKeyFrameCollectionList();
            data.__tag__ = buffer.ReadByte();
			if(data.HasCurrentFrameIndex())data.CurrentFrameIndex = buffer.ReadInt32();
			if(data.HasElements())data.Elements = buffer.ReadCollection( (rBytes)=>PtKeyFrameCollection.Read(rBytes) );
			
            return data;
        }       
    }
}
}
