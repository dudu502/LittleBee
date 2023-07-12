using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;
using Synchronize.Game.Lockstep.Service.Core;

namespace Synchronize.Game.Lockstep.Service.Modules
{
    public class BaseModule
    {
        protected BaseApplication ApplicationInstance;
        public BaseModule(BaseApplication app):base()
        {
            ApplicationInstance = app;
        }
        public T GetApplication<T>()where T:BaseApplication
        {
            return (T)ApplicationInstance;
        }
        public NetManager GetNetManager() {
            return ApplicationInstance.GetNetManager();     
        }

        public static Dictionary<Type,BaseModule> _modules = new Dictionary<Type, BaseModule>();
        public static T GetModule<T>() where T:BaseModule
        {
            return (T)_modules[typeof(T)];
        }
        public static void AddModule(BaseModule module)
        {
            _modules[module.GetType()] = module;
        }
        public static void RemoveModule<T>()
        {
            if (_modules.TryGetValue(typeof(T),out var module))
            {
                module.Dispose();
                _modules.Remove(typeof(T));
            }
        }
        public static void RemoveAllModules()
        {
            foreach(BaseModule module in _modules.Values)
                module.Dispose();
            _modules.Clear();
        }

        public void Log(string message)
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
        public virtual void Dispose()
        {

        }
    }
}
