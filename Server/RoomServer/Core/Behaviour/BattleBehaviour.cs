using Misc;
using Net;
using Net.Pt;
using Net.ServiceImpl;
using RoomServer.Core.Data;
using RoomServer.Services;
using RoomServer.Services.Sim;
using ServerDll.Service.Behaviour;
using Service.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoomServer.Core.Behaviour
{
    public class BattleBehaviour:BaseBehaviour
    {
        public BattleBehaviour()
        {
         
        }


       

        protected override void OnOpen()
        {
            base.OnOpen();
        }
        protected override void OnOpenEventImpl(NetMessageEvt evt)
        {
            base.OnOpenEventImpl(evt);
            NetworkModule.GetModule<BattleNetworkModule>().EnqueueNetworkEvents(evt);
        }

        protected override void OnClose(WebSocketSharp.CloseEventArgs e)
        {
            base.OnClose(e);
        }
        protected override void OnCloseEventImpl(NetMessageEvt evt)
        {
            base.OnCloseEventImpl(evt);
            NetworkModule.GetModule<BattleNetworkModule>().EnqueueNetworkEvents(evt);
        }
        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            base.OnError(e);
        }
        protected override void OnErrorEventImpl(NetMessageEvt evt)
        {
            base.OnErrorEventImpl(evt);
            NetworkModule.GetModule<BattleNetworkModule>().EnqueueNetworkEvents(evt);
        }
        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
        {
            base.OnMessage(e);
        }
        protected override void OnMessageEventImpl(NetMessageEvt evt)
        {
            base.OnMessageEventImpl(evt);
            NetworkModule.GetModule<BattleNetworkModule>().EnqueueNetworkEvents(evt);
        }

    }
}
