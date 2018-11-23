using System;
using System.Collections.Concurrent;
using UnityEngine;

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
    
    public object GetObject()
    {
        object result;
        if(!m_Queue.TryDequeue(out result))
            result = m_Constructor();
        m_Init(result);
        return result;
    }
    public void ReturnObjectToPool(object obj)
    {
        m_Reset(obj);
        m_Queue.Enqueue(obj);
    }    

    public GameObject GetGameObject()
    {
        object result;
        if (!m_Queue.TryDequeue(out result))
            result = m_Constructor();
        m_Init(result);
        GameObject obj = result as GameObject;
        obj.transform.SetParent(null);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnGameObjectToPool(GameObject obj)
    {
        m_Reset(obj);
        m_Queue.Enqueue(obj);
        obj.transform.SetParent(ObjectPoolContainer.Instance.m_TransGo);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.SetActive(false);
    }
}

