//Template auto generator:[AutoGenPt] v1.0
//Creation time:2021/6/30 11:58:47
using System;
using System.Collections;
using System.Collections.Generic;
namespace Lenovo.XR.Development.Net.Pt
{
public class PtLaunchGameData
{
    public byte __tag__ { get;private set;}

	public string RSAddress{ get;private set;}
	public ushort RSPort{ get;private set;}
	public uint MapId{ get;private set;}
	public string ConnectionKey{ get;private set;}
	public bool IsStandaloneMode{ get;private set;}
	public byte PlayerNumber{ get;private set;}
	   
    public PtLaunchGameData SetRSAddress(string value){RSAddress=value; __tag__|=1; return this;}
	public PtLaunchGameData SetRSPort(ushort value){RSPort=value; __tag__|=2; return this;}
	public PtLaunchGameData SetMapId(uint value){MapId=value; __tag__|=4; return this;}
	public PtLaunchGameData SetConnectionKey(string value){ConnectionKey=value; __tag__|=8; return this;}
	public PtLaunchGameData SetIsStandaloneMode(bool value){IsStandaloneMode=value; __tag__|=16; return this;}
	public PtLaunchGameData SetPlayerNumber(byte value){PlayerNumber=value; __tag__|=32; return this;}
	
    public bool HasRSAddress(){return (__tag__&1)==1;}
	public bool HasRSPort(){return (__tag__&2)==2;}
	public bool HasMapId(){return (__tag__&4)==4;}
	public bool HasConnectionKey(){return (__tag__&8)==8;}
	public bool HasIsStandaloneMode(){return (__tag__&16)==16;}
	public bool HasPlayerNumber(){return (__tag__&32)==32;}
	
    public static byte[] Write(PtLaunchGameData data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasRSAddress())buffer.WriteString(data.RSAddress);
			if(data.HasRSPort())buffer.WriteUInt16(data.RSPort);
			if(data.HasMapId())buffer.WriteUInt32(data.MapId);
			if(data.HasConnectionKey())buffer.WriteString(data.ConnectionKey);
			if(data.HasIsStandaloneMode())buffer.WriteBool(data.IsStandaloneMode);
			if(data.HasPlayerNumber())buffer.WriteByte(data.PlayerNumber);
			
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
			if(data.HasIsStandaloneMode())data.IsStandaloneMode = buffer.ReadBool();
			if(data.HasPlayerNumber())data.PlayerNumber = buffer.ReadByte();
			
            return data;
        }       
    }
}
}
