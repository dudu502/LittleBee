using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastRoot : MonoBehaviour
{
    [SerializeField] private List<ToastItem> toastItems;


    private Queue<string> queueMsgs = new Queue<string>();

    public static ToastRoot Instance { private set; get; }
    private void Awake()
    {
        Instance = this;
    }
    public void ShowToast(string msg)
    {
        lock(queueMsgs)
        {
            queueMsgs.Enqueue(msg);
        }
    }
    // Update is called once per frame
    void Update()
    {
        lock(queueMsgs)
        {
            while (queueMsgs.Count > 0)
            {
                ToastItem toastItem = toastItems.Find(t => t.IsIdle);
                if(toastItem==null)
                {
                    return;
                }
                toastItem.SetText(queueMsgs.Dequeue(), OnDisplayItemFinish);
            }
        }
    }

    void OnDisplayItemFinish(ToastItem toastItem)
    {
        if(toastItem!=null)
        {
            toastItem.transform.SetAsLastSibling();
            toastItems.Remove(toastItem);
            toastItems.Add(toastItem);
        }
    }
}
