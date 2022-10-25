using WebSocketSharp;

using System.Collections.Generic;
using System.Threading;
using System;
using WebSocketSharp.Server;
using ServerDll.Service.Behaviour;

namespace Service.Core
{
    public class BaseApplication
    {
        protected WebSocketServer m_WebsocketServer;
        protected int m_WebServerPort;
        public static ILogger Logger;
        private Dictionary<string, object> m_ConfigMaps;
        protected bool isRunning = false;
        public BaseApplication(int wsPort)
        {
            m_ConfigMaps = new Dictionary<string, object>();
            m_WebServerPort = wsPort;
            Logger.Log(string.Format("Application [{0}] Initialize ApplicationKey:{1}.", GetType().ToString(), m_WebServerPort));

            Logger.Log("Application has Setup.");
        }

        public WebSocketServer GetSocket()
        {
            return m_WebsocketServer;
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

        public virtual void StartServer()
        {
            m_WebsocketServer = new WebSocketServer(m_WebServerPort);
            SetUp();

            m_WebsocketServer.Start();
            Logger.Log(string.Format("Server [{0}] has launched.", m_WebServerPort));
            isRunning = true;
            ThreadPool.QueueUserWorkItem(PollEvents, null);
        }

        private void PollEvents(object state)
        {
            while (isRunning)
            {
                NetworkModule.Tick();
                Thread.Sleep(15);
            }
        }

        public virtual void ShutDown()
        {
            isRunning = false;

            if (m_WebsocketServer!=null)
                m_WebsocketServer.Stop();

            Logger.Log("ShutDown");
            Logger = null;
        }
        protected virtual void SetUp()
        {

        }
    }
}
