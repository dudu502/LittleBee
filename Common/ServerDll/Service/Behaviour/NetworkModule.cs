using Net;
using Net.ServiceImpl;
using Service.Core;
using Service.Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ServerDll.Service.Behaviour
{
    public class NetworkModule
    {
        #region static methods
        static Dictionary<Type,NetworkModule> networkModules = new Dictionary<Type, NetworkModule>();
        public static void AddModule(NetworkModule module)
        {
            networkModules[module.GetType()] = module;
        }
        public static T GetModule<T>() where T : NetworkModule
        {
            Type type = typeof(T);
            if (networkModules.ContainsKey(type))
                return (T)networkModules[type];
            return null;
        }
        public static void Tick()
        {
            foreach (NetworkModule module in networkModules.Values)
                module.PollEvents();
        }
        #endregion

        readonly protected ConcurrentQueue<NetMessageEvt> netEventQueue = new ConcurrentQueue<NetMessageEvt>();
        readonly protected WebSocketServer wss;
        public NetworkModule (WebSocketServer wssParam)
        {
            wss = wssParam;
        }
        public void EnqueueNetworkEvents(NetMessageEvt evt)
        {
            netEventQueue.Enqueue(evt);
        }

        public void PollEvents()
        {
            while(netEventQueue.TryDequeue(out var evt))
            {
                Evt.EventMgr<RequestMessageId, NetMessageEvt>.TriggerEvent(evt.MessageId, evt);
            }
        }
        protected WebSocketServiceHost FindSocketServiceHost<SERVICE>() where SERVICE : WebSocketBehavior
        {
            Type type = typeof (SERVICE);
            foreach(var host in wss.WebSocketServices.Hosts)
            {
                if (host.Type == type) return host;
            }
            return null;
        }
        protected async void SendToAsync<SERVICE>(PtMessagePackage ptMessage, string sessionId)where SERVICE : WebSocketBehavior
        {
            await SendToTask<SERVICE>(ptMessage, sessionId);
        }
        protected void SendToAsync<SERVICE>(ushort messageId, byte[] raw, string sessionId) where SERVICE : WebSocketBehavior
        {
            SendToAsync<SERVICE>(PtMessagePackage.Build(messageId, raw), sessionId);
        }

        protected Task SendToTask<SERVICE>(PtMessagePackage ptMessage, string sessionId)where SERVICE : WebSocketBehavior
        {
            return Task.Run(() => {
                WebSocketServiceHost host = FindSocketServiceHost<SERVICE>();
                if (host != null)
                    host.Sessions.SendTo(PtMessagePackage.Write(ptMessage), sessionId);
                else
                    LogError($"Host is null {typeof(SERVICE)}");
            });        
        }
        protected Task SendToTask<SERVICE>(ushort messageId, byte[] raw, string sessionId) where SERVICE : WebSocketBehavior
        {
            return SendToTask<SERVICE>(PtMessagePackage.Build(messageId, raw), sessionId);
        }

        protected Task BroadcastTask<SERVICE>(PtMessagePackage ptMessage) where SERVICE : WebSocketBehavior
        {
            return Task.Run(() => {
                WebSocketServiceHost host = FindSocketServiceHost<SERVICE>();
                if(host != null)
                    host.Sessions.Broadcast(PtMessagePackage.Write(ptMessage));
                else
                    LogError($"Host is null {typeof(SERVICE)}");
            });
        }
        protected Task BroadcastTask<SERVICE>(ushort messageId, byte[] raw) where SERVICE : WebSocketBehavior
        {
            return BroadcastTask<SERVICE>(PtMessagePackage.Build(messageId, raw));
        }

        protected async void BroadcastAsync<SERVICE>(PtMessagePackage ptMessage) where SERVICE : WebSocketBehavior
        {
            await BroadcastTask<SERVICE>(ptMessage);
        }
        protected void BroadcastAsync<SERVICE>(ushort messageId, byte[] raw) where SERVICE : WebSocketBehavior
        {
            BroadcastAsync<SERVICE>(PtMessagePackage.Build(messageId, raw));
        }
        public void LogInfo(string message)
        {
            BaseApplication.Logger?.Log(message);
        }
        public void LogWarning(string message)
        {
            BaseApplication.Logger?.LogWarning(message);
        }
        public void LogError(string message)
        {
            BaseApplication.Logger?.LogError(message);
        }
    }
}
