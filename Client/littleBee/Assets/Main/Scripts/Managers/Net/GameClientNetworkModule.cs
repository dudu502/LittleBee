using LiteNetLib;
using Net;
using Net.ServiceImpl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Managers
{
    public class GameClientNetworkModule : IModule
    {
        NetManager m_ClientMgr;
        EventBasedNetListener m_Listener;
        private ConcurrentQueue<PtMessagePackage> m_QueueMsg;
        private Thread m_PollEvtThread;
        private bool m_BlPollThreadState;
        private long m_HeartBeatTicks = 0;
        private DateTime m_HeartBeatDateTime;
 
        public void Init()
        {
            m_QueueMsg = new ConcurrentQueue<PtMessagePackage>();
        }

        public void CloseClient()
        {
            m_BlPollThreadState = false;
            if (m_PollEvtThread != null)
            {
                try
                {
                    m_PollEvtThread.Abort();
                    m_PollEvtThread = null;
                }
                catch (Exception) { }
            }

            if (m_ClientMgr != null)
            {
                m_ClientMgr.DisconnectAll();
                m_ClientMgr.Stop();
                m_ClientMgr = null;
            }
            if (m_Listener != null)
            {
                m_Listener.ClearConnectionRequestEvent();
                m_Listener.ClearDeliveryEvent();
                m_Listener.ClearNetworkErrorEvent();
                m_Listener.ClearNetworkLatencyUpdateEvent();
                m_Listener.ClearNetworkReceiveEvent();
                m_Listener.ClearNetworkReceiveUnconnectedEvent();
                m_Listener.ClearPeerConnectedEvent();
                m_Listener.ClearPeerDisconnectedEvent();
                m_Listener = null;
            }
        }

        public void Launch()
        {
            m_HeartBeatTicks = 0;
            m_Listener = new EventBasedNetListener();
            m_ClientMgr = new NetManager(m_Listener);
            m_ClientMgr.UnconnectedMessagesEnabled = true;
            m_Listener.ConnectionRequestEvent += OnConnectionRequestEvent;
            m_Listener.DeliveryEvent += OnDeliveryEvent;
            m_Listener.NetworkErrorEvent += OnNetworkErrorEvent;
            m_Listener.NetworkLatencyUpdateEvent += OnNetworkLatencyUpdateEvent;
            m_Listener.NetworkReceiveEvent += OnNetworkReceiveEvent;
            m_Listener.NetworkReceiveUnconnectedEvent += OnNetworkReceiveUnconnectedEvent;
            m_Listener.PeerConnectedEvent += OnPeerConnectedEvent;
            m_Listener.PeerDisconnectedEvent += OnPeerDisconnectedEvent;
        }

        private void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Debug.Log("OnPeerDisconnectedEvent");
            CloseClient();
        }

        private void OnPeerConnectedEvent(NetPeer peer)
        {
            Debug.Log("OnPeerConnectedEvent");
        }

        private void OnNetworkReceiveUnconnectedEvent(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            byte[] bytes = new byte[reader.AvailableBytes];
            reader.GetBytes(bytes, reader.AvailableBytes);
            PtMessagePackage package = PtMessagePackage.Read(bytes);
            package.ExtraObj = remoteEndPoint;
            m_QueueMsg.Enqueue(package);
            reader.Recycle();
        }

        private void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            byte[] bytes = new byte[reader.AvailableBytes];
            reader.GetBytes(bytes, reader.AvailableBytes);
            PtMessagePackage package = PtMessagePackage.Read(bytes);
            m_QueueMsg.Enqueue(package);
            reader.Recycle();
        }

        private void OnNetworkLatencyUpdateEvent(NetPeer peer, int latency)
        {
            //Debug.Log("OnNetworkLatencyUpdateEvent");
        }

        private void OnNetworkErrorEvent(IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
        {
            Debug.Log("OnNetworkErrorEvent");
            CloseClient();
        }

        private void OnDeliveryEvent(NetPeer peer, object userData)
        {
            Debug.Log("OnDeliveryEvent");
        }

        private void OnConnectionRequestEvent(ConnectionRequest request)
        {
            Debug.Log("OnConnectionRequestEvent");
        }

        public void Start(string ip, int port, string key)
        {
            m_ClientMgr.Start();
            m_ClientMgr.Connect(ip, port, key);
            CreateThreads();
        }
        public void Start()
        {
            m_ClientMgr.Start(50000);
            CreateThreads();
        }
        void CreateThreads()
        {
            if (m_PollEvtThread == null)
            {
                m_BlPollThreadState = true;
                m_PollEvtThread = new Thread(OnPollEvt);
                m_PollEvtThread.IsBackground = true;
                m_PollEvtThread.Start();
            }
        }
        void OnPollEvt(object obj)
        {
            while (m_BlPollThreadState)
            {
                m_ClientMgr.PollEvents();
                Thread.Sleep(15);
                TickDispatchMessages();
                TickHeartBeating();
            }
            try
            {
                m_PollEvtThread.Abort();
                m_PollEvtThread = null;
            }
            catch (Exception) { }

        }
        public void SendRequest(PtMessagePackage package)
        {
            lock (this)
                NetStreamUtil.SendToAll(m_ClientMgr, package);
        }
        public void SendRequest(RequestMessageId c2SMessageId, params object[] p)
        {
            PtMessagePackage package = PtMessagePackage.BuildParams((ushort)c2SMessageId, p);
            SendRequest(package);
        }
        public void SendUnconnectedRequest(PtMessagePackage package, IPEndPoint remotePoint)
        {
            if (m_ClientMgr != null)
                m_ClientMgr.SendUnconnectedMessage(PtMessagePackage.Write(package), remotePoint);
        }
        public void TickDispatchMessages()
        {
            while (m_QueueMsg.Count > 0)
            {
                PtMessagePackage package = null;
                if (m_QueueMsg.TryDequeue(out package))
                {
                    try
                    {
                        Evt.EventMgr<ResponseMessageId, PtMessagePackage>.TriggerEvent((ResponseMessageId)package.MessageId, package);
                    }
                    catch (Exception exc)
                    {
                        Debug.LogError("TickDispatchMessages Error " + exc.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 心跳机制
        /// 每10秒进行一次
        /// </summary>
        void TickHeartBeating()
        {
            //心跳10 secs
            if (m_HeartBeatTicks == 0)
            {
                m_HeartBeatDateTime = DateTime.Now;
            }
            else if (m_HeartBeatTicks > 100000000)//10s = 10*1000*10000
            {
                m_HeartBeatTicks -= 100000000;
                SendRequest(PtMessagePackage.Build((ushort)RequestMessageId.RS_HeartBeat, null));
            }
            m_HeartBeatTicks += (DateTime.Now - m_HeartBeatDateTime).Ticks;
            m_HeartBeatDateTime = DateTime.Now;
        }
    }
}
