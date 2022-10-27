using Net;
using Net.ServiceImpl;
using RoomServer.Core.Behaviour;
using RoomServer.Core.Data;

using RoomServer.Services.Sim;
using ServerDll.Service;
using ServerDll.Service.Module;
using ServerDll.Service.Provider;
using ServerDll.Service.Requester;
using Service.Core;
using WebSocketSharp;

namespace RoomServer.Services
{
    public enum RoomApplicationEventType
    {
        PlayerDisconnect,
    }
    public class RoomApplication:BaseApplication
    {
        public const int MAX_CONNECT_COUNT = 128;
        private NetworkRequesterWrap requesterWrap;
        public RoomApplication(ushort port) : base(port)
        {
            m_NetworkType = NetworkType.WSS;
            DataMgr.Instance.Init();
            Evt.EventMgr<RoomApplicationEventType, byte[]>.AddListener(RoomApplicationEventType.PlayerDisconnect, OnPlayerDisconnect);
        }
        public void InitRoomProcessWs(string gateServerName,ushort port)
        {
            //requesterWrap = new NetworkRequesterWrap(m_NetworkType);
            //ServerDll.Service.Logger.LogInfo("InitRoomProcess "+gateServerName + " Port "+port);
            //requesterWrap.Connect("127.0.0.1",port,gateServerName);
        }
        void OnPlayerDisconnect(byte[] raw)
        {
            //if (requesterWrap != null)
                //requesterWrap.Send(PtMessagePackage.Write(PtMessagePackage.Build((ushort)RequestMessageId.UGS_RoomPlayerDisconnect, raw)));
        }
        protected override void SetUp()
        {
            base.SetUp();
            SetUpSimulation();
            NetworkModule.AddModule(new BattleNetworkModule(providerWrap.Provider));
            if(providerWrap.Type == NetworkType.WSS)
            {
                WebsocketProvider websocketProvider = providerWrap.Provider as WebsocketProvider;
                if(websocketProvider != null)
                    websocketProvider.GetSocket().AddWebSocketService("/BattleBehaviour", () => new BattleBehaviour(websocketProvider));
            }
    

        }
        void SetUpSimulation()
        {
            Simulation simulation = new Simulation(1);
            SimulationManager.Instance.SetSimulation(simulation);
            ServerLogicFrameBehaviour frame = new ServerLogicFrameBehaviour(providerWrap.Provider);
            simulation.AddBehaviour(frame);
            ServerDll.Service.Logger.LogInfo("Simulation has been Created.");
        }
    }
}
