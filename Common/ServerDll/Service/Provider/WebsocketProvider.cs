using Net;
using Net.ServiceImpl;
using Service.Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ServerDll.Service.Provider
{
    public class WebsocketProvider : IProvider
    {
        private WebSocketServer socketServer;
        private readonly ConcurrentQueue<NetMessageEvt> netMessageEvts = new ConcurrentQueue<NetMessageEvt>();
        public WebsocketProvider()
        {
            
        }
        public void BroadcastAsync<T>(byte[] raw)
        {
            throw new NotImplementedException();
        }

        public void BroadcastAsync<T>(PtMessagePackage package)
        {
            throw new NotImplementedException();
        }

        public void OnConnected(string id)
        {
            throw new NotImplementedException();
        }

        public void OnDisconnected(string id)
        {
            throw new NotImplementedException();
        }

        public void OnError(string id)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(string id)
        {
            throw new NotImplementedException();
        }
        WebSocketServiceHost FindSocketServiceHost<T>() 
        {
            Type type = typeof(T);
            foreach (var host in socketServer.WebSocketServices.Hosts)
            {
                if (host.Type == type) return host;
            }
            return null;
        }
        public async void SendToAsync<T>(byte[] raw, string id) 
        {
            await Task.Run(() => {
                WebSocketServiceHost host = FindSocketServiceHost<T>();
                if (host != null)
                    host.Sessions.SendTo(raw,id);
            });
        }

        public void SendToAsync<T>(PtMessagePackage package, string id)
        {
            SendToAsync<T>(PtMessagePackage.Write(package), id);
        }

        public void SetUp()
        {
            
        }

        public void Start(ushort port)
        {
            socketServer = new WebSocketServer(port);
            socketServer.Start(); 
        }

        public void Stop()
        {
            socketServer.Stop();
        }

        public void EnqueueNetworkEvents(NetMessageEvt evt)
        {
            netMessageEvts.Enqueue(evt);
        }

        public void PollEvents()
        {
            while (netMessageEvts.TryDequeue(out var evt))
                Evt.EventMgr<RequestMessageId, NetMessageEvt>.TriggerEvent(evt.MessageId, evt);
        }
    }
}
