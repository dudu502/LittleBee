using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServiceImpl
{
    public class Service : Notify.Notifier
    {
        public Service():base()
        {
            Init();
        }
        public virtual void Reset()
        {

        }
        protected virtual void Init()
        {
            
        }

        private static Dictionary<Type, Service> s_DictServices = new Dictionary<Type, Service>();
      
        public static T Get<T>()where T:Service
        {
            Type t = typeof(T);
            if(!s_DictServices.ContainsKey(t))
            {
                s_DictServices[t] = Activator.CreateInstance<T>();
            }
            return (T)s_DictServices[t];
        }
    }
}
