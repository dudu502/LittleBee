using Net;
using Net.ServiceImpl;
using ServerDll.Service.Provider;
using Service.Core;
using Service.Event;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ServerDll.Service.Provider.Websocket
{
    public abstract class BaseBehaviour: WebSocketBehavior
    {
        protected IProvider provider;
        public BaseBehaviour(IProvider providerParam)
        {
            provider = providerParam;
        }
        protected override void OnOpen()
        {
            base.OnOpen();
            provider.EnqueueNetworkEvents(new NetMessageEvt(ID, null, NetMessageEvt.State.OnOpen, RequestMessageId.SOCKET_MSG_ONOPEN));
            LogInfo($"{GetType().Name} OnOpen ID:{ID}");
        }
        protected override void OnMessage(MessageEventArgs e)
        {   
            base.OnMessage(e);
            PtMessagePackage ptMessagePackage = PtMessagePackage.Read(e.RawData);
            if (ptMessagePackage != null)
            {
                provider.EnqueueNetworkEvents(new NetMessageEvt(ID, ptMessagePackage.Content, NetMessageEvt.State.OnMessage, (RequestMessageId)ptMessagePackage.MessageId));
                LogInfo($"{GetType().Name} OnMsg ID:{ID} MsgID:{(RequestMessageId)ptMessagePackage.MessageId}");
            }
        }

        public void LogInfo(string message)
        {
            Logger.LogInfo(message);
        }
        public void LogWarning(string message)
        {
            Logger.LogWarning(message);
        }
        public void LogError(string message)
        {
            Logger.LogError(message);
        }
        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
            provider.EnqueueNetworkEvents(new NetMessageEvt(ID, null, NetMessageEvt.State.OnError, RequestMessageId.SOCKET_MSG_ONERROR));
            LogError($"{GetType().Name} OnError ID:{ID} {e.Message}");
        }
        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            provider.EnqueueNetworkEvents(new NetMessageEvt(ID, null, NetMessageEvt.State.OnClose, RequestMessageId.SOCKET_MSG_ONCLOSE));
            LogWarning($"{GetType().Name} OnClose ID:{ID} {e.Reason}");
        }
    }
}
