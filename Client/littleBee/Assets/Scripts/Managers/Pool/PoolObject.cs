using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Synchronize.Game.Lockstep.Managers
{
    public class PoolObject : MonoBehaviour
    {
        [HideInInspector] public Pool OwnerPool;

        public UnityEvent RecycleEvent = new UnityEvent();
        public UnityEvent ReuseEvent = new UnityEvent();
        void Start()
        {

        }
        public string GetFullName()
        {
            if (OwnerPool != null)
                return OwnerPool.prefabFullName;
            return string.Empty;
        }
        public virtual void OnObjectReuse()
        {
            gameObject.SetActive(true);

        }
        public virtual void OnObjectRecycle()
        {
            gameObject.SetActive(false);
            if (RecycleEvent != null)
                RecycleEvent.Invoke();
        }
        protected void OnDestroy()
        {
            gameObject.SetActive(false);
            if (ReuseEvent != null)
                ReuseEvent.Invoke();
        }
    }
}