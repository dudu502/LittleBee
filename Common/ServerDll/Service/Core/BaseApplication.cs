using System;
using System.Collections.Generic;
using System.Threading;
using ServerDll.Service;
using ServerDll.Service.Provider;

namespace Service.Core
{
    public class BaseApplication
    {
        protected NetworkProviderWrap providerWrap;
        protected int m_Port;
        private Dictionary<string, object> m_ConfigMaps;
        protected bool isRunning = false;
        protected NetworkType m_NetworkType = NetworkType.WSS;
        public BaseApplication(ushort port)
        {
            m_ConfigMaps = new Dictionary<string, object>();
            m_Port = port;
            Logger.LogInfo(string.Format("Application [{0}] Initialize ApplicationKey:{1}.", GetType().ToString(), m_Port));

            Logger.LogInfo("Application has Setup.");

            ServerDll.Service.Logger.LogInfoAction = Console.Write;
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
            providerWrap = new NetworkProviderWrap(m_NetworkType);
            providerWrap.Start((ushort)m_Port);
            SetUp();
            Logger.LogInfo(string.Format("Server [{0}] has launched.", m_Port));
            isRunning = true;
            ThreadPool.QueueUserWorkItem(PollEvents, null);
        }

        private void PollEvents(object state)
        {
            while (isRunning)
            {
                providerWrap.Tick();
                Thread.Sleep(15);
            }
        }

        public virtual void ShutDown()
        {
            isRunning = false;

            if (providerWrap != null)
                providerWrap.Stop();

            Logger.LogInfo("ShutDown");
        }
        protected virtual void SetUp()
        {

        }
    }
}
