//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/10/9 18:09:54
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtRoomPlayer
{
    public byte __tag__ { get;private set;}

	public uint EntityId{ get;private set;}
	public byte TeamId{ get;private set;}
	public string NickName{ get;private set;}
	public string UserId{ get;private set;}
	public string Password{ get;private set;}
	public byte Status{ get;private set;}
	   
    public PtRoomPlayer SetEntityId(uint value){EntityId=value; __tag__|=1<<0; return this;}
	public PtRoomPlayer SetTeamId(byte value){TeamId=value; __tag__|=1<<1; return this;}
	public PtRoomPlayer SetNickName(string value){NickName=value; __tag__|=1<<2; return this;}
	public PtRoomPlayer SetUserId(string value){UserId=value; __tag__|=1<<3; return this;}
	public PtRoomPlayer SetPassword(string value){Password=value; __tag__|=1<<4; return this;}
	public PtRoomPlayer SetStatus(byte value){Status=value; __tag__|=1<<5; return this;}
	
    public bool HasEntityId(){return (__tag__&(1<<0))!=0;}
	public bool HasTeamId(){return (__tag__&(1<<1))!=0;}
	public bool HasNickName(){return (__tag__&(1<<2))!=0;}
	public bool HasUserId(){return (__tag__&(1<<3))!=0;}
	public bool HasPassword(){return (__tag__&(1<<4))!=0;}
	public bool HasStatus(){return (__tag__&(1<<5))!=0;}
	
    public static byte[] Write(PtRoomPlayer data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasEntityId())buffer.WriteUInt32(data.EntityId);
			if(data.HasTeamId())buffer.WriteByte(data.TeamId);
			if(data.HasNickName())buffer.WriteString(data.NickName);
			if(data.HasUserId())buffer.WriteString(data.UserId);
			if(data.HasPassword())buffer.WriteString(data.Password);
			if(data.HasStatus())buffer.WriteByte(data.Status);
			
            return buffer.Getbuffer();
        }
    }

    public static PtRoomPlayer Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtRoomPlayer data = new PtRoomPlayer();
            data.__tag__ = buffer.ReadByte();
			if(data.HasEntityId())data.EntityId = buffer.ReadUInt32();
			if(data.HasTeamId())data.TeamId = buffer.ReadByte();
			if(data.HasNickName())data.NickName = buffer.ReadString();
			if(data.HasUserId())data.UserId = buffer.ReadString();
			if(data.HasPassword())data.Password = buffer.ReadString();
			if(data.HasStatus())data.Status = buffer.ReadByte();
			
            return data;
        }       
    }
}
}
