using System;
using System.Collections.Generic;

namespace Synchronize.Game.Lockstep.Managers
{
    public interface IModule
    {
        void Init();
    }
    public class ModuleManager
    {
        private static Dictionary<Type,IModule> _Modules = new Dictionary<Type,IModule>();
        public static void Add(IModule module)
        {
            if (!_Modules.ContainsValue(module))
            {
                module.Init();
                _Modules[module.GetType()] = (module);
            }
        }
        public static bool Remove(IModule module)
        {
            return _Modules.Remove(module.GetType());
        }
        public static M GetModule<M>() where M : IModule
        {
            Type moduleType = typeof(M);
            if (_Modules.ContainsKey(moduleType))
                return (M)_Modules[moduleType];
            return default;
        }
    }
}
