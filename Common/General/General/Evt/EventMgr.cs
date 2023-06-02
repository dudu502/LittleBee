using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Synchronize.Game.Lockstep.Evt
{
    public sealed class EventMgr<T,P>
    {
        private EventMgr() { }
        readonly static Dictionary<T, Action<P>> delegates = new Dictionary<T, Action<P>>();      
        public static void AddListener(T type, Action<P> action)
        {
            if (delegates.ContainsKey(type))
                delegates[type] += action;
            else
                delegates[type] = action;
        }
        public static void RemoveListener(T type, Action<P> action)
        {
            if (delegates.ContainsKey(type))
                delegates[type] -= action;
        }
        public static void TriggerEvent(T type, P data)
        {
            if (delegates.ContainsKey(type))
                delegates[type]?.Invoke(data);
        }     
    }
}
