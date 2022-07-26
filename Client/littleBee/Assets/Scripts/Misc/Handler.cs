using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Misc
{
    public class Handler
    {
        private class CallBack
        {
            public object data;
            public Action<object> caller;
            public Action complete;
            public CallBack(Action<object> c, object d)
            {
                caller = c;
                data = d;
            }
            public void Invoke()
            {
                caller?.Invoke(data);
                complete?.Invoke();
            }
        }
        private static ConcurrentQueue<CallBack> iNs;
        public static void Init()
        {
            iNs = new ConcurrentQueue<CallBack>();
        }
        public static void Run(Action<object> action, object data)
        {
            iNs.Enqueue(new CallBack(action, data) );
        }
        public static void Run(Action<object> action,object data,Action complete)
        {
            var caller = new CallBack(action, data);
            caller.complete = complete;
            iNs.Enqueue(caller);
        }
        public static void Update()
        {
            while(iNs.Count>0)
            {
                if(iNs.TryDequeue(out CallBack callBack))
                {
                    callBack?.Invoke();                 
                }
            }
        }

        public static System.Threading.Tasks.Task WaitRunCompleteTask(Action<object> action,object data)
        {
            bool isComplete = false;
            Run(action, data,()=> { isComplete = true; });
            return System.Threading.Tasks.Task.Run(async () =>
            {
                while (!isComplete)
                {
                    await System.Threading.Tasks.Task.Delay(10);
                }
            });
        }
    }
}
