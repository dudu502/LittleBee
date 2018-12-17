using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notify
{
    public class NotifyMgr
    {
        static NotifyMgr _Ins = new NotifyMgr();
        private NotifyMgr() { }
        public static NotifyMgr Instance
        {
            get
            {
                return _Ins;
            }
        }
        static Dictionary<Enum, Action<Notification>> Actions = new Dictionary<Enum, Action<Notification>>();

        public void AddListener(Enum type,Action<Notification> handler)
        {
            if (!Actions.ContainsKey(type))
                Actions[type] = handler;
            else 
                Actions[type] += handler;
        }
        public void RemoveListener(Enum type, Action<Notification> handler)
        {
            if (Actions.ContainsKey(type))
                Actions[type] -= handler;
        }

        public void Send(Enum type, Notification data)
        {
            if (Actions.ContainsKey(type))
                Actions[type].Invoke(data);
        }
    }
}
