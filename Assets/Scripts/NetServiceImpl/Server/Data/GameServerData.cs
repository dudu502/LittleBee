using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServiceImpl.Server.Data
{
    public class GameServerData
    {
        #region roomlist
        public static GameRoomSvo GameRoom = new GameRoomSvo();
        
        public static int EnterGameRoom(long roldId,string roleName)
        {
            foreach(var mem in GameRoom.Members)
            {
                if(mem.Id == roldId)
                {         
                    return -1;
                }
            }
            GameRoom.Members.Add(new GameRoomMemberSvo() { Id = roldId, Name= roleName });
            return 0;
        }
        public static void SetGameMemeberReady(long roleId)
        {
            foreach (var mem in GameRoom.Members)
                if(mem.Id == roleId)
                    mem.Type = 1;
        }
        public static bool IsAllReadyInGameRoom()
        {
            foreach (var mem in GameRoom.Members)
                if (mem.Type == 0) return false;
            return true;
        }
        #endregion

        #region player
        private static List<GamePlayerSvo> GamePlayerList = new List<GamePlayerSvo>();
        public static GamePlayerSvo AddNewPlayer()
        {
            var player = new GamePlayerSvo();
            GamePlayerList.Add(player);
            return player;
        }
        #endregion

    }
}
