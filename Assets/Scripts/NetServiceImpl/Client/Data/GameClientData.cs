using NetServiceImpl.Server.Data;

namespace NetServiceImpl.Client.Data
{
    public class GameClientData
    {
        public static GamePlayerCvo SelfPlayer = new GamePlayerCvo();
        public static GameRoomSvo GameRoom;

        public static string SelfControlEntityId = "";
    }
}
