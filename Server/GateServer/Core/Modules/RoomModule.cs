
using LiteNetLib;
using Net;
using Net.Pt;
using Net.ServiceImpl;
using Synchronize.Game.Lockstep.Evt;
using Synchronize.Game.Lockstep.GateServer.Core.Data;
using Synchronize.Game.Lockstep.GateServer.Services;
using Synchronize.Game.Lockstep.Service.Core;
using Synchronize.Game.Lockstep.Service.Event;
using Synchronize.Game.Lockstep.Service.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.GateServer.Modules
{
    public class RoomProcess
    {
#if WAN_MODE
        public Process CurrentProcess { private set; get; }
        public uint RoomId { set; get; }
        public int Port { set; get; }
        public PtLaunchGameData LaunchGameData { set; get; }
        public void Set(Process proc)
        {
            CurrentProcess = proc;
        }
        public void Kill()
        {
            if (CurrentProcess != null)
                CurrentProcess.Kill();
        }
#elif LAN_MODE
        public RoomServer.Services.RoomApplication RoomInstance{private set;get;}
        public uint RoomId { set; get; }
        public int Port { set; get; }
        public void Set(RoomServer.Services.RoomApplication app)
        {
            RoomInstance = app;
        }
        public void Kill()
        {
            if(RoomInstance!=null)
                RoomInstance.ShutDown();
        }
#endif



    }
    public class RoomModule : BaseModule
    {
        private PtRoomList m_PtRoomList;
        Dictionary<int, RoomProcess> m_DictProcessId = new Dictionary<int, RoomProcess>();
        private List<User> m_AllPlayers = new List<User>();
        public RoomModule(BaseApplication app) : base(app)
        {
            m_PtRoomList = new PtRoomList();
            m_PtRoomList.SetRooms(new List<PtRoom>());
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.GS_EnterGate,OnEnterGate);
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.GS_RoomList, OnRoomList);
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.GS_CreateRoom, OnCreateRoom);
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.GS_UpdateRoom, OnUpdateRoom);
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.GS_JoinRoom, OnJoinRoom);
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.GS_LeaveRoom, OnLeaveRoom);
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.GS_LaunchGame,OnLaunchGame);
            EventMgr<RequestMessageId, UnconnectedNetMessageEvt>.AddListener(RequestMessageId.UGS_RoomPlayerDisconnect, OnRoomPlayerDisconnet);

            EventMgr<NetActionEvent, NetPeer>.AddListener(NetActionEvent.PeerDisconnectedEvent, OnPeerDisconnected);
        }

        void OnPeerDisconnected(NetPeer peer)
        {
            foreach(var user in m_AllPlayers)
            {
                if(user.Peer == peer)
                {
                    for(int i=m_PtRoomList.Rooms.Count-1;i>-1;--i)
                    {
                        var room = m_PtRoomList.Rooms[i];
                        if(room.Status == 0)
                        {
                            var player = room.Players.Find(player => player.UserId == user.UserId);
                            if(player!=null)
                            {
                                OnLeaveRoomImpl(room.RoomId, user);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Room Server 中有玩家断开连接
        /// </summary>
        /// <param name="evt"></param>
        void OnRoomPlayerDisconnet(UnconnectedNetMessageEvt evt)
        {
            using(ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                int roomPort = buffer.ReadInt32();
                string userName = buffer.ReadString();
                bool hasOnlinePlayer = buffer.ReadBool();
                if(!hasOnlinePlayer)
                {
                    //There is no player at room,so kill room process
                    var roomData = m_PtRoomList.Rooms.Find((room) => room.Players.Exists((player)=>player.NickName==userName));
                    if (roomData != null)
                    {
                        m_PtRoomList.Rooms.Remove(roomData);
                        BaseApplication.Logger.Log($"Remove Room Id:{roomData.RoomId} by PlayerDisconnect CurrentRoomListCount:{m_PtRoomList.Rooms.Count}");
                    }

                    KillRoomProcessByPort(roomPort);
                }
                BaseApplication.Logger.Log($"OnRoomPlayerDisconnet at roomPort:{roomPort} userName:{userName} hasOnlinePlayer:{hasOnlinePlayer}");
            }
        }

        void OnEnterGate(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                string userId = buffer.ReadString();
                User playerVO = new User();
                playerVO.UserId = userId;
                playerVO.Peer = evt.Peer;
                m_AllPlayers.Add(playerVO);
                BaseApplication.Logger.Log($"OnEnterGate UserId:{userId}");
            }
        }

        /// <summary>
        /// 返回room 列表给client
        /// </summary>
        /// <param name="evt"></param>
        void OnRoomList(NetMessageEvt evt)
        {
            NetStreamUtil.Send(evt.Peer, PtMessagePackage.Build((ushort)ResponseMessageId.GS_RoomList, PtRoomList.Write(m_PtRoomList)));
        }
        /// <summary>
         /// 返回room 列表给client
         /// </summary>
         /// <param name="evt"></param>
        void OnRoomList(NetPeer peer)
        {
            NetStreamUtil.Send(peer, PtMessagePackage.Build((ushort)ResponseMessageId.GS_RoomList, PtRoomList.Write(m_PtRoomList)));
        }
        uint GetGuidUint()
        {
            byte[] bytes = Guid.NewGuid().ToByteArray();
            return BitConverter.ToUInt32(bytes, 0);
        }
        /// <summary>
        /// 0:更新队伍
        /// 1:更新颜色
        /// 2:更新地图
        /// 更新机器人
        /// </summary>
        /// <param name="evt"></param>
        void OnUpdateRoom(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                byte type = buffer.ReadByte();
                uint roomId = buffer.ReadUInt32();
                string userId;
                PtRoom ptRoom = m_PtRoomList.Rooms.Find(room => room.RoomId == roomId);
                if (ptRoom != null)
                {
                    if (type == 0)
                    {
                        userId = buffer.ReadString();
                        byte teamId = buffer.ReadByte();
                        PtRoomPlayer ptRoomPlayer = ptRoom.Players.Find(player => player.UserId == userId);
                        if(ptRoomPlayer!=null)
                        {
                            ptRoomPlayer.SetTeamId(teamId);
                        }
                    }
                    else if (type == 1)
                    {
                        userId = buffer.ReadString();
                        PtRoomPlayer ptRoomPlayer = ptRoom.Players.Find(player => player.UserId == userId);
                        if (ptRoomPlayer != null)
                        {
                            for (int i = 0; i < 16; ++i)
                            {
                                if (null == ptRoom.Players.Find(player => player.Color == (byte)i))
                                {
                                    ptRoomPlayer.SetColor((byte)i);
                                    break;
                                }
                            }
                        }
                    }
                    else if (type == 2)
                    {
                        ptRoom.SetMapId(buffer.ReadUInt32());
                        ptRoom.SetMaxPlayerCount(buffer.ReadByte());

                        //while(ptRoom.Players.Count<ptRoom.MaxPlayerCount)
                        //{
                        //    ptRoom.Players.Add(new PtRoomPlayer()
                        //        .SetType((byte)Misc.RoomPlayerType.Empty)
                        //        );
                        //}
                    }
                    OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_UpdateRoom, PtRoom.Write(ptRoom));
                }
            }       
        }
        /// <summary>
        /// 创建房间-
        /// </summary>
        /// <param name="evt"></param>
        void OnCreateRoom(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                uint mapId = buffer.ReadUInt32(); 
                string userId = buffer.ReadString();
                string userName = buffer.ReadString();
                string userPwd = buffer.ReadString();
                byte teamId = buffer.ReadByte();
                PtRoom room = new PtRoom();
                room.SetMapId(mapId)
                    .SetStatus(0)//idle
                    .SetRoomId(GetGuidUint())
                    .SetRoomOwnerUserId(userId)
                    .SetPlayers(new List<PtRoomPlayer>());
                PtRoomPlayer player = new PtRoomPlayer();
                player.SetEntityId(1)
                    .SetType((byte)Synchronize.Game.Lockstep.Misc.RoomPlayerType.Player)
                    .SetNickName(userName)
                    .SetPassword(userPwd)
                    .SetStatus(1)
                    .SetTeamId(teamId)
                    .SetColor(0)
                    .SetUserId(userId);
                room.Players.Add(player);
                m_PtRoomList.Rooms.Add(room);

                NetStreamUtil.Send(evt.Peer, (ushort)ResponseMessageId.GS_CreateRoom,PtRoom.Write(room));
                BaseApplication.Logger.Log($"OnCreateRoom mapId:{mapId} userId:{userId} userName:{userName} teamId:{teamId}");
            }
        }

        /// <summary>
        /// receive a joinroom request.
        /// </summary>
        /// <param name="evt"></param>
        void OnJoinRoom(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                uint roomId = buffer.ReadUInt32();
                string userId = buffer.ReadString();
                string userName = buffer.ReadString();
                string userPwd = buffer.ReadString();
                byte teamId = buffer.ReadByte();
                var ptRoom = m_PtRoomList.Rooms.Find((room) => room.RoomId == roomId);
                if(ptRoom != null)
                {
                    if(ptRoom.Status == 0)
                    {                   
                        //房间是等待状态，这个状态下玩家可以进入房间，并且等待房主开始游戏
                        var ptPlayer = ptRoom.Players.Find((player) => player.UserId == userId);
                        if (ptPlayer == null)
                        {
                            ptRoom.Players.Add(new PtRoomPlayer()
                                .SetEntityId(2)
                                .SetType((byte)Synchronize.Game.Lockstep.Misc.RoomPlayerType.Player)
                                .SetNickName(userName)
                                .SetPassword(userPwd)
                                .SetStatus(1)
                                .SetTeamId(teamId)
                                .SetColor((byte)ptRoom.Players.Count)
                                .SetUserId(userId));
                        }
                        NetStreamUtil.Send(evt.Peer, (ushort)ResponseMessageId.GS_JoinRoom, new ByteBuffer().WriteByte(0).Getbuffer());
                        OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_UpdateRoom, PtRoom.Write(ptRoom));                     
                    }
                    else if (ptRoom.Players.Exists(p => p.NickName == userName))
                    {
                        //当前房间状态是正在战斗中，此时如果有房间内的玩家由于 断线原因重新进入房间
#if WAN_MODE
                        RoomProcess roomProcess = GetRoomProcess(ptRoom.RoomId);
                        if (roomProcess != null)
                        {
                            NetStreamUtil.Send(evt.Peer, (ushort)ResponseMessageId.GS_LaunchGame, null);
                            //await Task.Delay(30);
                            NetStreamUtil.Send(evt.Peer, (ushort)ResponseMessageId.GS_LaunchRoomInstance, PtLaunchGameData.Write(roomProcess.LaunchGameData));
                        }
#elif LAN_MODE
#endif
                    }
                    else
                    {
                        OnError(evt.Peer, 100000);
                    }
                }
            }
        }

        bool OnUpdateDataToRoomMember(PtRoom ptRoom, ushort messageId,byte[] bytes)
        {
            bool ret = false;
            if (ptRoom.HasPlayers())
            {
                ptRoom.Players.ForEach((player)=> 
                {
                    m_AllPlayers.ForEach((e) => 
                    {
                        if (e.UserId == player.UserId)
                        {
                            ret = true;
                            NetStreamUtil.Send(e.Peer, messageId, bytes);                           
                        }
                    });
                });
            }
            return ret;
        }
        void OnLeaveRoomImpl(uint roomId,User user)
        {
            BaseApplication.Logger.Log($"OnLeaveRoom Room Id:{roomId} by leave room CurrentRoomListCount hasRoom:{m_PtRoomList.HasRooms()}");
            if (m_PtRoomList.HasRooms())
            {
                PtRoom ptRoom = m_PtRoomList.Rooms.Find((room) => room.RoomId == roomId);
                if (ptRoom != null && ptRoom.HasPlayers())
                {
                    PtRoomPlayer roomPlayer = ptRoom.Players.Find((player) => player.UserId == user.UserId);
                    ptRoom.Players.Remove(roomPlayer);
                    if (roomPlayer.UserId == ptRoom.RoomOwnerUserId)
                    {
                        //select new player as room-owner
                        if (ptRoom.Players.Count > 0)
                            ptRoom.SetRoomOwnerUserId(ptRoom.Players[0].UserId);
                    }

                    NetStreamUtil.Send(user.Peer, (ushort)ResponseMessageId.GS_LeaveRoom, null);

                    OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_UpdateRoom, PtRoom.Write(ptRoom));

                    if (ptRoom.Players.Count == 0)
                    {
                        m_PtRoomList.Rooms.Remove(ptRoom);
                        BaseApplication.Logger.Log($"Remove Room Id:{ptRoom.RoomId} by leave room CurrentRoomListCount:{m_PtRoomList.Rooms.Count}");
                    }
                    OnRoomList(user.Peer);
                }
            }
        }
        void OnLeaveRoom(NetMessageEvt evt)
        {
            using(ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                User user = m_AllPlayers.Find((e) => e.Peer == evt.Peer);
                
                if (user != null)
                {
                    uint roomId = buffer.ReadUInt32();
                    OnLeaveRoomImpl(roomId, user);
                }               
            }
        }

        async void OnLaunchGame(NetMessageEvt evt)
        {
            using(ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                uint roomId = buffer.ReadUInt32();
                if (m_PtRoomList.HasRooms())
                {
                    PtRoom ptRoom = m_PtRoomList.Rooms.Find((room) => room.RoomId == roomId);
                    if (ptRoom != null)
                    {
                        if(ptRoom.HasPlayers())
                        {
                            PtLaunchGameData gameData = new PtLaunchGameData();
                            gameData.SetPlayerNumber((byte)ptRoom.Players.Count);
                            gameData.SetIsStandaloneMode(ptRoom.Players.Count == 1
                                || !ptRoom.Players.Exists(player => player.UserId != ptRoom.RoomOwnerUserId && (player.Type==(byte)Synchronize.Game.Lockstep.Misc.RoomPlayerType.Player)));
#if WAN_MODE
                            gameData.SetRSAddress(GetApplication<GateApplication>().StartupConfigs["RoomAddress"]);
                            gameData.SetRSPort(GetEmptyRoomPort());
                            gameData.SetMapId(ptRoom.MapId);
                            gameData.SetConnectionKey("RoomBattle");
                         
                            OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_LaunchGame, null);
                            if (!gameData.IsStandaloneMode)
                            {
                                await CreateRoomProcess(GetApplication<GateApplication>().StartupConfigs["RoomModuleFullPath"], gameData, ptRoom);
                            }
                            else
                            {                               
                                m_PtRoomList.Rooms.Remove(ptRoom);
                                BaseApplication.Logger.Log($"Remove Room Id:{ptRoom.RoomId} by launch game CurrentRoomListCount:{m_PtRoomList.Rooms.Count}");
                            }
                            
                            OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_LaunchRoomInstance, PtLaunchGameData.Write(gameData));
                            ptRoom.SetStatus(1);//in battle
#elif LAN_MODE
                            gameData.SetRSAddress(GetApplication<GateApplication>().StartupConfigs["RoomAddress"]);
                            gameData.SetRSPort(GetEmptyRoomPort());
                            gameData.SetMapId(ptRoom.MapId);
                            gameData.SetConnectionKey("RoomBattle");

                            OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_LaunchGame, null);
                            Task createRoomInstanceTask = CreateRoomInstance(gameData.ConnectionKey, gameData.RSPort);
                            await createRoomInstanceTask;
                            OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_LaunchRoomInstance, PtLaunchGameData.Write(gameData));
                            ptRoom.SetStatus(1);//in battle
#endif

                        }
                    }
                }              
            }
        }
        ushort GetEmptyRoomPort()
        {
            for(int i = 60000; i < 61000; ++i)
            {
                if (!m_DictProcessId.ContainsKey(i))
                    return (ushort)i;
            }
            return 0;
        }

