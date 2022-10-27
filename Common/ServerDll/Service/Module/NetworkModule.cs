using Net;
using Net.ServiceImpl;
using ServerDll.Service.Provider;
using Service.Core;
using Service.Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ServerDll.Service.Module
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

        #endregion
        protected IProvider netProvider;
        public NetworkModule (IProvider provider)
        {
            netProvider = provider;
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
    }
}
