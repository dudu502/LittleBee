using LogicFrameSync.Src.LockStep;
using LogicFrameSync.Src.LockStep.Net.Pt;
using Net;
using NetServiceImpl.Client.Data;
using NetServiceImpl.Server.Data;
using Notify;
using System.Collections.Concurrent;
using UnityEngine;

namespace NetServiceImpl.Client
{
   
    public class LoginService:Service
    {
        public ConcurrentQueue<PtKeyFrameCollection> QueueKeyFrameCollection;
        public int KeyframesCount = 0;
        public int AllFramesCount = 0;
        protected override void Init()
        {
            base.Init();
            QueueKeyFrameCollection = new ConcurrentQueue<PtKeyFrameCollection>();

            //NotifyMgr.Instance.AddListener(S2CMessageId.ResponseClientConnected, OnResponseClientConnected);
            //NotifyMgr.Instance.AddListener(S2CMessageId.ResponseEnterRoom, OnResponseEnterRoom);
            //NotifyMgr.Instance.AddListener(S2CMessageId.ResponseSyncKeyframes, OnResponseSyncKeyframes);
            //NotifyMgr.Instance.AddListener(S2CMessageId.ResponseInitPlayer, OnResponseInitPlayer);
            //NotifyMgr.Instance.AddListener(S2CMessageId.ResponsePlayerReady, OnResponsePlayerReady);
            //NotifyMgr.Instance.AddListener(S2CMessageId.ResponseAllPlayerReady, OnResponseAllPlayerReady);
        }
        
        #region 连接成功
        [Subscribe(S2CMessageId.ResponseClientConnected)]
        void OnResponseClientConnected(Notification note)
        {
            long id = new ByteBuffer(note.GetBytes()).ReadLong();
            GameClientData.SelfPlayer.Id = id;
            Debug.Log("OnResponseClientConnected "+id);
            AllUI.Instance.Show("PanelLan");
        }
        #endregion
        #region 进入房间
        public void RequestEnterRoom(string name)
        {
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.Build((int)C2SMessageId.RequestEnterRoom,
                new ByteBuffer().WriteLong(GameClientData.SelfPlayer.Id).WriteString(name).Getbuffer()));
        }
        [Subscribe(S2CMessageId.ResponseEnterRoom)]
        void OnResponseEnterRoom(Notification note)
        {
            ByteBuffer buff = new ByteBuffer(note.GetBytes());
            
            int state = buff.ReadInt32();
            if (state == -1)
            {
                Debug.Log("has same roleid in room");
            }
            else
            {
                GameClientData.GameRoom = GameRoomSvo.Read(buff.ReadBytes());
                Debug.Log(1);
            }
            Debug.Log("[client] OnResponseEnterRoom " );
                                       
        }
        #endregion

        #region 发送关键帧
        public void RequestSyncClientKeyframes(int frameIdx,PtKeyFrameCollection keyframes)
        {
            keyframes.FrameIdx = frameIdx;
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.Build((int)C2SMessageId.RequestSyncClientKeyframes,
               new ByteBuffer().WriteBytes(PtKeyFrameCollection.Write(keyframes))
                               .Getbuffer()));
        }


        [Subscribe(S2CMessageId.ResponseSyncKeyframes)]
        void OnResponseSyncKeyframes(Notification note)
        {
            ByteBuffer buff = new ByteBuffer(note.GetBytes());

            PtKeyFrameCollection collection = PtKeyFrameCollection.Read(buff.ReadBytes());
            

            QueueKeyFrameCollection.Enqueue(collection);
            KeyframesCount++;
            AllFramesCount += collection.KeyFrames.Count;
            //Debug.Log(string.Format("[client receive]  frameIdx:{0}",  collection.FrameIdx));                     
        }
        #endregion
        #region 发送玩家准备
        public void RequestInitPlayer()
        {
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.Build((int)C2SMessageId.RequestInitPlayer,
                new ByteBuffer().WriteLong(GameClientData.SelfPlayer.Id).Getbuffer()));
        }
        public void RequestPlayerReady()
        {
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.Build((int)C2SMessageId.RequestPlayerReady,
                new ByteBuffer().WriteLong(GameClientData.SelfPlayer.Id).Getbuffer()));
        }

        [Subscribe(S2CMessageId.ResponseInitPlayer)]
        void OnResponseInitPlayer(Notification note)
        {
            Debug.Log("------------OnResponseInitPlayer---------");
            ByteBuffer buff = new ByteBuffer(note.GetBytes());
            
            long roleId = buff.ReadLong();

                //Send(AppMain.Notifications.InitPlayer, roleId);
            
        
        }
        [Subscribe(S2CMessageId.ResponsePlayerReady)]
        void OnResponsePlayerReady(Notification note)
        {
            Debug.Log("------------OnResponsePlayerReady---------");
            
        }
        [Subscribe(S2CMessageId.ResponseAllPlayerReady)]
        void OnResponseAllPlayerReady(Notification note)
        {
            Debug.Log("------------OnResponseAllPlayerReady---------");
            ByteBuffer buff = new ByteBuffer(note.GetBytes());
            
            int memberSize = buff.ReadInt32();
            for (int i = 0; i < memberSize; ++i)
            {
                Send(AppMain.Notifications.ReadyPlayerAndAdd, buff.ReadLong());
            }                
            
            SimulationManager.Instance.Start();
        }
        #endregion

        public override void Reset()
        {
            base.Reset();
        }
    }
}
