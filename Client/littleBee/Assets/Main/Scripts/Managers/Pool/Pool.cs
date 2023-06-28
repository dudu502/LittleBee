using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Synchronize.Game.Lockstep.Managers
{
	public class ObjectInstance
	{
		GameObject gameObject;
		PoolObject poolObjectScript;

		public ObjectInstance(GameObject objectInstance,Pool pool)
		{
			gameObject = objectInstance;
			gameObject.SetActive(false);
			if (gameObject.GetComponent<PoolObject>())
			{
				poolObjectScript = gameObject.GetComponent<PoolObject>();
				poolObjectScript.OwnerPool = pool;
			}
			else
			{
				Debug.LogError("Pool Object invalid.Please add PoolObject Component to GameObject..");
			}
		}
		public GameObject GetSource() { return gameObject; }
		public void Recycle()
		{
			poolObjectScript.OnObjectRecycle();
			poolObjectScript.transform.SetParent(poolObjectScript.OwnerPool.transform);
		}
		public void Reuse()
		{
			poolObjectScript.OnObjectReuse();
		}
	}
	public class Pool : MonoBehaviour
    {
		public string prefabFullName;
		public Queue<ObjectInstance> queue = new Queue<ObjectInstance>();

		public void Recycle(GameObject go)
		{
			ObjectInstance objectInstance = new ObjectInstance(go,this);
			objectInstance.Recycle();
			queue.Enqueue(objectInstance);
		}
		public GameObject Reuse()
		{
			if(queue.Count==0)
			{
				print("Reuse " + prefabFullName);
				GameObject go = Instantiate(Resources.Load<GameObject>(prefabFullName));
				queue.Enqueue(new ObjectInstance(go,this));
			}
			ObjectInstance instance = queue.Dequeue();
			instance.Reuse();
			return instance.GetSource();
		}
    }
}