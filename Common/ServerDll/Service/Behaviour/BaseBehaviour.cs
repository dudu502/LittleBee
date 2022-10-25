using Net;
using Net.ServiceImpl;
using Service.Core;
using Service.Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ServerDll.Service.Behaviour
{
    public abstract class BaseBehaviour: WebSocketBehavior
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            OnOpenEventImpl(new NetMessageEvt(ID, null, NetMessageEvt.State.OnOpen, RequestMessageId.SOCKET_MSG_ONOPEN));
            LogInfo($"{GetType().Name} OnOpen ID:{ID}");
        }
        protected override void OnMessage(MessageEventArgs e)
        {   
            base.OnMessage(e);
            PtMessagePackage ptMessagePackage = PtMessagePackage.Read(e.RawData);
            if (ptMessagePackage != null)
            {
                OnMessageEventImpl(new NetMessageEvt(ID, ptMessagePackage.Content, NetMessageEvt.State.OnMessage,(RequestMessageId)ptMessagePackage.MessageId));
                LogInfo($"{GetType().Name} OnMsg ID:{ID} MsgID:{(RequestMessageId)ptMessagePackage.MessageId}");
            }
        }
        protected virtual void OnOpenEventImpl(NetMessageEvt evt)
        {

        }
        protected virtual void OnMessageEventImpl(NetMessageEvt evt)
        {

        }
        protected virtual void OnErrorEventImpl(NetMessageEvt evt)
        {

        }
        protected virtual void OnCloseEventImpl(NetMessageEvt evt)
        {

        }
        public void LogInfo(string message)
        {
            BaseApplication.Logger?.Log(message);
        }
        public void LogWarning(string message)
        {
            BaseApplication.Logger?.LogWarning(message);
        }
        public void LogError(string message)
        {
            BaseApplication.Logger?.LogError(message);
        }
        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
            OnErrorEventImpl( new NetMessageEvt(ID, null, NetMessageEvt.State.OnError, RequestMessageId.SOCKET_MSG_ONERROR));
            LogError($"{GetType().Name} OnError ID:{ID} {e.Message}");
        }
        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            OnCloseEventImpl(new NetMessageEvt(ID,null, NetMessageEvt.State.OnClose, RequestMessageId.SOCKET_MSG_ONCLOSE));
            LogWarning($"{GetType().Name} OnClose ID:{ID} {e.Reason}");
        }
    }
}
