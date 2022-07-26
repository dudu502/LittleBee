using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServiceImpl
{
    public class ClientService
    {
        protected ILogger logger;
        public ClientService():base()
        {
            logger = new Logger.UnityEnvLogger("Client");
            Init();
        }
        public virtual void Reset()
        {

        }
        protected virtual void Init()
        {
            
        }
        protected void TriggerMainThreadEvent<T,P>(T type,P paramObj)
        {
            Misc.Handler.Run((item) => { Evt.EventMgr<T, P>.TriggerEvent(type, paramObj); }, null);
        }
        private static Dictionary<Type, ClientService> s_DictServices = new Dictionary<Type, ClientService>();
      
        
        public static T Get<T>() where T : ClientService
        {
            Type t = typeof(T);
            if (s_DictServices.ContainsKey(t))
            {
                return (T)s_DictServices[t];
            }
            throw new NullReferenceException();
        }
        public static void Init(ClientService service)
        {
            Type t = service.GetType();
            if (!s_DictServices.ContainsKey(t))
            {
                s_DictServices[t] = service;
            }
        }
         
    }
}
