//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/10/9 18:09:54
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtRoom
{
    public byte __tag__ { get;private set;}

	public uint RoomId{ get;private set;}
	public byte Status{ get;private set;}
	public uint MapId{ get;private set;}
	public string RoomOwnerUserId{ get;private set;}
	public List<PtRoomPlayer> Players{ get;private set;}
	   
    public PtRoom SetRoomId(uint value){RoomId=value; __tag__|=1<<0; return this;}
	public PtRoom SetStatus(byte value){Status=value; __tag__|=1<<1; return this;}
	public PtRoom SetMapId(uint value){MapId=value; __tag__|=1<<2; return this;}
	public PtRoom SetRoomOwnerUserId(string value){RoomOwnerUserId=value; __tag__|=1<<3; return this;}
	public PtRoom SetPlayers(List<PtRoomPlayer> value){Players=value; __tag__|=1<<4; return this;}
	
    public bool HasRoomId(){return (__tag__&(1<<0))!=0;}
	public bool HasStatus(){return (__tag__&(1<<1))!=0;}
	public bool HasMapId(){return (__tag__&(1<<2))!=0;}
	public bool HasRoomOwnerUserId(){return (__tag__&(1<<3))!=0;}
	public bool HasPlayers(){return (__tag__&(1<<4))!=0;}
	
    public static byte[] Write(PtRoom data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasRoomId())buffer.WriteUInt32(data.RoomId);
			if(data.HasStatus())buffer.WriteByte(data.Status);
			if(data.HasMapId())buffer.WriteUInt32(data.MapId);
			if(data.HasRoomOwnerUserId())buffer.WriteString(data.RoomOwnerUserId);
			if(data.HasPlayers())buffer.WriteComplexCollection(data.Players,(element)=>PtRoomPlayer.Write(element));
			
            return buffer.Getbuffer();
        }
    }

    public static PtRoom Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtRoom data = new PtRoom();
            data.__tag__ = buffer.ReadByte();
			if(data.HasRoomId())data.RoomId = buffer.ReadUInt32();
			if(data.HasStatus())data.Status = buffer.ReadByte();
			if(data.HasMapId())data.MapId = buffer.ReadUInt32();
			if(data.HasRoomOwnerUserId())data.RoomOwnerUserId = buffer.ReadString();
			if(data.HasPlayers())data.Players = buffer.ReadComplexCollection( (rBytes)=>PtRoomPlayer.Read(rBytes) );
			
            return data;
        }       
    }
}
}
