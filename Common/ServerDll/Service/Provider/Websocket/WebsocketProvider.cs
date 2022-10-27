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

        public WebSocketServer GetSocket()
        {
            return socketServer;
        }

        public async void BroadcastAsync(byte[] raw)
        {
            await Task.Run(() => 
            {
                socketServer.WebSocketServices.Broadcast(raw);
            });
        }

        public void BroadcastAsync(PtMessagePackage package)
        {
            BroadcastAsync(PtMessagePackage.Write(package));
        }

        public async void SendToAsync(byte[] raw, string id) 
        {
            await Task.Run(() => {
                foreach (var host in socketServer.WebSocketServices.Hosts)
                {
                    if (host != null)
                    {
                        host.Sessions.SendTo(raw, id);
                    }
                }
            });
        }

        public void SendToAsync(PtMessagePackage package, string id)
        {
            SendToAsync( PtMessagePackage.Write(package), id);
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
