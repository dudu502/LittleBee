using Evt;
using GateServer.Core.Data;
using Net;
using Net.Pt;
using Net.ServiceImpl;
using ServerDll.Service.Behaviour;
using Service.Core;
using Service.Event;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WebSocketSharp;

namespace GateServer.Core.Behaviour
{
    public class RoomBehaviour : BaseBehaviour
    {
        public RoomBehaviour()
        {

        }

        #region web-socket-events
        protected override void OnOpen()
        {
            base.OnOpen();
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
        }
        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }
        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
        }
        protected override void OnCloseEventImpl(NetMessageEvt evt)
        {
            base.OnCloseEventImpl(evt);
            NetworkModule.GetModule<RoomNetworkModule>().EnqueueNetworkEvents(evt);
        }
        protected override void OnErrorEventImpl(NetMessageEvt evt)
        {
            base.OnErrorEventImpl(evt);
            NetworkModule.GetModule<RoomNetworkModule>().EnqueueNetworkEvents(evt);
        }
        protected override void OnMessageEventImpl(NetMessageEvt evt)
        {
            base.OnMessageEventImpl(evt);
            NetworkModule.GetModule<RoomNetworkModule>().EnqueueNetworkEvents(evt);
        }
        protected override void OnOpenEventImpl(NetMessageEvt evt)
        {
            base.OnOpenEventImpl(evt);
            NetworkModule.GetModule<RoomNetworkModule>().EnqueueNetworkEvents(evt);
        }
        #endregion

    }
}
