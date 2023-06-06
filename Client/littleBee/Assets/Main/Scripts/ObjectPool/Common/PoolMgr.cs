
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ObjectPool.Common
{
    public class PoolMgr
    {
        public const int EXPIRE_TIME = 60;
        public static Dictionary<string, Pool> DictPools = new Dictionary<string, Pool>();
        public static void Init()
        {
            
        }
        public static Pool CreatePool(string path,Transform transformRoot)
        {
            if (!DictPools.ContainsKey(path))
                DictPools.Add(path, new Pool(transformRoot,path));
            return DictPools[path];
        }
        public static void RemovePool(string path)
        {
            if (DictPools.ContainsKey(path))
                DictPools.Remove(path);
        }
        public static void PushObject(string path,GameObject gameObject)
        {
            if (!DictPools.ContainsKey(path))
                throw new Exception("Create a pool first!");
            if(DictPools.TryGetValue(path,out Pool pool))
            {
                pool.PushObject(gameObject);
            }
        }
        public static GameObject PopObject(string path,Transform transformRoot)
        {
            if(DictPools.ContainsKey(path))
            {
                var obj= DictPools[path].PopObject(transformRoot);
                if (obj != null)
                    return obj;
            }
            GameObject createGo = null;
            if (DictPools[path].CreateGameObjectAction != null)
            {
                createGo = DictPools[path].CreateGameObjectAction?.Invoke();
                createGo.transform.parent = transformRoot;
            }            
            return createGo;
        }

        public static void ExpireObject()
        {
            foreach(Pool pool in DictPools.Values)
            {
                pool.ExpireObject();
            }
        }
        public static void Update()
        {
            ExpireObject();
        }
    }
}
