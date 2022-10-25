//Template auto generator:[AutoGenPt] v1.0
//Creation time:2021/2/10 15:11:43
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtRoomList
{
    public byte __tag__ { get;private set;}

	public List<PtRoom> Rooms{ get;private set;}
	   
    public PtRoomList SetRooms(List<PtRoom> value){Rooms=value; __tag__|=1; return this;}
	
    public bool HasRooms(){return (__tag__&1)==1;}
	
    public static byte[] Write(PtRoomList data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasRooms())buffer.WriteCollection(data.Rooms,element=>PtRoom.Write(element));
			
            return buffer.GetRawBytes();
        }
    }

    public static PtRoomList Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtRoomList data = new PtRoomList();
            data.__tag__ = buffer.ReadByte();
			if(data.HasRooms())data.Rooms = buffer.ReadCollection(retbytes=>PtRoom.Read(retbytes));
			
            return data;
        }       
    }
}
}
