using GateServer.Core.Data;
using Net;
using Net.Pt;
using Net.ServiceImpl;
using ServerDll.Service.Module;
using ServerDll.Service.Provider;
using Service.Event;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace GateServer.Core.Behaviour
{
    public class RoomNetworkModule:NetworkModule
    {
        public RoomNetworkModule(IProvider provider) : base(provider) 
        {
            Evt.EventMgr<RequestMessageId,  NetMessageEvt>.AddListener(RequestMessageId.GS_EnterGate, OnEnterGate);
            Evt.EventMgr<RequestMessageId,  NetMessageEvt>.AddListener(RequestMessageId.GS_RoomList, OnRoomList);
            Evt.EventMgr<RequestMessageId,  NetMessageEvt>.AddListener(RequestMessageId.GS_CreateRoom, OnCreateRoom);
            Evt.EventMgr<RequestMessageId,  NetMessageEvt>.AddListener(RequestMessageId.GS_UpdateRoom, OnUpdateRoom);
            Evt.EventMgr<RequestMessageId,  NetMessageEvt>.AddListener(RequestMessageId.GS_JoinRoom, OnJoinRoom);
            Evt.EventMgr<RequestMessageId,  NetMessageEvt>.AddListener(RequestMessageId.GS_LeaveRoom, OnLeaveRoom);
            Evt.EventMgr<RequestMessageId,  NetMessageEvt>.AddListener(RequestMessageId.GS_LaunchGame, OnLaunchGame);
            Evt.EventMgr<RequestMessageId,  NetMessageEvt>.AddListener(RequestMessageId.SOCKET_MSG_ONOPEN, OnOpen);
            Evt.EventMgr<RequestMessageId,  NetMessageEvt>.AddListener(RequestMessageId.SOCKET_MSG_ONCLOSE, OnRoomPlayerDisconnect);
            Evt.EventMgr<RequestMessageId,  NetMessageEvt>.AddListener(RequestMessageId.SOCKET_MSG_ONERROR, OnRoomPlayerDisconnect);
            Evt.EventMgr<RequestMessageId,  NetMessageEvt>.AddListener(RequestMessageId.UGS_RoomPlayerDisconnect, OnRoomPlayerDisconnectAndKillProcess);
        }
        void OnOpen(NetMessageEvt evt)
        {
            netProvider.SendToAsync(PtMessagePackage.Build((ushort)ResponseMessageId.GS_ClientConnected,null), evt.SessionId);
        }
        void OnRoomPlayerDisconnect(NetMessageEvt evt)
        {
            foreach (var user in DataMgr.Instance.Users)
            {
                if (user.SessionId == evt.SessionId)
                {
                    for (int i = DataMgr.Instance.RoomList.Rooms.Count - 1; i >= 0; i--)
                    {
                        var room = DataMgr.Instance.RoomList.Rooms[i];
                        bool inRoom = false;
                        if (room != null && room.Status == 0)
                        {
                            for (int j = room.Players.Count - 1; j >= 0; j--)
                            {
                                var player = room.Players[j];
                                if (player != null && player.UserId == user.UserId)
                                {
                                    OnLeaveRoomImpl(room.RoomId, user);
                                    inRoom = true;
                                    break;
                                }
                            }
                        }
                        if (inRoom)
                            break;
                    }
                    break;
                }
            }
        }
        void OnRoomPlayerDisconnectAndKillProcess(NetMessageEvt evt)
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

        void OnEnterGate(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                string name = buffer.ReadString();
                User playerVO = new User();
                playerVO.UserId = name;
                playerVO.SessionId = evt.SessionId;
                DataMgr.Instance.Users.Add(playerVO);
                LogInfo($"OnEnterGate UserName:{name}");
            }
        }

        void OnRoomList(NetMessageEvt evt)
        {
            netProvider.SendToAsync(PtMessagePackage.Build((ushort)ResponseMessageId.GS_RoomList, PtRoomList.Write(DataMgr.Instance.RoomList)),evt.SessionId);
        }
        void OnRoomList(string sessionId)
        {
            netProvider.SendToAsync(PtMessagePackage.Build((ushort)ResponseMessageId.GS_RoomList, PtRoomList.Write(DataMgr.Instance.RoomList)), sessionId);
        }
        uint GetGuidUint()
        {
            byte[] bytes = Guid.NewGuid().ToByteArray();
            return BitConverter.ToUInt32(bytes, 0);
        }
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
                    .SetType((byte)Misc.RoomPlayerType.Player)
                    .SetNickName(userName)
                    .SetPassword(userPwd)
                    .SetStatus(1)
                    .SetTeamId(teamId)
                    .SetColor(0)
                    .SetUserId(userId);
                room.Players.Add(player);
                DataMgr.Instance.RoomList.Rooms.Add(room);

                netProvider.SendToAsync(PtMessagePackage.Build((ushort)ResponseMessageId.GS_CreateRoom, PtRoom.Write(room)),evt.SessionId);

            }
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
                PtRoom ptRoom = DataMgr.Instance.RoomList.Rooms.Find(room => room.RoomId == roomId);
                if (ptRoom != null)
                {
                    if (type == 0)
                    {
                        userId = buffer.ReadString();
                        byte teamId = buffer.ReadByte();
                        PtRoomPlayer ptRoomPlayer = ptRoom.Players.Find(player => player.UserId == userId);
                        if (ptRoomPlayer != null)
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

                    }
                    OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_UpdateRoom, PtRoom.Write(ptRoom));
                }
            }
        }
        bool OnUpdateDataToRoomMember(PtRoom ptRoom, ushort messageId, byte[] bytes)
        {
            bool ret = false;
            if (ptRoom.HasPlayers())
            {
                ptRoom.Players.ForEach((player) =>
                {
                    DataMgr.Instance.Users.ForEach((e) =>
                    {
                        if (e.UserId == player.UserId)
                        {
                            ret = true;
                            netProvider.SendToAsync(PtMessagePackage.Build( messageId, bytes), e.SessionId);
                        }
                    });
                });
            }
            return ret;
        }

        void OnLeaveRoomImpl(uint roomId, User user)
        {
            LogInfo($"OnLeaveRoom Room Id:{roomId} by leave room CurrentRoomListCount hasRoom:{DataMgr.Instance.RoomList.HasRooms()}");
            if (DataMgr.Instance.RoomList.HasRooms())
            {
                PtRoom ptRoom = DataMgr.Instance.RoomList.Rooms.Find((room) => room.RoomId == roomId);
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

                    netProvider.SendToAsync(PtMessagePackage.Build((ushort)ResponseMessageId.GS_LeaveRoom, null),user.SessionId);

                    OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_UpdateRoom, PtRoom.Write(ptRoom));

                    if (ptRoom.Players.Count == 0)
                    {
                        DataMgr.Instance.RoomList.Rooms.Remove(ptRoom);
                        LogInfo($"Remove Room Id:{ptRoom.RoomId} by leave room CurrentRoomListCount:{DataMgr.Instance.RoomList.Rooms.Count}");
                    }
                    OnRoomList(user.SessionId);
                }
            }
        }
        void OnLeaveRoom(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                User user = DataMgr.Instance.Users.Find((e) => e.SessionId == evt.SessionId);

                if (user != null)
                {
                    uint roomId = buffer.ReadUInt32();
                    OnLeaveRoomImpl(roomId, user);
                }
            }
        }
        void OnJoinRoom(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                uint roomId = buffer.ReadUInt32();
                string userId = buffer.ReadString();
                string userName = buffer.ReadString();
                string userPwd = buffer.ReadString();
                byte teamId = buffer.ReadByte();
                var ptRoom = DataMgr.Instance.RoomList.Rooms.Find((room) => room.RoomId == roomId);
                if (ptRoom != null)
                {
                    if (ptRoom.Status == 0)
                    {
                        //房间是等待状态，这个状态下玩家可以进入房间，并且等待房主开始游戏
                        var ptPlayer = ptRoom.Players.Find((player) => player.UserId == userId);
                        if (ptPlayer == null)
                        {
                            ptRoom.Players.Add(new PtRoomPlayer()
                                .SetEntityId(2)
                                .SetType((byte)Misc.RoomPlayerType.Player)
                                .SetNickName(userName)
                                .SetPassword(userPwd)
                                .SetStatus(1)
                                .SetTeamId(teamId)
                                .SetColor((byte)ptRoom.Players.Count)
                                .SetUserId(userId));
                        }

                        netProvider.SendToAsync(PtMessagePackage.Build((ushort)ResponseMessageId.GS_JoinRoom, new ByteBuffer().WriteByte(0).GetRawBytes()),evt.SessionId);
                        OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_UpdateRoom, PtRoom.Write(ptRoom));
                    }
                    else if (ptRoom.Players.Exists(p => p.NickName == userName))
                    {
                        //当前房间状态是正在战斗中，此时如果有房间内的玩家由于 断线原因重新进入房间

                        RoomProcess roomProcess = GetRoomProcess(ptRoom.RoomId);
                        if (roomProcess != null)
                        {
                            netProvider.SendToAsync(PtMessagePackage.Build((ushort)ResponseMessageId.GS_LaunchGame),evt.SessionId);
                            netProvider.SendToAsync(PtMessagePackage.Build((ushort)ResponseMessageId.GS_LaunchRoomInstance, PtLaunchGameData.Write(roomProcess.LaunchGameData)),evt.SessionId);
                        }

                    }
                    else
                    {
                        OnError(evt.SessionId, 100000);
                    }
                }
            }
        }
        async void OnLaunchGame(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                uint roomId = buffer.ReadUInt32();
                if (DataMgr.Instance.RoomList.HasRooms())
                {
                    PtRoom ptRoom = DataMgr.Instance.RoomList.Rooms.Find((room) => room.RoomId == roomId);
                    if (ptRoom != null)
                    {
                        if (ptRoom.HasPlayers())
                        {
                            PtLaunchGameData gameData = new PtLaunchGameData();
                            gameData.SetPlayerNumber((byte)ptRoom.Players.Count);
                            gameData.SetIsStandaloneMode(ptRoom.Players.Count == 1
                                || !ptRoom.Players.Exists(player => player.UserId != ptRoom.RoomOwnerUserId && (player.Type == (byte)Misc.RoomPlayerType.Player)));

                            gameData.SetRSAddress(DataMgr.Instance.StartupConfigs["RoomAddress"]);
                            gameData.SetRSPort(GetEmptyRoomPort());
                            gameData.SetMapId(ptRoom.MapId);
                            gameData.SetConnectionKey("BattleBehaviour");

                            OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_LaunchGame, null);
                            if (!gameData.IsStandaloneMode)
                            {
                                await CreateRoomProcess(DataMgr.Instance.StartupConfigs["RoomModuleFullPath"], gameData, ptRoom);
                            }
                            else
                            {
                                DataMgr.Instance.RoomList.Rooms.Remove(ptRoom);
                                LogInfo($"Remove Room Id:{ptRoom.RoomId} by launch game CurrentRoomListCount:{DataMgr.Instance.RoomList.Rooms.Count}");
                            }

                            OnUpdateDataToRoomMember(ptRoom, (ushort)ResponseMessageId.GS_LaunchRoomInstance, PtLaunchGameData.Write(gameData));
                            ptRoom.SetStatus(1);//in battle
                        }
                    }
                }
            }
        }

        ushort GetEmptyRoomPort()
        {
            for (int i = 60000; i < 61000; ++i)
            {
                if (!DataMgr.Instance.ProcessIds.ContainsKey(i))
                    return (ushort)i;
            }
            return 0;
        }


        public Task CreateRoomProcess(string dllPath, PtLaunchGameData launchGameData, PtRoom ptRoom)
        {
            if (DataMgr.Instance.ProcessIds.ContainsKey(launchGameData.RSPort))
            {
                throw new Exception("port is occupied " + launchGameData.RSPort);
            }
            RoomProcess roomProcess = new RoomProcess();
            DataMgr.Instance.ProcessIds[launchGameData.RSPort] = roomProcess;
            roomProcess.LaunchGameData = launchGameData;
            roomProcess.RoomId = ptRoom.RoomId;
            roomProcess.Port = launchGameData.RSPort;
            var psi = new ProcessStartInfo("dotnet", " " + dllPath +
                " -port " + launchGameData.RSPort +
                " -mapId " + ptRoom.MapId +
                " -playernumber " + launchGameData.PlayerNumber +
                " -gateWsServerName " + nameof(RoomBehaviour) +
                " -gateWsPort " + DataMgr.Instance.GateServerWSPort +
                " -hash " + ptRoom.RoomId);
            psi.CreateNoWindow = false;
            psi.UseShellExecute = false;
            Task task = Task.Run(() =>
            {
                DateTime n = DateTime.Now;
                var proc = Process.Start(psi);
                roomProcess.Set(proc);
                LogInfo("Start process in Task " + (DateTime.Now - n).TotalMilliseconds);
            });
            return task;
        }

        RoomProcess GetRoomProcess(uint roomId)
        {
            foreach (RoomProcess roomProcess in DataMgr.Instance.ProcessIds.Values)
            {
                if (roomProcess.RoomId == roomId)
                    return roomProcess;
            }
            return null;
        }

        void OnError(string sessionId, int errorId)
        {
            netProvider.SendToAsync(PtMessagePackage.Build((ushort)ResponseMessageId.GS_ErrorCode, PtErrorCode.Write(new PtErrorCode().SetId(errorId))), sessionId);
        }

        
    }
}