#if WAN_MODE
        public Task CreateRoomProcess(string dllPath, PtLaunchGameData launchGameData , PtRoom ptRoom)
        {
            if (m_DictProcessId.ContainsKey(launchGameData.RSPort))
            {
                throw new Exception("port is occupied " + launchGameData.RSPort);
            }
            RoomProcess roomProcess = new RoomProcess();
            m_DictProcessId[launchGameData.RSPort] = roomProcess;
            roomProcess.LaunchGameData = launchGameData;
            roomProcess.RoomId = ptRoom.RoomId;
            roomProcess.Port = launchGameData.RSPort;
            var psi = new ProcessStartInfo("dotnet", " "+ dllPath + 
                " -key " + launchGameData.ConnectionKey + 
                " -port " + launchGameData.RSPort + 
                " -mapId "+ptRoom.MapId+
                " -playernumber "+ launchGameData.PlayerNumber+
                " -gsPort "+ApplicationInstance.Port+
                " -hash "+ ptRoom.GetHashCode()+"_"+DateTime.Now.Ticks +
                " -logserverurl "+ ApplicationInstance.GetConfigElement<string>("log_server_address"));
            psi.CreateNoWindow = false;
            psi.UseShellExecute = false;
            Task task = Task.Run(()=> 
            {
                DateTime n = DateTime.Now;
                var proc = Process.Start(psi);
                roomProcess.Set(proc);
                BaseApplication.Logger.Log("Start process in Task " + (DateTime.Now - n).TotalMilliseconds);
            });
            return task;      
        }
        RoomProcess GetRoomProcess(uint roomId)
        {
            foreach(RoomProcess roomProcess in m_DictProcessId.Values)
            {
                if (roomProcess.RoomId == roomId)
                    return roomProcess;
            }
            return null;
        }
#elif LAN_MODE
        public Task CreateRoomInstance(string key, ushort port)
        {
            if (m_DictProcessId.ContainsKey(port))
            {
                throw new Exception("port is occupied " + port);
            }
            var roomprocess = new RoomProcess();
            RoomServer.Services.RoomApplication.Logger = new Logger.UnityEnvLogger();
            var room = new RoomServer.Services.RoomApplication(key);
            room.StartServer(port);
            roomprocess.Set(room);
            m_DictProcessId[port] = roomprocess;
            return Task.Delay(10);
        }
#endif

        public void KillRoomProcessByPort(int port)
        {
            if (m_DictProcessId.ContainsKey(port))
            {
                m_DictProcessId[port].Kill();
                m_DictProcessId.Remove(port);
            }         
        }
        void OnError(NetPeer peer,int errorId)
        {
            NetStreamUtil.Send(peer, (ushort)ResponseMessageId.GS_ErrorCode, PtErrorCode.Write(new PtErrorCode().SetId(errorId)));
        }
    }
}
