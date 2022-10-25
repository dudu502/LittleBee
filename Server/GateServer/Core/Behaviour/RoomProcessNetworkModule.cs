using GateServer.Core.Data;
using ServerDll.Service.Behaviour;
using Service.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace GateServer.Core.Behaviour
{
    public class RoomProcessNetworkModule : NetworkModule
    {
        public RoomProcessNetworkModule(WebSocketServer wssParam) : base(wssParam)
        {
            Evt.EventMgr<Net.ServiceImpl.RequestMessageId, NetMessageEvt>.AddListener(Net.ServiceImpl.RequestMessageId.UGS_RoomPlayerDisconnect, OnRoomPlayerDisconnect);
        }
        void OnRoomPlayerDisconnect(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                int roomPort = buffer.ReadInt32();
                string userName = buffer.ReadString();
                bool hasOnlinePlayer = buffer.ReadBool();
                if (!hasOnlinePlayer)
                {
                    var room = DataMgr.Instance.RoomList.Rooms.Find(r => r.Players.Exists(player => player.NickName == userName));
                    if (room != null && room.Status == 0)
                    {
                        DataMgr.Instance.RoomList.Rooms.Remove(room);
                        LogInfo($"Remove Room Id:{room.RoomId} by PlayerDisconnect CurrentRoomListCount:{DataMgr.Instance.RoomList.Rooms.Count} ");
                    }
                    KillRoomProcessByPort(roomPort);
                }
                LogInfo($"OnRoomPlayerDisconnect at room port:{roomPort} userName:{userName} hasOnlinePlayer:{hasOnlinePlayer}");
            }
        }
        public void KillRoomProcessByPort(int port)
        {
            if (DataMgr.Instance.ProcessIds.ContainsKey(port))
            {
                DataMgr.Instance.ProcessIds[port].Kill();
                DataMgr.Instance.ProcessIds.Remove(port);
            }
        }
        
    }
}
