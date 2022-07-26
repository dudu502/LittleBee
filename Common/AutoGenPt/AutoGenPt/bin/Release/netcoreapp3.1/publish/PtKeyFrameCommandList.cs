//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/10/9 18:09:54
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtKeyFrameCommandList
{
    public byte __tag__ { get;private set;}

	public int FrameIndex{ get;private set;}
	public List<PtKeyFrameCommand> Commands{ get;private set;}
	   
    public PtKeyFrameCommandList SetFrameIndex(int value){FrameIndex=value; __tag__|=1<<0; return this;}
	public PtKeyFrameCommandList SetCommands(List<PtKeyFrameCommand> value){Commands=value; __tag__|=1<<1; return this;}
	
    public bool HasFrameIndex(){return (__tag__&(1<<0))!=0;}
	public bool HasCommands(){return (__tag__&(1<<1))!=0;}
	
    public static byte[] Write(PtKeyFrameCommandList data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasFrameIndex())buffer.WriteInt32(data.FrameIndex);
			if(data.HasCommands())buffer.WriteComplexCollection(data.Commands,(element)=>PtKeyFrameCommand.Write(element));
			
            return buffer.Getbuffer();
        }
    }

    public static PtKeyFrameCommandList Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtKeyFrameCommandList data = new PtKeyFrameCommandList();
            data.__tag__ = buffer.ReadByte();
			if(data.HasFrameIndex())data.FrameIndex = buffer.ReadInt32();
			if(data.HasCommands())data.Commands = buffer.ReadComplexCollection( (rBytes)=>PtKeyFrameCommand.Read(rBytes) );
			
            return data;
        }       
    }
}
}
