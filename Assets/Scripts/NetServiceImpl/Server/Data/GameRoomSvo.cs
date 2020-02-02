using Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServiceImpl.Server.Data
{
    public class GameRoomSvo
    {
        public int RoomId;
        public List<GameRoomMemberSvo> Members;

        public GameRoomSvo()
        {
            RoomId = GetHashCode();
            Members = new List<GameRoomMemberSvo>();
        }

        public static GameRoomSvo Read(byte[] bytes)
        {
            GameRoomSvo svo = new GameRoomSvo();
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                svo.RoomId = buffer.ReadInt32();
                svo.Members = new List<GameRoomMemberSvo>();
                int size = buffer.ReadInt32();
                for (int i = 0; i < size; ++i)
                    svo.Members.Add(GameRoomMemberSvo.Read(buffer.ReadBytes()));
                return svo;
            }                
        }
        public static byte[] Write(GameRoomSvo value)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteInt32(value.RoomId)
                    .WriteInt32(value.Members.Count);
                for (int i = 0; i < value.Members.Count; ++i)
                    buffer.WriteBytes(GameRoomMemberSvo.Write(value.Members[i]));
                return buffer.Getbuffer();
            }
        }
    }


    public class GameRoomMemberSvo
    {
        public long Id;
        public string Name="";
        public int Type;
        
        public static GameRoomMemberSvo Read(byte[] bytes)
        {
            GameRoomMemberSvo svo = new GameRoomMemberSvo();
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                svo.Id = buffer.ReadInt64();
                svo.Name = buffer.ReadString();
                svo.Type = buffer.ReadInt32();

                return svo;
            }
           
        }
        public static byte[] Write(GameRoomMemberSvo svo)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteInt64(svo.Id)
                    .WriteString(svo.Name)
                    .WriteInt32(svo.Type);
                return buffer.Getbuffer();
            }                
        }
    }
}
