using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer.Core.Data
{
    public class UserState
    {      
        public Misc.UserState StateFlag = Misc.UserState.EnteredRoom;
        public LiteNetLib.NetPeer NetPeer;
        public string UserName;
        public uint EntityId;
        public bool IsOnline = true;
        public UserState(LiteNetLib.NetPeer peer, Misc.UserState state,string userName,uint entityId)
        {
            EntityId = entityId;
            UserName = userName;
            Update(peer, state);
        }
        public void Update(LiteNetLib.NetPeer peer, Misc.UserState state)
        {
            NetPeer = peer;
            StateFlag = state;
        }
    }
}
