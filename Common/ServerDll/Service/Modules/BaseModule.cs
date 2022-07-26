using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;
using Service.Core;

namespace ServerDll.Service.Modules
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
