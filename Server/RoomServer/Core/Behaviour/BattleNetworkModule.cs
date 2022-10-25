using Misc;
using Net;
using Net.Pt;
using Net.ServiceImpl;
using RoomServer.Core.Data;
using RoomServer.Services;
using RoomServer.Services.Sim;
using ServerDll.Service.Behaviour;
using Service.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace RoomServer.Core.Behaviour
{
    public class BattleNetworkModule : NetworkModule
    {
        public BattleNetworkModule(WebSocketServer wssParam) : base(wssParam)
        {
            Evt.EventMgr<RequestMessageId,NetMessageEvt>.AddListener(RequestMessageId.RS_EnterRoom, OnEnterRoom);
            Evt.EventMgr<RequestMessageId,NetMessageEvt>.AddListener(RequestMessageId.RS_InitPlayer, OnInitPlayer);
            Evt.EventMgr<RequestMessageId,NetMessageEvt>.AddListener(RequestMessageId.RS_PlayerReady, OnPlayerReady);
            Evt.EventMgr<RequestMessageId,NetMessageEvt>.AddListener(RequestMessageId.RS_SyncClientKeyframes, OnSyncClientKeyframes);
            Evt.EventMgr<RequestMessageId,NetMessageEvt>.AddListener(RequestMessageId.RS_HistoryKeyframes, OnHistoryKeyframes);
            Evt.EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.SOCKET_MSG_ONOPEN, OnOpen);
            Evt.EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.SOCKET_MSG_ONCLOSE, OnPlayerDisconnect);
            Evt.EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.SOCKET_MSG_ONERROR, OnPlayerDisconnect);

        }

        void OnOpen(NetMessageEvt evt)
        {
            SendToAsync<BattleBehaviour>((ushort)ResponseMessageId.RS_ClientConnected, null,evt.SessionId);
        }
        /// <summary>
        /// 请求进入房间。
        /// 分配给用户EntityId
        /// </summary>
        /// <param name="notification"></param>
        void OnEnterRoom(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                string name = buffer.ReadString();
                Data.UserData userState = DataMgr.Instance.BattleSession.FindUserStateByUserName(name);
                if (userState == null)
                {
                    DataMgr.Instance.BattleSession.InitEntityId++;
                    DataMgr.Instance.BattleSession.DictUsers[DataMgr.Instance.BattleSession.InitEntityId] = new UserData(evt.SessionId, Misc.UserState.EnteredRoom, name, DataMgr.Instance.BattleSession.InitEntityId);
                    bool isFull = DataMgr.Instance.BattleSession.DictUsers.Count == DataMgr.Instance.BattleSession.StartupCFG.PlayerNumber;
                    LogInfo($"OnEnterRoom PlayerNumberNow:{DataMgr.Instance.BattleSession.DictUsers.Count} PlayerNumberCFG:{DataMgr.Instance.BattleSession.StartupCFG.PlayerNumber} isFull:{isFull} InitEntityId:{DataMgr.Instance.BattleSession.InitEntityId}");
 
                    SendToAsync<BattleBehaviour>(PtMessagePackage.Build((ushort)ResponseMessageId.RS_EnterRoom, new ByteBuffer().WriteUInt32(DataMgr.Instance.BattleSession.InitEntityId).WriteString(name).WriteString(DataMgr.Instance.BattleSession.StartupCFG.Hash).GetRawBytes()),evt.SessionId);
                    if (isFull)
                    {
                        BroadcastAsync<BattleBehaviour>(PtMessagePackage.BuildParams((ushort)ResponseMessageId.RS_AllUserState, (byte)UserState.EnteredRoom, DataMgr.Instance.BattleSession.StartupCFG.MapId));
                    }
                }
                else
                {
                    userState.Update(evt.SessionId, Misc.UserState.Re_EnteredRoom);
                    userState.IsOnline = true;
                   
                    SendToAsync<BattleBehaviour>(PtMessagePackage.Build((ushort)ResponseMessageId.RS_EnterRoom,new ByteBuffer().WriteUInt32(userState.EntityId).WriteString(name).WriteString(DataMgr.Instance.BattleSession.StartupCFG.Hash).GetRawBytes()),evt.SessionId);
                    BroadcastAsync<BattleBehaviour>(PtMessagePackage.BuildParams((ushort)ResponseMessageId.RS_AllUserState, (byte)UserState.Re_EnteredRoom, DataMgr.Instance.BattleSession.StartupCFG.MapId, name));
                }
            }
        }

        /// <summary>
        /// 客户端初始化
        /// </summary>
        /// <param name="notification"></param>
        void OnInitPlayer(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                uint id = buffer.ReadUInt32();
             
                BroadcastAsync<BattleBehaviour>(PtMessagePackage.Build((ushort)ResponseMessageId.RS_InitPlayer, new ByteBuffer().WriteUInt32(id).GetRawBytes()));
               
                SendToAsync<BattleBehaviour>((ushort)ResponseMessageId.RS_InitSelfPlayer, null,evt.SessionId);
            }
        }
        /// <summary>
        /// 客户端准备就绪只等开始
        /// </summary>
        /// <param name="notification"></param>
        void OnPlayerReady(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                UserData user = DataMgr.Instance.BattleSession.DictUsers[buffer.ReadUInt32()];
                switch (user.StateFlag)
                {
                    case UserState.EnteredRoom:
                        user.Update(evt.SessionId, UserState.BeReadyToEnterScene);
                        bool allReady = true;
                        LogInfo("OnRequestPlayerReady SessionId:" + evt.SessionId);
                        foreach (UserData userState in DataMgr.Instance.BattleSession.DictUsers.Values)
                        {
                            allReady &= (userState.StateFlag == UserState.BeReadyToEnterScene);
                            LogInfo("UserState.StateFlag == UserState.State.ReadyForInit:userStateId" + userState.SessionId + " state:" + (userState.StateFlag));
                        }
                        LogInfo("OnPlayerReady allReady :" + allReady + " PlayerNumber:" + DataMgr.Instance.BattleSession.DictUsers.Count);
                        if (allReady)
                        {
                            List<uint> userEntityIds = new List<uint>(DataMgr.Instance.BattleSession.DictUsers.Keys);
                            userEntityIds.Sort((a, b) => a.CompareTo(b));
                        
                            BroadcastAsync<BattleBehaviour>(PtMessagePackage.BuildParams((ushort)ResponseMessageId.RS_AllUserState, (byte)UserState.BeReadyToEnterScene, PtUInt32List.Write(new PtUInt32List().SetElements(userEntityIds))));
                            SimulationManager.Instance.Start();
                            LogInfo("Start Simulation.");
                        }
                        break;
                    case UserState.Re_EnteredRoom:
                        user.Update(evt.SessionId, UserState.Re_BeReadyToEnterScene);
                        List<uint> newuserEntityIds = new List<uint>(DataMgr.Instance.BattleSession.DictUsers.Keys);
                        newuserEntityIds.Sort((a, b) => a.CompareTo(b));
                        
                        BroadcastAsync<BattleBehaviour>(PtMessagePackage.BuildParams((ushort)ResponseMessageId.RS_AllUserState, (byte)UserState.Re_BeReadyToEnterScene, user.UserName, PtUInt32List.Write(new PtUInt32List().SetElements(newuserEntityIds))));
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 响应客户端请求的历史关键帧数据
        /// 主要用于客户端重新进入游戏
        /// </summary>
        /// <param name="evt"></param>
        async void OnHistoryKeyframes(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                DateTime date = DateTime.Now;
                int startIndex = buffer.ReadInt32();//todo

                LogInfo($"Request FrameIdxAt:{startIndex} HistoryKeyframes details: {General.Utils.Extends.Join(' ', DataMgr.Instance.BattleSession.KeyFrameList.Elements, pt => pt.FrameIdx.ToString())}");
                byte[] keyframeRawSource = PtKeyFrameCollectionList.Write(DataMgr.Instance.BattleSession.KeyFrameList);
                byte[] compressedKeyframeSource = await SevenZip.Helper.CompressBytesAsync(keyframeRawSource);

                long encodingTicks = (DateTime.Now - date).Ticks;
                byte[] keyframeBytes = new ByteBuffer().WriteInt32(startIndex).WriteInt64(encodingTicks).WriteBytes(compressedKeyframeSource).GetRawBytes();
                LogInfo($"Response KeyFrames CompressRate:{1f * compressedKeyframeSource.Length / keyframeRawSource.Length} RawLength:{keyframeRawSource.Length} CompressedLength:{compressedKeyframeSource.Length} msecs:{(DateTime.Now - date).TotalMilliseconds}");
               
                SendToAsync<BattleBehaviour>(PtMessagePackage.Build((ushort)ResponseMessageId.RS_HistoryKeyframes, keyframeBytes), evt.SessionId);
            }
        }

        /// <summary>
        /// 关键帧数据
        /// </summary>
        /// <param name="evt"></param>
        void OnSyncClientKeyframes(NetMessageEvt evt)
        {
            PtKeyFrameCollection collection = PtKeyFrameCollection.Read(evt.Content);
            foreach (var keyFrame in collection.KeyFrames)
            {
                switch (keyFrame.Cmd)
                {
                    case Frame.FrameCommand.SYNC_CREATE_ENTITY:
                        keyFrame.ParamsContent = AppendEntityIdParamsContent(keyFrame.ParamsContent);
                        break;
                    case Frame.FrameCommand.SYNC_MOVE:
                        break;
                    default:
                        LogInfo("OnSyncClientKeyframes.KeyFrameCMD TODO" + keyFrame.Cmd);
                        break;
                }
            }
            DataMgr.Instance.BattleSession.QueueKeyFrameCollection.Enqueue(collection);
        }
        byte[] AppendEntityIdParamsContent(byte[] content)
        {
            EntityType type = (EntityType)content[0];
            using (ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteByte(content[0]).WriteUInt32(DataMgr.Instance.BattleSession.EntityTypeUids[type]++).GetRawBytes();
            }
        }

        void OnPlayerDisconnect(NetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                UserData userData = DataMgr.Instance.BattleSession.FindUserStateBySessionId(evt.SessionId);
                if (userData != null && DataMgr.Instance.BattleSession.StartupCFG.GateWsPort != 0)
                {
                    userData.IsOnline = false;
                    buffer.WriteInt32(DataMgr.Instance.BattleSession.StartupCFG.Port);
                    buffer.WriteString(userData.UserName);
                    buffer.WriteBool(DataMgr.Instance.BattleSession.HasOnlinePlayer());
                    Evt.EventMgr<RoomApplicationEventType, byte[]>
                        .TriggerEvent(RoomApplicationEventType.PlayerDisconnect, buffer.GetRawBytes());
                }
            }
        }
    }
}
