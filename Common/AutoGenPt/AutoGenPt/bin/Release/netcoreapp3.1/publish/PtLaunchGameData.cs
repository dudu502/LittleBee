//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/10/9 18:09:54
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtLaunchGameData
{
    public byte __tag__ { get;private set;}

	public string RSAddress{ get;private set;}
	public ushort RSPort{ get;private set;}
	public uint MapId{ get;private set;}
	public string ConnectionKey{ get;private set;}
	   
    public PtLaunchGameData SetRSAddress(string value){RSAddress=value; __tag__|=1<<0; return this;}
	public PtLaunchGameData SetRSPort(ushort value){RSPort=value; __tag__|=1<<1; return this;}
	public PtLaunchGameData SetMapId(uint value){MapId=value; __tag__|=1<<2; return this;}
	public PtLaunchGameData SetConnectionKey(string value){ConnectionKey=value; __tag__|=1<<3; return this;}
	
    public bool HasRSAddress(){return (__tag__&(1<<0))!=0;}
	public bool HasRSPort(){return (__tag__&(1<<1))!=0;}
	public bool HasMapId(){return (__tag__&(1<<2))!=0;}
	public bool HasConnectionKey(){return (__tag__&(1<<3))!=0;}
	
    public static byte[] Write(PtLaunchGameData data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasRSAddress())buffer.WriteString(data.RSAddress);
			if(data.HasRSPort())buffer.WriteUInt16(data.RSPort);
			if(data.HasMapId())buffer.WriteUInt32(data.MapId);
			if(data.HasConnectionKey())buffer.WriteString(data.ConnectionKey);
			
            return buffer.Getbuffer();
        }
    }

    public static PtLaunchGameData Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtLaunchGameData data = new PtLaunchGameData();
            data.__tag__ = buffer.ReadByte();
			if(data.HasRSAddress())data.RSAddress = buffer.ReadString();
			if(data.HasRSPort())data.RSPort = buffer.ReadUInt16();
			if(data.HasMapId())data.MapId = buffer.ReadUInt32();
			if(data.HasConnectionKey())data.ConnectionKey = buffer.ReadString();
			
            return data;
        }       
    }
}
}
