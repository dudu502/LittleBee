using Synchronize.Game.Lockstep.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synchronize.Game.Lockstep.RoomServer.Core.Data
{
    public class UserStateObject
    {      
        public UserState StateFlag = UserState.EnteredRoom;
        public LiteNetLib.NetPeer NetPeer;
        public string UserName;
        public uint EntityId;
        public bool IsOnline = true;
        public UserStateObject(LiteNetLib.NetPeer peer, UserState state,string userName,uint entityId)
        {
            EntityId = entityId;
            UserName = userName;
            Update(peer, state);
        }
        public void Update(LiteNetLib.NetPeer peer, UserState state)
        {
            NetPeer = peer;
            StateFlag = state;
        }
    }
}
