
using Service.Core;
using System;
using System.Collections.Generic;

using GateServer.Core.Data;
using LitJson;

using GateServer.Core.Behaviour;

using ServerDll.Service.Provider;
using ServerDll.Service;
using ServerDll.Service.Module;
using ServerDll.Service.Provider.KCP;

namespace GateServer.Services
{
    public class GateApplication : BaseApplication
    {
        public Dictionary<string,string> StartupConfigs;
        public GateApplication(ushort port) : base(port)
        {
            m_NetworkType = NetworkType.WSS;
            DataMgr.Instance.Init();
            DataMgr.Instance.GateServerWSPort = port;
        }
       

        protected override void SetUp()
        {
            base.SetUp();

            if (System.IO.File.Exists("Startup.json"))
            {
                string startUpJson = System.IO.File.ReadAllText("Startup.json");
                DataMgr.Instance.StartupConfigs = JsonMapper.ToObject<Dictionary<string,string>>(startUpJson);
                if (DataMgr.Instance.StartupConfigs == null)
                {
                    throw new Exception("Startup.json format ERROR! " + Environment.CurrentDirectory);
                }
            }
            NetworkModule.AddModule(new RoomNetworkModule(providerWrap.Provider));
 
            if(providerWrap.Type == NetworkType.WSS)
            {
                WebsocketProvider websocketProvider = providerWrap.Provider as WebsocketProvider;
                if(websocketProvider != null)
                {
                    websocketProvider.GetSocket().AddWebSocketService("/RoomBehaviour", () => new RoomBehaviour(websocketProvider));
                }
            }
        }

    }
}
