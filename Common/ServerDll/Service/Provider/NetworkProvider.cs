using Net;
using Service.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerDll.Service.Provider
{
    public interface IProvider
    {
        void Start(ushort port);
        void SetUp();
        void Stop();
        void OnConnected(string id);
        void OnDisconnected(string id);
        void OnMessage(string id);
        void OnError(string id);
        void SendToAsync<T>(byte[] raw,string id);
        void BroadcastAsync<T>(byte[] raw);
        void SendToAsync<T>(PtMessagePackage package, string id);
        void BroadcastAsync<T>(PtMessagePackage package);
        void EnqueueNetworkEvents(NetMessageEvt evt);
        void PollEvents();
    }
    public class NetworkProvider
    {
        public enum ProviderType
        {
            WSS,
            KCP,
        }
        public ProviderType Type { private set; get; }
        public IProvider Provider { private set; get; }
        public NetworkProvider(ProviderType type)
        {
            Type = type;
        }

    }
}
