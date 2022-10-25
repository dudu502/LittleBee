using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer.Core.Data
{
    public class UserData
    {      
        public Misc.UserState StateFlag = Misc.UserState.EnteredRoom;
        public string SessionId;
        public string UserName;
        public uint EntityId;
        public bool IsOnline = true;
        public UserData(string sessionId, Misc.UserState state,string userName,uint entityId)
        {
            EntityId = entityId;
            UserName = userName;
            Update(sessionId, state);
        }
        public void Update(string sessionId, Misc.UserState state)
        {
            SessionId = sessionId;
            StateFlag = state;
        }
    }
}
