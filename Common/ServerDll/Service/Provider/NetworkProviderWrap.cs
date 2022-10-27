using Net;
using ServerDll.Service.Provider.KCP;
using Service.Event;
using System;

namespace ServerDll.Service.Provider
{
    public interface IProvider
    {
        void Start(ushort port);
        void Stop();
        void SendToAsync(byte[] raw,string id);
        void BroadcastAsync(byte[] raw);
        void SendToAsync(PtMessagePackage package, string id);
        void BroadcastAsync(PtMessagePackage package);
        void EnqueueNetworkEvents(NetMessageEvt evt);
        void PollEvents();
    }
    public class NetworkProviderWrap
    {
        public NetworkType Type { private set; get; }
        public IProvider Provider { private set; get; }
        public NetworkProviderWrap(NetworkType type)
        {
            Type = type;
            if (type == NetworkType.WSS)
                Provider = new WebsocketProvider();
            else
                Provider = new KcpProvider();
        }
        public void Start(ushort port)
        {
            Provider.Start(port);
        }
        public void Stop()
        {
            Provider.Stop();
        }

        public void Tick()
        {
            Provider.PollEvents();
        }
    }
}
