using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool.Common
{
    public class PoolData
    {
        public GameObject Go { private set; get; }
        public float ExpireTime;

        public PoolData(GameObject go)
        {
            Go = go;
            ExpireTime = Time.time;
        }

        public bool IsExpire()
        {
            return Time.time - ExpireTime > PoolMgr.EXPIRE_TIME;
        }

        public void SetParent(Transform transformRoot)
        {
            Go.transform.parent = transformRoot;
        }

        public void Destory()
        {
            GameObject.Destroy(Go);
        }
    }

    public class Pool
    {
        public Func<GameObject> CreateGameObjectAction;
        public Transform TransformRoot { private set; get; }
        public List<PoolData> PoolDatas = new List<PoolData>();
        public string Path;
        public Pool(Transform root,string path)
        {
            TransformRoot = root;
            Path = path;
        }

        public void PushObject(GameObject gameObject)
        {
            PoolData poolData = new PoolData(gameObject);
            poolData.SetParent(TransformRoot);
            PoolDatas.Add(poolData);
        }

        public GameObject PopObject(Transform transformRoot)
        {
            if(PoolDatas.Count>0)
            {
                PoolData poolData = PoolDatas[PoolDatas.Count-1];
                PoolDatas.RemoveAt(PoolDatas.Count - 1);
                poolData.SetParent(transformRoot);
                return poolData.Go;
            }
            return null;
        }

        public void ExpireObject()
        {
            for(int i = PoolDatas.Count - 1; i > -1; --i)
            {
                PoolData poolData = PoolDatas[i];
                if (poolData.IsExpire())
                {
                    poolData.Destory();
                    PoolDatas.RemoveAt(i);
                }                  
            }
        }
    }
}