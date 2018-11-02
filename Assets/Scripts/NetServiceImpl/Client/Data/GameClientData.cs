using NetServiceImpl.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServiceImpl.Client.Data
{
    public class GameClientData
    {
        public static GamePlayerCvo SelfPlayer = new GamePlayerCvo();
        public static GameRoomSvo GameRoom;
    }
}
