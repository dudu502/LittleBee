using NetServiceImpl.Server.Data;
using System;

namespace NetServiceImpl.Client.Data
{
    public class GameClientData
    {
        public static GamePlayerCvo SelfPlayer = new GamePlayerCvo();
        public static GameRoomSvo GameRoom;

        public static Guid SelfControlEntityId;
    }
}
