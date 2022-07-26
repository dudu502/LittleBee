//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/10/9 18:09:54
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtGameInfo
{
    public byte __tag__ { get;private set;}

	public uint MapId{ get;private set;}
	public uint RoomId{ get;private set;}
	public List<PtEntity> EntityList{ get;private set;}
	   
    public PtGameInfo SetMapId(uint value){MapId=value; __tag__|=1<<0; return this;}
	public PtGameInfo SetRoomId(uint value){RoomId=value; __tag__|=1<<1; return this;}
	public PtGameInfo SetEntityList(List<PtEntity> value){EntityList=value; __tag__|=1<<2; return this;}
	
    public bool HasMapId(){return (__tag__&(1<<0))!=0;}
	public bool HasRoomId(){return (__tag__&(1<<1))!=0;}
	public bool HasEntityList(){return (__tag__&(1<<2))!=0;}
	
    public static byte[] Write(PtGameInfo data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasMapId())buffer.WriteUInt32(data.MapId);
			if(data.HasRoomId())buffer.WriteUInt32(data.RoomId);
			if(data.HasEntityList())buffer.WriteComplexCollection(data.EntityList,(element)=>PtEntity.Write(element));
			
            return buffer.Getbuffer();
        }
    }

    public static PtGameInfo Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtGameInfo data = new PtGameInfo();
            data.__tag__ = buffer.ReadByte();
			if(data.HasMapId())data.MapId = buffer.ReadUInt32();
			if(data.HasRoomId())data.RoomId = buffer.ReadUInt32();
			if(data.HasEntityList())data.EntityList = buffer.ReadComplexCollection( (rBytes)=>PtEntity.Read(rBytes) );
			
            return data;
        }       
    }
}
}
