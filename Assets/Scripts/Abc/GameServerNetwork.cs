using Net;
using NetServiceImpl;
using NetServiceImpl.Server;
using NetServiceImpl.Server.Data;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


public class GameServerNetwork
{
    static GameServerNetwork _Ins;
    public static GameServerNetwork Instance
    {
        get {
            if (_Ins == null) _Ins = new GameServerNetwork();
            return _Ins;
        }
    }
    Notify.Notifier notifier;
    private ConcurrentQueue<Message> m_QueueMsg;
    private GameServerNetwork() {
        _Ins = this;
        notifier = new Notify.Notifier();
        Service.Get<LoginService>();
        m_QueueMsg = new ConcurrentQueue<Message>();
    }
    Server m_Server;
    public void Start()
    {
        m_Server = new Server();
        //m_Server.Start(IPAddress.Parse("192.168.1.112"), 10000);
        m_Server.Start(IPAddress.Parse("127.0.0.1"), 10000);
        m_Server.ClientConnected += (send, msg) => 
        {
            var player = GameServerData.AddNewPlayer();
            PtMessagePackage package = PtMessagePackage.Build((int)S2CMessageId.ResponseClientConnected, false, player.Id);
            byte[] byts = PtMessagePackage.Write(package);
            NetworkStreamUtil.Write(msg.GetStream(), byts); 
        };

        m_Server.ClientDisconnected += (send, msg) => 
        {
            UnityEngine.Debug.Log("ClientDisconnected"+msg.ToString());
        };

        m_Server.DataReceived += (send, msg) => 
        {
            m_QueueMsg.Enqueue(msg);
            TickDispatchMessages();
        };
    }

    void Update()
    {
        TickDispatchMessages();
    }
    public void TickDispatchMessages()
    {
        while (m_QueueMsg.Count > 0)
        {
            Message msg = null;           
            if(m_QueueMsg.TryDequeue(out msg))
            {
                PtMessagePackage package = PtMessagePackage.Read(msg.Data);
                notifier.Send((C2SMessageId)package.MessageId, package.Content, msg);
                //Notify.NotifyMgr.Instance.Send((C2SMessageId)package.MessageId,new Notify.Notification() { Params = new object[] { package.Content,msg} });
            }           
        }
    }

    public void Broadcast(PtMessagePackage package)
    {
        m_Server.Broadcast(package);
    }

}

