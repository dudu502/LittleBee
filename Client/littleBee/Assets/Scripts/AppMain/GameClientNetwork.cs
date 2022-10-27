using UnityEngine;
using Net;
using System.Net;
using System.Collections.Concurrent;
using Net.ServiceImpl;

using System;
using WebSocketSharp;
using ServerDll.Service.Requester;
using System.Threading;

public class GameClientNetwork
{
    public enum NetworkEvtType
    {
        OnOpen,
        OnClose,
        OnError,
    }
    #region single instance
    static GameClientNetwork _Inst = null;
    public static GameClientNetwork Instance {
        get
        {
            if (_Inst == null)
                _Inst = new GameClientNetwork();
            return _Inst; 
        }
    }
    private ConcurrentQueue<PtMessagePackage> _messageQueue = new ConcurrentQueue<PtMessagePackage>();
    private GameClientNetwork()
    {
        ServerDll.Service.Logger.LogInfoAction = Debug.Log;
        ServerDll.Service.Logger.LogErrorAction = Debug.LogError;
        ServerDll.Service.Logger.LogWarningAction = Debug.LogWarning;

    }
    #endregion

    private NetworkRequesterWrap requesterWrap;
   // private WebSocket m_websocket;
    private long m_HeartBeatTicks = 0;
    private DateTime m_HeartBeatDateTime;

    public void CloseClient()
    {
        if (requesterWrap != null)
            requesterWrap.Disconnect();
        requesterWrap = null;
    }


    public void Start(string ip,ushort port,string param)
    {
        requesterWrap = new NetworkRequesterWrap(ServerDll.Service.NetworkType.WSS);
        requesterWrap.Requester.OnConnected += OnConnected;
        requesterWrap.Requester.OnMessage += OnMessage;
        requesterWrap.Requester.OnDisconnected += OnDisconnected;
        requesterWrap.Requester.OnError += OnError;
        requesterWrap.Connect(ip, port, param);
        ThreadPool.QueueUserWorkItem(PollEvents, null); 
    }

    void PollEvents(object obj)
    {
        while (requesterWrap != null)
        {
            requesterWrap.Tick();
            Thread.Sleep(15);
        }
    }
    void OnConnected()
    {

    }
    void OnMessage(byte[] raw)
    {
        PtMessagePackage package = PtMessagePackage.Read(raw);
        if (package != null)
        {
            Evt.EventMgr<ResponseMessageId, PtMessagePackage>.TriggerEvent((ResponseMessageId)package.MessageId, package);
        }
    }
    void OnDisconnected()
    {
        Evt.EventMgr<NetworkEvtType, string>.TriggerEvent(NetworkEvtType.OnClose, "Network disconnected");
    }
    void OnError()
    {
        Evt.EventMgr<NetworkEvtType, string>.TriggerEvent(NetworkEvtType.OnError, "Network Error");
    }
    private void OnWebsocketError(object sender, ErrorEventArgs e)
    {
        Debug.LogWarning("OnWebsocketError"+e.Message);
        Evt.EventMgr<NetworkEvtType, string>.TriggerEvent(NetworkEvtType.OnError, e.Message);
    }

    private void OnWebsocketClose(object sender, CloseEventArgs e)
    {
        Debug.LogWarning("OnWebsocketClose" + e.Reason);
        Evt.EventMgr<NetworkEvtType, string>.TriggerEvent(NetworkEvtType.OnClose, e.Reason);
    }

    private void OnWebsocketMessage(object sender, MessageEventArgs e)
    {
        PtMessagePackage package = PtMessagePackage.Read(e.RawData);
        if (package != null)
        {
            Evt.EventMgr<ResponseMessageId, PtMessagePackage>.TriggerEvent((ResponseMessageId)package.MessageId, package);
        }
    }

    private void OnWebsocketOpen(object sender, EventArgs e)
    {
        Debug.LogWarning("OnWebsocketOpen" + e.ToString());
        
    }

    public void Send(PtMessagePackage package)
    {
        if (requesterWrap != null)
        {
            requesterWrap.Send(PtMessagePackage.Write(package));
        }
    }

    public void SendAsync(PtMessagePackage package, Action<bool> complete = null)
    {
        if (requesterWrap != null)
        {
            requesterWrap.Send(PtMessagePackage.Write(package));
        }
    }
    
   

    /// <summary>
    /// 心跳机制
    /// 每10秒进行一次
    /// </summary>
    void TickHeartBeating()
    {
        //心跳10 secs
        if (m_HeartBeatTicks == 0)
        {
            m_HeartBeatDateTime = DateTime.Now;
        }
        else if (m_HeartBeatTicks > 100000000)//10s = 10*1000*10000
        {
            m_HeartBeatTicks -= 100000000;
            SendAsync(PtMessagePackage.Build((ushort)RequestMessageId.RS_HeartBeat, null),null);
        }
        m_HeartBeatTicks += (DateTime.Now - m_HeartBeatDateTime).Ticks;
        m_HeartBeatDateTime = DateTime.Now;
    }
}
