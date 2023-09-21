
using Net;
using Net.Pt;
using Net.ServiceImpl;
using Synchronize.Game.Lockstep;
using Synchronize.Game.Lockstep.Config.Static;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using Synchronize.Game.Lockstep.Evt;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.Managers.UI;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Net;
using Synchronize.Game.Lockstep.Proxy;
using Synchronize.Game.Lockstep.UI;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Room
{
    public class RoomSession
    {
        public uint Id;
        public string Name;
        public string RoomHash;
        public Dictionary<int, PtKeyFrameCollection> DictKeyframeCollection;
        public int InitIndex = -1;
        public int WriteKeyframeCollectionIndex = -1;
        public ConcurrentQueue<PtKeyFrameCollection> QueueKeyFrameCollection = new ConcurrentQueue<PtKeyFrameCollection>();

        private PtKeyFrameCollection keyFrameCachedCollection = new PtKeyFrameCollection() { KeyFrames = new List<FrameIdxInfo>() };

        public void AddCurrentFrameCommand(int curFrameIdx,ushort cmd,uint entityId, byte[] paramsContent)
        {
            FrameIdxInfo frameInfo = new FrameIdxInfo(cmd,entityId,paramsContent);
            keyFrameCachedCollection.FrameIdx = curFrameIdx;
            keyFrameCachedCollection.KeyFrames.Add(frameInfo);
        }
        public PtKeyFrameCollection GetKeyFrameCachedCollection()
        {
            return keyFrameCachedCollection;
        }
        public void ClearKeyFrameCachedCollection()
        {
            keyFrameCachedCollection.FrameIdx = 0;
            keyFrameCachedCollection.KeyFrames.Clear();
        }
        public override string ToString()
        {
            return $"RoomSession->Id:{Id} Name:{Name} RoomHash:{RoomHash}";
        }
        public void Clear()
        {
            Id = 0;
            Name = string.Empty;
            RoomHash = string.Empty;
            DictKeyframeCollection = null;
            InitIndex = -1;
            WriteKeyframeCollectionIndex = -1;
            QueueKeyFrameCollection = new ConcurrentQueue<PtKeyFrameCollection>();
            ClearKeyFrameCachedCollection();
        }
    }
    public class RoomServiceProxy:NetServiceProxy
    {
        public enum EvtType
        {
            CreateMapElement,
            CreatePlayer,
        }
        public RoomSession Session = new RoomSession();
        public RoomServiceProxy()
        {
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_ClientConnected, OnResponseRoomServerClientConnected);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_EnterRoom, OnResponseEnterRoom);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_SyncKeyframes, OnResponseSyncKeyframes);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_InitPlayer, OnResponseInitPlayer);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_InitSelfPlayer, OnResponseInitSelfPlayer);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_PlayerReady, OnResponsePlayerReady);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_AllUserState, OnResponseAllUserState);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_HistoryKeyframes, OnResponseHistoryKeyframes);
        }
        
        void OnResponseRoomServerClientConnected(PtMessagePackage package) 
        {
            Debug.Log("OnResponseRoomServerClientConnected");
            string name = DataProxy.Get<UserDataProxy>().GetUserName();
            RequestEnterRoom(name);
        }     

        #region 进入房间
        public void RequestEnterRoom(string name)
        {
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.Build((ushort)RequestMessageId.RS_EnterRoom,
                new ByteBuffer().WriteString(name).Getbuffer()));
        }

        void OnResponseEnterRoom(PtMessagePackage package)
        {
            using (ByteBuffer buff = new ByteBuffer(package.Content))
            {
                Session.Id = buff.ReadUInt32();
                Session.Name = buff.ReadString();
                Session.RoomHash = buff.ReadString();
                Debug.Log($"[client] OnResponseEnterRoom Id:{Session.Id} Name:{Session.Name} Hash:{Session.RoomHash}");
            }
        }



        #endregion
        #region 发送玩家准备
        /// <summary>
        /// step 1 准备阶段
        /// </summary>
        public void RequestInitPlayer()
        {
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.Build((ushort)RequestMessageId.RS_InitPlayer,
                new ByteBuffer().WriteUInt32(Session.Id).Getbuffer()));
        }
        public void RequestPlayerReady()
        {
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.Build((ushort)RequestMessageId.RS_PlayerReady,
                new ByteBuffer().WriteUInt32(Session.Id).Getbuffer()));
        }

        void OnResponseInitPlayer(PtMessagePackage package)
        {
            using (ByteBuffer buff = new ByteBuffer(package.Content))
            {
                uint id = buff.ReadUInt32();
                Debug.Log("------------OnResponseInitPlayer---------"+id);
            }
        }
        void OnResponseInitSelfPlayer(PtMessagePackage package)
        {
            RequestPlayerReady();
            //TriggerMainThreadEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Language.GetText(21), 1f));
        }

        void OnResponsePlayerReady(PtMessagePackage package)
        {
            Debug.Log("------------OnResponsePlayerReady---------");
        }

        /// <summary>
        /// 房间内的所有玩家状态切换
        /// </summary>
        /// <param name="package"></param>
        async void OnResponseAllUserState(PtMessagePackage package)
        {
            using(ByteBuffer buffer = new ByteBuffer(package.Content))
            {
                UserState state = (UserState)buffer.ReadByte();
                Debug.Log("------------OnResponseAllUserState---------" + state);
                switch (state)
                {                   
                    case UserState.EnteredRoom://所有的玩家进入roomserver
                        uint mapId = buffer.ReadUInt32();
                        MapIdCFG mapCfg = ModuleManager.GetModule<ConfigModule>().GetConfig<MapIdCFG>((int)mapId);
                        var sim = BattleEntryPoint.CreateClientSimulation();
                        
                        sim.GetEntityWorld().SetMeta(Meta.META_KEY_MAPID, mapId.ToString());
                        await EntityManager.CreateMapEntity(sim.GetEntityWorld(), mapCfg.ResKey);
                        RequestInitPlayer();
                        break;
                    case UserState.Re_EnteredRoom://有一个玩家重新进入房间状态
                        uint re_mapId = buffer.ReadUInt32();
                        string re_name = buffer.ReadString();
                        if (re_name == Session.Name)
                        {
                            MapIdCFG re_mapCfg = ModuleManager.GetModule<ConfigModule>().GetConfig<MapIdCFG>((int)re_mapId);
                            var re_sim = BattleEntryPoint.CreateClientSimulation();
                            re_sim.GetEntityWorld().SetMeta(Meta.META_KEY_MAPID, re_mapId.ToString());
                            await EntityManager.CreateMapEntity(re_sim.GetEntityWorld(), re_mapCfg.ResKey);
                            RequestInitPlayer();
                        }                       
                        break;
                    case UserState.BeReadyToEnterScene://所有的玩家完成初始化并且准备进入场景
                        DateTime now = DateTime.Now;
                        var playerIds = PtUInt32List.Read(buffer.ReadBytes());
                        SimulationManager.Instance.GetSimulation().GetEntityWorld()
                            .SetMeta(Meta.META_KEY_PLAYER_ENTITYIDS,playerIds.Elements);
                        playerIds.Elements.ForEach(entityId =>
                            EntityManager.CreatePlayerEntity(SimulationManager.Instance.GetSimulation().GetEntityWorld(), entityId,entityId==Session.Id));
                        SimulationManager.Instance.Start(now);
                        Handler.Run(_ => 
                                {
                                    EventMgr<LoadingPanel.EventType, object>.TriggerEvent(LoadingPanel.EventType.ClosePanel, null);
                                    EventMgr<RoomPanel.EventType, object>.TriggerEvent(RoomPanel.EventType.ClosePanel, null);
                                    ModuleManager.GetModule<UIModule>().Push(UITypes.BattlePanel, Layer.Bottom, PlayBattleMode.PlayRealBattle); 
                                }
                        , null);
                        break;
                    case UserState.Re_BeReadyToEnterScene://有一个玩家完成初始化并且准备进入场景
                        string re_beReadyname = buffer.ReadString();
                        if (re_beReadyname == Session.Name)
                        {
                            var re_playerIds = PtUInt32List.Read(buffer.ReadBytes());
                            SimulationManager.Instance.GetSimulation().GetEntityWorld()
                                .SetMeta(Meta.META_KEY_PLAYER_ENTITYIDS,re_playerIds.Elements);
                            re_playerIds.Elements.ForEach(entityId=>
                                EntityManager.CreatePlayerEntity(SimulationManager.Instance.GetSimulation().GetEntityWorld(), entityId,entityId == Session.Id));                  
                            RequestHistoryKeyframes();
                        }
              
                        break;
                    default:
                        Debug.Log("------------OnResponseAllUserState TODO---------" + state);
                        break;
                }
            }
        }

        async void OnResponseHistoryKeyframes(PtMessagePackage package)
        {
            using (ByteBuffer buffer = new ByteBuffer(package.Content))
            {
                DateTime startDate = DateTime.Now;
                Session.InitIndex = buffer.ReadInt32();
                long encodingTicks = buffer.ReadInt64();
                #region Decompress keyframe datas
                var list = PtKeyFrameCollectionList.Read(await SevenZip.Helper.DecompressBytesAsync(buffer.ReadBytes()));
                Session.WriteKeyframeCollectionIndex = list.CurrentFrameIndex ;//-startIndex;
                Session.DictKeyframeCollection = new Dictionary<int, PtKeyFrameCollection>();
                list.Elements.ForEach(e => Session.DictKeyframeCollection.Add(e.FrameIdx, e));
                #endregion
                int offset = Session.InitIndex == -1 ? 0 : Session.InitIndex;
                startDate -= DateTime.Now - startDate + new TimeSpan(encodingTicks);
                SimulationManager.Instance.Start(startDate, Session.WriteKeyframeCollectionIndex - offset,
                    process => TriggerMainThreadEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Localization.Localization.GetTranslation("Synchronizing key frames"), process)),
                    () => Handler.Run(_ =>
                    {
                        EventMgr<LoadingPanel.EventType, object>.TriggerEvent(LoadingPanel.EventType.ClosePanel, null);
                        EventMgr<RoomPanel.EventType, object>.TriggerEvent(RoomPanel.EventType.ClosePanel, null);
                        ModuleManager.GetModule<UIModule>().Push(UITypes.BattlePanel, Layer.Bottom, PlayBattleMode.PlayRealBattle);
                    }
                    , null));
            }
        }

        /// <summary>
        /// 需要扩展
        /// 加上 从服务器获取部分指令与本地快照模式组合的方式。这样可以减少断线重连的加载时间
        /// </summary>
        async void RequestHistoryKeyframes()
        {
            int startIndex = -1;
            
            //find cache ewfd files and await loading ewfd files.
            string worldsnapFolderPath = BattleEntryPoint.PersistentDataPath + "/WorldSnapshots/" + Session.RoomHash + "/" + Session.Name;
            if (System.IO.Directory.Exists(worldsnapFolderPath))
            {
                string[] files = System.IO.Directory.GetFiles(worldsnapFolderPath, Const.EXTENSION_TYPE_PATTERN_SNAP);
                if(files.Length>0)
                {
                    List<int> fileNameSortList = new List<int>();
                    foreach (string fileName in files)
                    {
                        int fileNameToInt32 = Convert.ToInt32(System.IO.Path.GetFileNameWithoutExtension(fileName));
                        fileNameSortList.Add(fileNameToInt32);
                    }
                    fileNameSortList.Sort();
                    if (fileNameSortList.Count > 1)
                    {
                        startIndex = fileNameSortList[fileNameSortList.Count - 2];//取出第二新的文件！

                        EntityWorldFrameData cacheEntityWorldFrameData = EntityWorldFrameData.Read(
                            await System.Threading.Tasks.Task.Run(() => File.ReadAllBytes($"{worldsnapFolderPath}/{startIndex}{Const.EXTENSION_TYPE_SNAP}")));
                        SimulationManager.Instance.GetSimulation().GetEntityWorld().RestoreWorld(cacheEntityWorldFrameData);
                    }
                }          
            }
            GameClientNetwork.Instance.SendRequest(RequestMessageId.RS_HistoryKeyframes, startIndex);
        }

        #endregion
        #region 发送关键帧
        public void RequestSyncClientKeyframes(int frameIdx, PtKeyFrameCollection keyframes)
        {
            keyframes.FrameIdx = frameIdx;
            GameClientNetwork.Instance.SendRequest(
                PtMessagePackage.Build((ushort)RequestMessageId.RS_SyncClientKeyframes, PtKeyFrameCollection.Write(keyframes)));
        }


        void OnResponseSyncKeyframes(PtMessagePackage package)
        {
            Session.QueueKeyFrameCollection.Enqueue(PtKeyFrameCollection.Read(package.Content));
        }
        #endregion

    }
}

