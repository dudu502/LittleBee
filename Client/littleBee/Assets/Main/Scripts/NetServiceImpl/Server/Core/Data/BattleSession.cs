using LiteNetLib;
using Net.Pt;
using Synchronize.Game.Lockstep.Misc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Synchronize.Game.Lockstep.RoomServer.Core.Data
{
    public class BattleSession
    {
        public readonly PtKeyFrameCollectionList KeyFrameList = new PtKeyFrameCollectionList();
        public readonly StartupConfig StartupCFG = new StartupConfig();
        public readonly Dictionary<EntityType, uint> EntityTypeUids = new Dictionary<EntityType, uint>();
        public readonly ConcurrentQueue<PtKeyFrameCollection> QueueKeyFrameCollection = new ConcurrentQueue<PtKeyFrameCollection>();
        public readonly Dictionary<uint, UserStateObject> DictUsers = new Dictionary<uint, UserStateObject>();
        public uint InitEntityId = 0;
        public BattleSession()
        {
            KeyFrameList.SetElements(new List<PtKeyFrameCollection>());
            EntityTypeUids[EntityType.Bullet] = 1000000000;
        }

        public bool HasOnlinePlayer()
        {
            foreach (UserStateObject userState in DictUsers.Values)
            {
                if (userState.IsOnline) return true;
            }
            return false;
        }

        public UserStateObject FindUserStateByUserName(string userName)
        {
            foreach (UserStateObject userState in DictUsers.Values)
            {
                if (userState.UserName == userName)
                    return userState;
            }
            return null;
        }
        public UserStateObject FindUserStateByNetPeer(NetPeer netPeer)
        {
            foreach (UserStateObject userState in DictUsers.Values)
            {
                if (userState.NetPeer == netPeer)
                    return userState;
            }
            return null;
        }
    }
}
