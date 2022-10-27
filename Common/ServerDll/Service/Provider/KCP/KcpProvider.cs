using kcp2k;
using Net;
using Net.ServiceImpl;
using Service.Core;
using Service.Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ServerDll.Service.Provider.KCP
{
    public class KcpProvider : IProvider
    {
        private KcpServer kcpServer;
        private readonly ConcurrentQueue<NetMessageEvt> netMessageEvts = new ConcurrentQueue<NetMessageEvt>();

        public void BroadcastAsync(byte[] raw)
        {
            kcpServer.Broadcast(new ArraySegment<byte>(raw),KcpChannel.Reliable);
        }

        public void BroadcastAsync( PtMessagePackage package)
        {
            BroadcastAsync(PtMessagePackage.Write(package));
        }

        public void EnqueueNetworkEvents(NetMessageEvt evt)
        {
            netMessageEvts.Enqueue(evt);
        }

        public void PollEvents()
        {
            kcpServer.Tick();

            while (netMessageEvts.TryDequeue(out var evt))
                Evt.EventMgr<RequestMessageId, NetMessageEvt>.TriggerEvent(evt.MessageId, evt);
        }

        public void SendToAsync(byte[] raw, string id)
        {
            if (int.TryParse(id,out int connectionId))
            {
                kcpServer.Send(connectionId, new ArraySegment<byte>(raw), KcpChannel.Reliable);
            }
        }

        public void SendToAsync(PtMessagePackage package, string id)
        {
            SendToAsync(PtMessagePackage.Write(package), id);
        }

        public void LogInfo(string message)
        {
            Logger.LogInfo(message);
        }
        public void LogWarning(string message)
        {
            Logger.LogWarning(message);
        }
        public void LogError(string message)
        {
            Logger.LogError(message);
        }
        public void Start(ushort port)
        {
            kcpServer = new KcpServer(_OnConnected,_OnMessage,_OnDisconnected,_OnError,true,true,10);
            kcpServer.Start(port);
        }
        void _OnConnected(int id)
        {
            EnqueueNetworkEvents(new NetMessageEvt(id.ToString(),null,NetMessageEvt.State.OnOpen,RequestMessageId.SOCKET_MSG_ONOPEN));
            LogInfo($"{GetType().Name} OnOpen ID:{id}");
        }
        void _OnMessage(int id, ArraySegment<byte> raw, KcpChannel channel)
        {
            byte[] result = new byte[raw.Count];
            Array.Copy(raw.Array, raw.Offset, result, 0, result.Length);
            PtMessagePackage ptMessagePackage = PtMessagePackage.Read(result);
            if(ptMessagePackage != null)
            {
                EnqueueNetworkEvents(new NetMessageEvt(id.ToString(),ptMessagePackage.Content,NetMessageEvt.State.OnMessage,(RequestMessageId)ptMessagePackage.MessageId));
                LogInfo($"{GetType().Name} OnMsg ID:{id} MsgID:{(RequestMessageId)ptMessagePackage.MessageId}");
            }
        }
        void _OnDisconnected(int id)
        {
            EnqueueNetworkEvents(new NetMessageEvt(id.ToString(),null,NetMessageEvt.State.OnClose,RequestMessageId.SOCKET_MSG_ONCLOSE));
            LogWarning($"{GetType().Name} OnClose ID:{id}");
        }
        void _OnError(int id, ErrorCode error, string reason)
        {
            EnqueueNetworkEvents(new NetMessageEvt(id.ToString(),null,NetMessageEvt.State.OnError,RequestMessageId.SOCKET_MSG_ONERROR));
            LogError($"{GetType().Name} OnError ID:{id}");
        }
        public void Stop()
        {
            kcpServer.Stop();
        }
    }
}
