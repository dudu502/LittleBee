//Template auto generator:[AutoGenPt] v1.0
//Creation time:2020/10/9 18:09:54
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtRoomList
{
    public byte __tag__ { get;private set;}

	public List<PtRoom> Rooms{ get;private set;}
	   
    public PtRoomList SetRooms(List<PtRoom> value){Rooms=value; __tag__|=1<<0; return this;}
	
    public bool HasRooms(){return (__tag__&(1<<0))!=0;}
	
    public static byte[] Write(PtRoomList data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasRooms())buffer.WriteComplexCollection(data.Rooms,(element)=>PtRoom.Write(element));
			
            return buffer.Getbuffer();
        }
    }

    public static PtRoomList Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtRoomList data = new PtRoomList();
            data.__tag__ = buffer.ReadByte();
			if(data.HasRooms())data.Rooms = buffer.ReadComplexCollection( (rBytes)=>PtRoom.Read(rBytes) );
			
            return data;
        }       
    }
}
}
