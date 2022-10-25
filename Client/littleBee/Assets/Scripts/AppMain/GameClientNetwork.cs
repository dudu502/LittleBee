using UnityEngine;
using Net;
using System.Net;
using System.Collections.Concurrent;
using Net.ServiceImpl;

using System;
using WebSocketSharp;
public class GameClientNetwork
{
    public enum State
    {
        Gate,
        Battle,
    }
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
 
    }
    #endregion
    public State GameState = State.Gate;
 
    private WebSocket m_websocket;
    private long m_HeartBeatTicks = 0;
    private DateTime m_HeartBeatDateTime;

    public void CloseClient()
    {
        if(m_websocket!=null)
        {
            m_websocket.CloseAsync();

        }
        m_websocket = null;
    }


    public void Start(string ws, State state)
    {
        GameState = state;
        m_websocket = new WebSocket(ws);
        m_websocket.OnOpen += OnWebsocketOpen;
        m_websocket.OnMessage += OnWebsocketMessage;
        m_websocket.OnClose += OnWebsocketClose;
        m_websocket.OnError += OnWebsocketError;
        m_websocket.ConnectAsync();
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
        Evt.EventMgr<NetworkEvtType, State>.TriggerEvent(NetworkEvtType.OnOpen, GameState);
    }

    public void Send(PtMessagePackage package)
    {
        if (m_websocket != null)
        {
            m_websocket.Send(PtMessagePackage.Write(package));
        }
    }

    public void SendAsync(PtMessagePackage package, Action<bool> complete = null)
    {
        if (m_websocket != null)
        {
            m_websocket.SendAsync(PtMessagePackage.Write(package), complete);
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
