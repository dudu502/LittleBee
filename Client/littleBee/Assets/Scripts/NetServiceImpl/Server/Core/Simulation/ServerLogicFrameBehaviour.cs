using Net;
using Net.Pt;
using Net.ServiceImpl;
using RoomServer.Core.Data;
using System.Collections.Generic;

namespace RoomServer.Services.Sim
{
    public class ServerLogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim
        {
            set; get;
        }
        
        private int m_CurrentFrameIdx;

        private WebSocketSharp.Server.WebSocketServer _webSocketServer;
        public ServerLogicFrameBehaviour(WebSocketSharp.Server.WebSocketServer server)
        {
            _webSocketServer = server;
        }
        public void Quit()
        {

        }
        public void Start()
        {
            m_CurrentFrameIdx = -1;
        }

        public void Update()
        {
            FlushKeyFrame(++m_CurrentFrameIdx);
        }

        void FlushKeyFrame(int currentFrameIdx)
        {
            DataMgr.Instance.BattleSession.KeyFrameList.SetCurrentFrameIndex(currentFrameIdx);
            if (DataMgr.Instance.BattleSession.QueueKeyFrameCollection.Count == 0) return;
            PtKeyFrameCollection flushCollection = new PtKeyFrameCollection() { FrameIdx = currentFrameIdx, KeyFrames = new List<FrameIdxInfo>() };
            while (DataMgr.Instance.BattleSession.QueueKeyFrameCollection.TryDequeue(out PtKeyFrameCollection collection))
            {
                collection.FrameIdx = currentFrameIdx;
                flushCollection.AddKeyFramesRange(collection);
            }
            flushCollection.KeyFrames.Sort();
            DataMgr.Instance.BattleSession.KeyFrameList.Elements.Add(flushCollection);
            if (flushCollection.KeyFrames.Count > 0)
            {
                //BroadcastAsync(PtMessagePackage.Build((ushort)ResponseMessageId.RS_SyncKeyframes, PtKeyFrameCollection.Write(flushCollection)));
                _webSocketServer.WebSocketServices.Broadcast(PtMessagePackage.Write( PtMessagePackage.Build((ushort)ResponseMessageId.RS_SyncKeyframes,PtKeyFrameCollection.Write(flushCollection))));
            }
        }
    }
}
