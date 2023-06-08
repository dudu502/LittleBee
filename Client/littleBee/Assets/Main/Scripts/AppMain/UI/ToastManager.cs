using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Notification
{
    public class ToastManager : MonoBehaviour
    {
        [SerializeField] private List<ToastItem> toastItems;

        private Queue<string> queueMsgs = new Queue<string>();

        public static ToastManager Instance { private set; get; }
        private void Awake()
        {
            Instance = this;
        }
        public void ShowToast(string msg)
        {
            lock (queueMsgs)
            {
                queueMsgs.Enqueue(msg);
            }
        }

        void Update()
        {
            lock (queueMsgs)
            {
                while (queueMsgs.Count > 0)
                {
                    ToastItem toastItem = toastItems.Find(t => t.IsIdle);
                    if (toastItem == null)
                    {
                        return;
                    }
                    toastItem.SetText(queueMsgs.Dequeue(), OnDisplayItemFinish);
                }
            }
        }

        void OnDisplayItemFinish(ToastItem toastItem)
        {
            if (toastItem != null)
            {
                toastItem.transform.SetAsLastSibling();
                toastItems.Remove(toastItem);
                toastItems.Add(toastItem);
            }
        }
    }
}