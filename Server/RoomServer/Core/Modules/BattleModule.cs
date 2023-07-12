using System.Collections.Generic;
using Net.ServiceImpl;
using Net;
using Net.Pt;
using LiteNetLib;
using System;
using System.Net;
using Synchronize.Game.Lockstep.Service.Modules;
using Synchronize.Game.Lockstep.Service.Core;
using Synchronize.Game.Lockstep.Evt;
using Synchronize.Game.Lockstep.Service.Event;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Frame;
using Synchronize.Game.Lockstep.RoomServer.Core.Data;
using Synchronize.Game.Lockstep.RoomServer.Services.Sim;

namespace Synchronize.Game.Lockstep.RoomServer.Modules
{
    /// <summary>
    /// Battle module
    /// </summary>
    public class BattleModule:BaseModule
    {
        public BattleSession Session { get; private set; }
        public BattleModule(BaseApplication app):base(app)
        {
            Session = new BattleSession();
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.RS_EnterRoom, OnEnterRoom);
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.RS_InitPlayer, OnInitPlayer);
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.RS_PlayerReady, OnPlayerReady);
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.RS_SyncClientKeyframes, OnSyncClientKeyframes);
            EventMgr<RequestMessageId, NetMessageEvt>.AddListener(RequestMessageId.RS_HistoryKeyframes, OnHistoryKeyframes);
            EventMgr<NetActionEvent, NetPeer>.AddListener(NetActionEvent.PeerDisconnectedEvent, OnPeerDisconnectedEvent);
        }
        public override void Dispose()
        {
            EventMgr<RequestMessageId, NetMessageEvt>.RemoveListener(RequestMessageId.RS_EnterRoom, OnEnterRoom);
            EventMgr<RequestMessageId, NetMessageEvt>.RemoveListener(RequestMessageId.RS_InitPlayer, OnInitPlayer);
            EventMgr<RequestMessageId, NetMessageEvt>.RemoveListener(RequestMessageId.RS_PlayerReady, OnPlayerReady);
            EventMgr<RequestMessageId, NetMessageEvt>.RemoveListener(RequestMessageId.RS_SyncClientKeyframes, OnSyncClientKeyframes);
            EventMgr<RequestMessageId, NetMessageEvt>.RemoveListener(RequestMessageId.RS_HistoryKeyframes, OnHistoryKeyframes);
            EventMgr<NetActionEvent, NetPeer>.RemoveListener(NetActionEvent.PeerDisconnectedEvent, OnPeerDisconnectedEvent);
            base.Dispose();
        }
        void OnPeerDisconnectedEvent(NetPeer netPeer)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                UserStateObject userState = Session.FindUserStateByNetPeer(netPeer);
                if (userState != null && Session.StartupCFG.GsPort!=0)
                {
                    userState.IsOnline = false;
                    buffer.WriteInt32(ApplicationInstance.Port);
                    buffer.WriteString(userState.UserName);
                    buffer.WriteBool(Session.HasOnlinePlayer());
                    NetStreamUtil.SendUnconnnectedRequest(GetNetManager(), (ushort)RequestMessageId.UGS_RoomPlayerDisconnect, buffer.Getbuffer(),new IPEndPoint(IPAddress.Parse("127.0.0.1"), Session.StartupCFG.GsPort));
                }
            }
        }
        public void InitStartup(uint mapId,ushort playerNumber,int gsPort,string hash)
        {
            Session.StartupCFG.MapId = mapId;
            Session.StartupCFG.PlayerNumber = playerNumber;
            Session.StartupCFG.GsPort = gsPort;
            Session.StartupCFG.Hash = hash;
            BaseApplication.Logger.Log($"InitStartup MapId:{mapId} PlayerNumber:{playerNumber} Port:{gsPort} Hash:{hash}");
        }
        /// <summary>
        /// 请求进入房间。
        /// 分配给用户EntityId
        /// </summary>
        /// <param name="notification"></param>
        void OnEnterRoom(NetMessageEvt evt)
        {
            using(ByteBuffer buffer = new ByteBuffer(evt.Content))
            {                   
                string name = buffer.ReadString();
                UserStateObject userState = Session.FindUserStateByUserName(name);
                if (userState == null)
                {
                    Session.InitEntityId++;
                    Session.DictUsers[Session.InitEntityId] = new UserStateObject(evt.Peer, UserState.EnteredRoom, name, Session.InitEntityId);
                    bool isFull = Session.DictUsers.Count == Session.StartupCFG.PlayerNumber;
                    BaseApplication.Logger.Log($"OnEnterRoom PlayerNumberNow:{Session.DictUsers.Count} PlayerNumberCFG:{Session.StartupCFG.PlayerNumber} isFull:{isFull} InitEntityId:{Session.InitEntityId}");
                    NetStreamUtil.Send(evt.Peer,
                        PtMessagePackage.Build((ushort)ResponseMessageId.RS_EnterRoom,
                        new ByteBuffer().WriteUInt32(Session.InitEntityId).WriteString(name).WriteString(Session.StartupCFG.Hash).Getbuffer()));
                    if (isFull)
                        NetStreamUtil.SendToAll(GetNetManager(), PtMessagePackage.BuildParams((ushort)ResponseMessageId.RS_AllUserState, (byte)UserState.EnteredRoom, Session.StartupCFG.MapId));
                }
                else
                {
                    userState.Update(evt.Peer, UserState.Re_EnteredRoom);
                    userState.IsOnline = true;
                    NetStreamUtil.Send(evt.Peer,
                        PtMessagePackage.Build((ushort)ResponseMessageId.RS_EnterRoom,
                        new ByteBuffer().WriteUInt32(userState.EntityId).WriteString(name).WriteString(Session.StartupCFG.Hash).Getbuffer()));                    
                    NetStreamUtil.SendToAll(GetNetManager(),PtMessagePackage.BuildParams((ushort)ResponseMessageId.RS_AllUserState,(byte)UserState.Re_EnteredRoom, Session.StartupCFG.MapId,name));
                }
            }
        }

        /// <summary>
        /// 客户端初始化
        /// </summary>
        /// <param name="notification"></param>
        void OnInitPlayer(NetMessageEvt evt)
        {
            using(ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                uint id = buffer.ReadUInt32();
                NetStreamUtil.SendToAll(GetNetManager(), PtMessagePackage.Build((ushort)ResponseMessageId.RS_InitPlayer, new ByteBuffer().WriteUInt32(id).Getbuffer()));
                NetStreamUtil.Send(evt.Peer, (ushort)ResponseMessageId.RS_InitSelfPlayer, null);
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
                UserStateObject user = Session.DictUsers[buffer.ReadUInt32()];
                switch (user.StateFlag)
                {
                    case UserState.EnteredRoom:
                        user.Update(evt.Peer, UserState.BeReadyToEnterScene);
                        bool allReady = true;
                        BaseApplication.Logger.Log("OnRequestPlayerReady PeerId:" + evt.Peer.Id);
                        foreach (UserStateObject userState in Session.DictUsers.Values)
                        {
                            allReady &= (userState.StateFlag == UserState.BeReadyToEnterScene);
                            BaseApplication.Logger.Log("UserState.StateFlag == UserState.State.ReadyForInit:userStateId" + userState.NetPeer.Id + " state:" + (userState.StateFlag));
                        }
                        BaseApplication.Logger.Log("OnPlayerReady allReady :" + allReady + " PlayerNumber:" + Session.DictUsers.Count);
                        if (allReady)
                        {
                            List<uint> userEntityIds = new List<uint>(Session.DictUsers.Keys);
                            userEntityIds.Sort((a, b) => a.CompareTo(b));
                            NetStreamUtil.SendToAll(GetNetManager(), PtMessagePackage.BuildParams((ushort)ResponseMessageId.RS_AllUserState, (byte)UserState.BeReadyToEnterScene, PtUInt32List.Write(new PtUInt32List().SetElements(userEntityIds))));
                            Synchronize.Game.Lockstep.RoomServer.Services.Sim.SimulationManager.Instance.Start();
                            BaseApplication.Logger.Log("Start Simulation.");
                        }
                        break;
                    case UserState.Re_EnteredRoom:
                        user.Update(evt.Peer, UserState.Re_BeReadyToEnterScene);
                        List<uint> newuserEntityIds = new List<uint>(Session.DictUsers.Keys);
                        newuserEntityIds.Sort((a, b) => a.CompareTo(b));
                        NetStreamUtil.SendToAll(GetNetManager(), PtMessagePackage.BuildParams((ushort)ResponseMessageId.RS_AllUserState,(byte)UserState.Re_BeReadyToEnterScene,user.UserName, PtUInt32List.Write(new PtUInt32List().SetElements(newuserEntityIds))));
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
            using(ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                DateTime date = DateTime.Now;
                int startIndex = buffer.ReadInt32();//todo
                         
                BaseApplication.Logger.Log($"Request FrameIdxAt:{startIndex} HistoryKeyframes details: {General.Utils.Extends.Join(' ', Session.KeyFrameList.Elements, pt => pt.FrameIdx.ToString())}");
                byte[] keyframeRawSource = PtKeyFrameCollectionList.Write(Session.KeyFrameList);
                byte[] compressedKeyframeSource = await SevenZip.Helper.CompressBytesAsync(keyframeRawSource);

                long encodingTicks = (DateTime.Now - date).Ticks;
                byte[] keyframeBytes = new ByteBuffer().WriteInt32(startIndex).WriteInt64(encodingTicks).WriteBytes(compressedKeyframeSource).Getbuffer();
                
                BaseApplication.Logger.Log($"Response KeyFrames CompressRate:{1f*compressedKeyframeSource.Length/keyframeRawSource.Length} RawLength:{keyframeRawSource.Length} CompressedLength:{compressedKeyframeSource.Length} msecs:{(DateTime.Now-date).TotalMilliseconds}");
                NetStreamUtil.Send(evt.Peer, (ushort)ResponseMessageId.RS_HistoryKeyframes, keyframeBytes);
            }        
        }

        /// <summary>
        /// 关键帧数据
        /// </summary>
        /// <param name="evt"></param>
        void OnSyncClientKeyframes(NetMessageEvt evt)
        {
            PtKeyFrameCollection collection = PtKeyFrameCollection.Read(evt.Content);
            foreach(var keyFrame in collection.KeyFrames)
            {
                switch(keyFrame.Cmd)
                {
                    case FrameCommand.SYNC_CREATE_ENTITY:
                        keyFrame.ParamsContent = AppendEntityIdParamsContent(keyFrame.ParamsContent);
                        break;
                    case FrameCommand.SYNC_MOVE:
                        break;
                    default:
                        BaseApplication.Logger.Log("OnSyncClientKeyframes.KeyFrameCMD TODO"+keyFrame.Cmd);
                        break;
                }
            }
            Session.QueueKeyFrameCollection.Enqueue(collection);          
        }
        byte[] AppendEntityIdParamsContent(byte[] content)
        {
            EntityType type = (EntityType)content[0];
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteByte(content[0]).WriteUInt32(Session.EntityTypeUids[type]++).Getbuffer();
            }
        }
        /// <summary>
        /// 把缓存的关键帧刷新给各个客户端
        /// </summary>
        /// <param name="currentFrameIdx"></param>
        public void FlushKeyFrame(int currentFrameIdx)
        {
            Session.KeyFrameList.SetCurrentFrameIndex(currentFrameIdx);
            if (Session.QueueKeyFrameCollection.Count == 0) return;
            PtKeyFrameCollection flushCollection = new PtKeyFrameCollection() { FrameIdx = currentFrameIdx, KeyFrames = new List<FrameIdxInfo>() };
            while (Session.QueueKeyFrameCollection.TryDequeue(out PtKeyFrameCollection collection))
            {
                collection.FrameIdx = currentFrameIdx;
                flushCollection.AddKeyFramesRange(collection);
            }
            flushCollection.KeyFrames.Sort();
            Session.KeyFrameList.Elements.Add(flushCollection);
            if (flushCollection.KeyFrames.Count > 0)
                NetStreamUtil.SendToAll(GetNetManager(), PtMessagePackage.Build((ushort)ResponseMessageId.RS_SyncKeyframes, PtKeyFrameCollection.Write(flushCollection)));
        }
    }
}
