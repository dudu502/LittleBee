using GateServer.Core.Data;
using ServerDll.Service.Behaviour;
using Service.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace GateServer.Core.Behaviour
{
    public class RoomProcessBehaviour:BaseBehaviour
    {
        public RoomProcessBehaviour()
        {
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
        }
        protected override void OnCloseEventImpl(NetMessageEvt evt)
        {
            base.OnCloseEventImpl(evt);
            NetworkModule.GetModule<RoomProcessNetworkModule>().EnqueueNetworkEvents(evt);
        }
        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }
        protected override void OnErrorEventImpl(NetMessageEvt evt)
        {
            base.OnErrorEventImpl(evt);
            NetworkModule.GetModule<RoomProcessNetworkModule>().EnqueueNetworkEvents(evt);
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
        }
        protected override void OnMessageEventImpl(NetMessageEvt evt)
        {
            base.OnMessageEventImpl(evt);
            NetworkModule.GetModule<RoomProcessNetworkModule>().EnqueueNetworkEvents(evt);
        }
        protected override void OnOpen()
        {
            base.OnOpen();
        }
        protected override void OnOpenEventImpl(NetMessageEvt evt)
        {
            base.OnOpenEventImpl(evt);
            NetworkModule.GetModule<RoomProcessNetworkModule>().EnqueueNetworkEvents(evt);
        }
    }
}
