using LiteNetLib;
using Synchronize.Game.Lockstep.Evt;
using Synchronize.Game.Lockstep.Service.Modules;
using System.Collections.Generic;
using System.Threading;

namespace Synchronize.Game.Lockstep.Service.Core
{
    public enum NetActionEvent
    {
        CallCustomEvent,
        PeerDisconnectedEvent,
        PeerConnectedEvent,
        NetworkReceiveUnconnectedEvent,
        NetworkReceiveEvent,
        NetworkLatencyUpdateEvent,
        NetworkErrorEvent,
        DeliveryEvent,
        ConnectionRequestEvent,
    }

    public class BaseApplication
    {
        public int Port { private set; get; }
        protected NetManager m_Network;
        protected EventBasedNetListener m_NetListener;
        protected string m_ApplicationKey;
        public static ILogger Logger;
        private Dictionary<string, object> m_ConfigMaps;
        public BaseApplication(string key)
        {
            m_ConfigMaps = new Dictionary<string, object>();
            m_ApplicationKey = key;
            Logger.Log(string.Format("Application [{0}] Initialize ApplicationKey:{1}.", GetType().ToString(), m_ApplicationKey));
            SetUp();
            Logger.Log("Application has Setup.");
            m_NetListener = new EventBasedNetListener();
            m_NetListener.ConnectionRequestEvent += OnConnectionRequestEvent;
            m_NetListener.DeliveryEvent += OnDeliveryEvent;
            m_NetListener.NetworkErrorEvent += OnNetworkErrorEvent;
            m_NetListener.NetworkLatencyUpdateEvent += OnNetworkLatencyUpdateEvent;
            m_NetListener.NetworkReceiveEvent += OnNetworkReceiveEvent;
            m_NetListener.NetworkReceiveUnconnectedEvent += OnNetworkReceiveUnconnectedEvent;
            m_NetListener.PeerConnectedEvent += OnPeerConnectedEvent;
            m_NetListener.PeerDisconnectedEvent += OnPeerDisconnectedEvent;
            m_Network = new NetManager(m_NetListener);
        }
        public void AddConfigElement(string key,object data)
        {
            m_ConfigMaps[key] = data;
        }
        public T GetConfigElement<T>(string key)
        {
            if (m_ConfigMaps.ContainsKey(key))
            {
                return (T)m_ConfigMaps[key];
            }
            return default(T);
        }
        public string GetApplicationKey() { return m_ApplicationKey; }
        public NetManager GetNetManager() { return m_Network; }
        #region Event Call
        protected virtual void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            EventMgr<NetActionEvent, NetPeer>.TriggerEvent(NetActionEvent.PeerDisconnectedEvent, peer);
        }

        protected virtual void OnPeerConnectedEvent(NetPeer peer)
        {
            EventMgr<NetActionEvent, NetPeer>.TriggerEvent(NetActionEvent.PeerConnectedEvent, peer);
        }

        protected virtual void OnNetworkReceiveUnconnectedEvent(System.Net.IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            EventMgr<NetActionEvent, System.Net.IPEndPoint>.TriggerEvent(NetActionEvent.NetworkReceiveUnconnectedEvent, remoteEndPoint);
        }

        protected virtual void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            EventMgr<NetActionEvent, NetPeer>.TriggerEvent(NetActionEvent.NetworkReceiveEvent, peer);
        }

        protected virtual void OnNetworkLatencyUpdateEvent(NetPeer peer, int latency)
        {
            EventMgr<NetActionEvent, NetPeer>.TriggerEvent(NetActionEvent.NetworkLatencyUpdateEvent, peer);
        }

        protected virtual void OnNetworkErrorEvent(System.Net.IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
        {
            EventMgr<NetActionEvent, System.Net.IPEndPoint>.TriggerEvent(NetActionEvent.NetworkErrorEvent, endPoint);
        }

        protected virtual void OnDeliveryEvent(NetPeer peer, object userData)
        {
            EventMgr<NetActionEvent, NetPeer>.TriggerEvent(NetActionEvent.DeliveryEvent, peer);
        }

        protected virtual void OnConnectionRequestEvent(ConnectionRequest request)
        {
            EventMgr<NetActionEvent, ConnectionRequest>.TriggerEvent(NetActionEvent.ConnectionRequestEvent, request);
        }
        #endregion

        public virtual void StartServer(int port)
        {
            Port = port;
            _networkState = true;
            m_Network.Start(port);
            ThreadPool.QueueUserWorkItem(PollEvent,null);
            Logger.Log(string.Format("Server [{0}] has launched at port:{1} Success.",GetType().ToString(),port));
        }

        bool _networkState = true;
        void PollEvent(object obj)
        {
            Logger.Log($"Start poll at ThreadId:{Thread.CurrentThread.ManagedThreadId}.");
            while(_networkState)
            {
                m_Network.PollEvents();
                CallCustomEvent();
                Thread.Sleep(15);
            }
        }
        protected virtual void CallCustomEvent()
        {
            EventMgr<NetActionEvent, object>.TriggerEvent(NetActionEvent.CallCustomEvent, null);
        }
        public virtual void ShutDown()
        {
            _networkState = false;
            m_NetListener.ClearConnectionRequestEvent();
            m_NetListener.ClearDeliveryEvent();
            m_NetListener.ClearNetworkErrorEvent();
            m_NetListener.ClearNetworkLatencyUpdateEvent();
            m_NetListener.ClearNetworkReceiveEvent();
            m_NetListener.ClearNetworkReceiveUnconnectedEvent();
            m_NetListener.ClearPeerConnectedEvent();
            m_NetListener.ClearPeerDisconnectedEvent();
            m_NetListener = null;
            m_Network.DisconnectAll();
            m_Network.Stop();
            m_Network = null;

            Logger.Log("ShutDown");
            Logger = null;
        }
        protected virtual void SetUp()
        {

        }

        public virtual void Dispose()
        {
            m_ApplicationKey = null;
            m_ConfigMaps = null;
            BaseModule.RemoveAllModules();
        }

        protected virtual void AddModule(BaseModule module)
        {
            BaseModule.AddModule(module);
            Logger.Log(string.Format("AddModule [{0}] Success.",module.GetType().ToString()));
        }
    }
}
