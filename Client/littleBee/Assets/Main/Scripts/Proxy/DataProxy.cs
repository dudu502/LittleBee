using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    public class DataProxy
    {
        public DataProxy()
        {
            OnInit();
        }
        protected virtual void OnInit()
        {

        }
        private static Dictionary<Type, DataProxy> s_Proxys = new Dictionary<Type, DataProxy>();
        public static void Init<T>(DataProxy proxy)
        {
            Type type = typeof(T);
            if (!s_Proxys.ContainsKey(type))
                s_Proxys.Add(type, proxy);
        }
        public static T Get<T>() where T : DataProxy
        {
            Type type = typeof(T);
            if (s_Proxys.ContainsKey(type))
            {
                return (T)s_Proxys[type];
            }
            return null;
        }
    }
}
