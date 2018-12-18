using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Net;
using System.Net;
using System.Collections.Generic;
using NetServiceImpl;
using System;
using NetServiceImpl.Client;
using System.Collections.Concurrent;

public class GameClientNetwork : MonoBehaviour
{
    public static GameClientNetwork Instance {
        get;private set;
    }
    Notify.Notifier notifier;
    private Client m_Client;
    private ConcurrentQueue<PtMessagePackage> m_QueueMsg;

    private void Awake()
    {
        Instance = this;
        notifier = new Notify.Notifier();
        m_QueueMsg = new ConcurrentQueue<PtMessagePackage>();
        Service.Get<LoginService>();
    }
    // Use this for initialization
    void Start()
    {    
        m_Client = new Client();
        m_Client.DataReceived += (sender, msg) => 
        {
            m_QueueMsg.Enqueue(PtMessagePackage.Read(msg.Data));          
        };
    }
    public void Connect()
    {
        m_Client.Connect("192.168.18.56", 10000);
    }
    public void SendRequest(PtMessagePackage package)
    {
        m_Client.Write(package);
    }
    // Update is called once per frame
    void Update()
    {
        TickDispatchMessages();
    }

    public void TickDispatchMessages()
    {
        while(m_QueueMsg.Count>0)
        {
            PtMessagePackage package = null;

            if (m_QueueMsg.TryDequeue(out package))
                notifier.Send((S2CMessageId)package.MessageId, package.Content);
                //Notify.NotifyMgr.Instance.Send((S2CMessageId)package.MessageId, new Notify.Notification() { Params = new object[] { package.Content } });
        }
    }
}
