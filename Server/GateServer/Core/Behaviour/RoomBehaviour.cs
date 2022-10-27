using Evt;
using GateServer.Core.Data;
using Net;
using Net.Pt;
using Net.ServiceImpl;
using ServerDll.Service.Provider;
using ServerDll.Service.Provider.Websocket;
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
        public RoomBehaviour(IProvider provider) : base(provider)
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
       
        #endregion

    }
}
