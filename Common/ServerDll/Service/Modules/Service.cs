using System;
using System.Collections.Generic;
using System.Text;

namespace ServerDll.Service.Modules
{
    public class Service
    {
        static Dictionary<Type, BaseModule> _Modules = new Dictionary<Type, BaseModule>();
        public static bool AddModule(BaseModule module)
        {
            Type type = module.GetType();
            if (Contain(type))
                return false;
            else
            {
                _Modules[type] = module;
                return true;
            }
        }

        public static void RemoveAllModule()
        {
            var values = new List<BaseModule>(_Modules.Values);
            foreach(var value in values)
            {
                RemoveModule(value);
            }
        }
        public static bool RemoveModule(BaseModule module)
        {
            module.Dispose();
            Type type = module.GetType();
            if (Contain(type))
            {
                return _Modules.Remove(type);
            }
            return false;
        }
        public static bool Contain(Type type)
        {
            return _Modules.ContainsKey(type);
        }

        public static T GetModule<T>()where T:BaseModule
        {
            Type type = typeof(T);
            if (Contain(type))
                return (T)_Modules[type];
            return null;
        }
    }
}
