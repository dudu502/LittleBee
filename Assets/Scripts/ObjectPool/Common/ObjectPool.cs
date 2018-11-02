using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ObjectPool
{
    Func<object> m_Constructor;
    Action<object> m_Init;
    Action<object> m_Reset;
    ConcurrentQueue<object> m_Queue;
    public ObjectPool(Func<object> constructor,Action<object> init,Action<object> reset)
    {
        if (constructor == null) throw new ArgumentNullException();
        if (init == null) throw new ArgumentNullException();
        if (reset == null) throw new ArgumentNullException();
        m_Constructor = constructor;
        m_Init = init;
        m_Reset = reset;
        m_Queue = new ConcurrentQueue<object>();
    }

    public int GetPoolSize() { return m_Queue.Count; }
    object CreateObject()
    {
        object result = m_Constructor();
        return result;
    }
    public object GetObject()
    {
        object result;
        if(!m_Queue.TryDequeue(out result))
            result = CreateObject();
        m_Init(result);
        return result;
    }
    public void ReturnObjectToPool(object obj)
    {
        m_Reset(obj);
        m_Queue.Enqueue(obj);
    }    
}

