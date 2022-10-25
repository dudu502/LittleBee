
using Net.ServiceImpl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Event
{
    public class NetMessageEvt
    {
        public enum State:byte
        {
            Default,
            OnOpen,
            OnMessage,
            OnError,
            OnClose,
        }
        public RequestMessageId MessageId;
        public State MessageState = State.Default;
        public string SessionId { private set; get; }
        public byte[] Content { private set; get; }
        public NetMessageEvt(string sessionId,byte[] content,State state , RequestMessageId msgId)
        {
            SessionId = sessionId;
            Content = content;
            MessageState = state;
            MessageId = msgId;
        }
    }
}
