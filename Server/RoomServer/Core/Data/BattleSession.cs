
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
        public readonly Dictionary<uint, UserData> DictUsers = new Dictionary<uint, UserData>();
        public uint InitEntityId = 0;
        public BattleSession()
        {
            KeyFrameList.SetElements(new List<PtKeyFrameCollection>());
            EntityTypeUids[Misc.EntityType.Bullet] = 1000000000;
        }

        public bool HasOnlinePlayer()
        {
            foreach (UserData userState in DictUsers.Values)
            {
                if (userState.IsOnline) return true;
            }
            return false;
        }

        public UserData FindUserStateByUserName(string userName)
        {
            foreach (UserData userState in DictUsers.Values)
            {
                if (userState.UserName == userName)
                    return userState;
            }
            return null;
        }
        public UserData FindUserStateBySessionId(string sessionId)
        {
            foreach (UserData userState in DictUsers.Values)
            {
                if (userState.SessionId == sessionId) return userState;
            }
            return null;
        }
    }
}
