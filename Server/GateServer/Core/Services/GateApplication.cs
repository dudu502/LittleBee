
using Service.Core;
using System;
using System.Collections.Generic;

using GateServer.Core.Data;
using LitJson;

using GateServer.Core.Behaviour;
using ServerDll.Service.Behaviour;

namespace GateServer.Services
{
    public class GateApplication : BaseApplication
    {
        public Dictionary<string,string> StartupConfigs;
        public GateApplication(int port) : base(port)
        {
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
            NetworkModule.AddModule(new RoomProcessNetworkModule(m_WebsocketServer));
            NetworkModule.AddModule(new RoomNetworkModule(m_WebsocketServer));
            m_WebsocketServer.AddWebSocketService("/RoomBehaviour", () => new RoomBehaviour());
            m_WebsocketServer.AddWebSocketService("/RoomProcessBehaviour", () => new RoomProcessBehaviour());
            
        }
    }
}
