using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Managers
{	
    public class PoolModule :MonoBehaviour, IModule
    {
		Dictionary<string, Pool> poolDictionary = new Dictionary<string, Pool>();
        public void Init()
        {

		}
		public void Clear()
		{
			var keys = poolDictionary.Keys.ToList<string>();
			foreach(string key in keys)
			{
				var pool = poolDictionary[key]; 
				poolDictionary.Remove(key);
				if (pool != null && pool.transform != null && pool.gameObject != null)
					Destroy(pool.gameObject);			
			}
		}
		public bool HasPool(string prefabFullName)
		{
			return poolDictionary.ContainsKey(prefabFullName);
		}
		public void CreatePoolIfNotExist(string prefabFullName)
		{
			if (!HasPool(prefabFullName))
				CreatePool(prefabFullName);
		}
		public void CreatePool(string prefabFullName)
		{	
			if (!HasPool(prefabFullName))
			{
				GameObject pool = new GameObject(prefabFullName);
				pool.transform.SetParent(transform);
				Pool poolScript = pool.AddComponent<Pool>();
				poolScript.prefabFullName = prefabFullName;
				poolDictionary[prefabFullName] = poolScript ;
			}
		}
		public void Recycle(string prefabFullName,GameObject go)
		{
			if (HasPool(prefabFullName))
			{
				poolDictionary[prefabFullName].Recycle(go);
			}
		}
		public GameObject Reuse(string prefabFullName)
		{
			if(HasPool(prefabFullName))
			{
				Pool pool = poolDictionary[prefabFullName];
				return pool.Reuse();
			}
			throw new Exception("Error!");
		}
        public void Tick()
        {
            
        }

		
	}

}
