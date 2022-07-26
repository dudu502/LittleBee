using LiteNetLib;
using Net.Pt;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace RoomServer.Core.Data
{
    public class BattleSession
    {
        public readonly PtKeyFrameCollectionList KeyFrameList = new PtKeyFrameCollectionList();
        public readonly StartupConfig StartupCFG = new StartupConfig();
        public readonly Dictionary<Misc.EntityType, uint> EntityTypeUids = new Dictionary<Misc.EntityType, uint>();
        public readonly ConcurrentQueue<PtKeyFrameCollection> QueueKeyFrameCollection = new ConcurrentQueue<PtKeyFrameCollection>();
        public readonly Dictionary<uint, UserState> DictUsers = new Dictionary<uint, UserState>();
        public uint InitEntityId = 0;
        public BattleSession()
        {
            KeyFrameList.SetElements(new List<PtKeyFrameCollection>());
            EntityTypeUids[Misc.EntityType.Bullet] = 1000000000;
        }

        public bool HasOnlinePlayer()
        {
            foreach (UserState userState in DictUsers.Values)
            {
                if (userState.IsOnline) return true;
            }
            return false;
        }

        public UserState FindUserStateByUserName(string userName)
        {
            foreach (UserState userState in DictUsers.Values)
            {
                if (userState.UserName == userName)
                    return userState;
            }
            return null;
        }
        public UserState FindUserStateByNetPeer(NetPeer netPeer)
        {
            foreach (UserState userState in DictUsers.Values)
            {
                if (userState.NetPeer == netPeer)
                    return userState;
            }
            return null;
        }
    }
}
