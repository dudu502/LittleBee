using LogicFrameSync.Src.LockStep;
using LogicFrameSync.Src.LockStep.Behaviours;
using LogicFrameSync.Src.LockStep.Net.Pt;
using Net;
using NetServiceImpl.Server.Data;
using Notify;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace NetServiceImpl.Server
{
    public class LoginService:Service
    {
        ConcurrentQueue<PtKeyFrameCollection> QueueKeyFrameCollection;
        Dictionary<int, PtKeyFrameCollection> DictKeyFrames = new Dictionary<int, PtKeyFrameCollection>();
        protected override void Init()
        {
            base.Init();


            //NotifyMgr.Instance.AddListener(C2SMessageId.RequestEnterRoom, OnRequestEnterRoom);
            //NotifyMgr.Instance.AddListener(C2SMessageId.RequestSyncClientKeyframes, OnRequestSyncClientKeyframes);
            //NotifyMgr.Instance.AddListener(C2SMessageId.RequestInitPlayer, OnRequestInitPlayer);
            //NotifyMgr.Instance.AddListener(C2SMessageId.RequestPlayerReady, OnRequestPlayerReady);
            StartRollingSyncMsgThread();
        }
        void StartRollingSyncMsgThread()
        {
            QueueKeyFrameCollection = new ConcurrentQueue<PtKeyFrameCollection>();
            ThreadPool.QueueUserWorkItem((state) => 
            {
                while(true)
                {
                    DictKeyFrames.Clear();
                    PtKeyFrameCollection collection = null;
                    while(QueueKeyFrameCollection.TryDequeue(out collection))
                    {
                        if (!DictKeyFrames.ContainsKey(collection.FrameIdx))
                            DictKeyFrames[collection.FrameIdx] = collection;
                        else
                            DictKeyFrames[collection.FrameIdx].AddKeyFramesRange(collection);
                    }
                    foreach(var value in DictKeyFrames.Values)
                        GameServerNetwork.Instance.Broadcast(PtMessagePackage.Build((int)S2CMessageId.ResponseSyncKeyframes, new ByteBuffer().WriteBytes(PtKeyFrameCollection.Write(value)).Getbuffer(), false));
                    Thread.Sleep(20);
                }            
            });
        }
        public override void Reset()
        {
            base.Reset();
        }

        [Subscribe(C2SMessageId.RequestEnterRoom)]
        void OnRequestEnterRoom(Notification note)
        {
            Message msg = note.GetMessage();
            using (ByteBuffer buff = new ByteBuffer(note.GetBytes()))
            {
                long roleId = buff.ReadLong();
                string name = buff.ReadString();
                int state = GameServerData.EnterGameRoom(roleId, name);
                msg.Reply(PtMessagePackage.Build((int)S2CMessageId.ResponseEnterRoom, 
                    new ByteBuffer().WriteInt32(state).WriteBytes(GameRoomSvo.Write(GameServerData.GameRoom)).Getbuffer()));
                //Debug.Log("[server] OnRequestEnterRoom !" + roleId);
            }
        }

        /// <summary>
        /// 客户端请求同步关键帧数据
        /// </summary>
        /// <param name="note"></param>
        [Subscribe(C2SMessageId.RequestSyncClientKeyframes)]
        void OnRequestSyncClientKeyframes(Notification note)
        {  
            int serverFrameIdx = SimulationManager.Instance.GetSimulation(Const.SERVER_SIMULATION_ID).GetBehaviour<ServerLogicFrameBehaviour>().CurrentFrameIdx;
            Message msg = note.GetMessage();
            ByteBuffer buffer = new ByteBuffer(note.GetBytes());

            PtKeyFrameCollection collection = PtKeyFrameCollection.Read(buffer.ReadBytes());
            foreach (var item in collection.KeyFrames)
                item.Idx = serverFrameIdx;
            collection.FrameIdx = serverFrameIdx;
            QueueKeyFrameCollection.Enqueue(collection);
            //GameServerNetwork.Instance.Broadcast(PtMessagePackage.Build((int)S2CMessageId.ResponseSyncKeyframes,
            //   new ByteBuffer().WriteBytes(PtKeyFrameCollection.Write(collection)).Getbuffer(),false));
        }
        [Subscribe(C2SMessageId.RequestInitPlayer)]
        void OnRequestInitPlayer(Notification note)
        {
            GameServerNetwork.Instance.Broadcast(PtMessagePackage.Build((int)S2CMessageId.ResponseInitPlayer,note.GetBytes()));
        }
        [Subscribe(C2SMessageId.RequestPlayerReady)]
        void OnRequestPlayerReady(Notification note)
        {
            Message msg = note.GetMessage();
            using (ByteBuffer buff = new ByteBuffer(note.GetBytes()))
            {
                long roleId = buff.ReadLong();
                GameServerData.SetGameMemeberReady(roleId);
                GameServerNetwork.Instance.Broadcast(PtMessagePackage.Build((int)S2CMessageId.ResponsePlayerReady,buff.Getbuffer()));

                if(GameServerData.IsAllReadyInGameRoom())
                {
                    ByteBuffer buffer = new ByteBuffer();
                    buffer.WriteInt32(GameServerData.GameRoom.Members.Count);
                    foreach (var mem in GameServerData.GameRoom.Members)
                        buffer.WriteLong(mem.Id);
                    GameServerNetwork.Instance.Broadcast(PtMessagePackage.Build((int)S2CMessageId.ResponseAllPlayerReady, buffer.Getbuffer()));
                }
            } 
        }
    }
}
