using Misc;
using Net;
using Net.Pt;
using Net.ServiceImpl;
using RoomServer.Core.Data;
using RoomServer.Services;
using RoomServer.Services.Sim;
using ServerDll.Service.Provider;
using ServerDll.Service.Provider.Websocket;
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
        public BattleBehaviour(IProvider providerParam):base(providerParam)
        {
         
        }


       

        protected override void OnOpen()
        {
            base.OnOpen();
        }
        
        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
        {
            base.OnMessage(e);
        }
      
    }
}
