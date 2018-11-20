using LogicFrameSync.Src.LockStep;
using LogicFrameSync.Src.LockStep.Behaviours;
using LogicFrameSync.Src.LockStep.Net.Pt;
using Net;
using NetServiceImpl.Server.Data;
using Notify;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NetServiceImpl.Server
{
    public class LoginService:Service
    {
        protected override void Init()
        {
            base.Init();
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
                Debug.Log("[server] OnRequestEnterRoom !" + roleId);
            }
        }

        public void ResponseSyncClientKeyFrames(int frameIdx)
        {
            Dictionary<int, PtKeyFrameCollection> dict = new Dictionary<int, PtKeyFrameCollection>();
            while (QueueMsg.Count > 0)
            {
                PtKeyFrameCollection collection = QueueMsg.Dequeue() as PtKeyFrameCollection;
                if (!dict.ContainsKey(collection.FrameIdx))
                    dict[collection.FrameIdx] = new PtKeyFrameCollection() { FrameIdx = collection.FrameIdx,KeyFrames=new List<LogicFrameSync.Src.LockStep.Frame.FrameIdxInfo>()};

                dict[collection.FrameIdx].KeyFrames.AddRange(collection.KeyFrames);              
            }
            foreach(PtKeyFrameCollection pt in dict.Values)
            {
                GameServerNetwork.Instance.Broadcast(PtMessagePackage.Build((int)S2CMessageId.ResponseSyncKeyframes,
                    new ByteBuffer().WriteBytes(PtKeyFrameCollection.Write(pt)).Getbuffer(), false));
            }
        }

        Queue QueueMsg = Queue.Synchronized(new Queue());

        [Subscribe(C2SMessageId.RequestSyncClientKeyframes)]
        void OnRequestSyncClientKeyframes(Notification note)
        {  
            int serverFrameIdx = SimulationManager.Instance.GetSimulation(Const.SERVER_SIMULATION_ID).GetBehaviour<ServerLogicFrameBehaviour>().CurrentFrameIdx;
            Debug.Log("Server Receive ClientKeyFrameData At" + serverFrameIdx);
            Message msg = note.GetMessage();
            ByteBuffer buffer = new ByteBuffer(note.GetBytes());

            PtKeyFrameCollection collection = PtKeyFrameCollection.Read(buffer.ReadBytes());
            foreach (var item in collection.KeyFrames)
                item.Idx = serverFrameIdx;
            collection.FrameIdx = serverFrameIdx;

            //QueueMsg.Enqueue(collection);

            GameServerNetwork.Instance.Broadcast(PtMessagePackage.Build((int)S2CMessageId.ResponseSyncKeyframes,
               new ByteBuffer().WriteBytes(PtKeyFrameCollection.Write(collection)).Getbuffer(),false));
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
