using Net;
using Net.ServiceImpl;
using RoomServer.Core.Behaviour;
using RoomServer.Core.Data;

using RoomServer.Services.Sim;
using ServerDll.Service.Behaviour;
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
        private WebSocket m_RoomProcessSocket;
        public RoomApplication(int port) : base(port)
        {
            DataMgr.Instance.Init();
            Evt.EventMgr<RoomApplicationEventType, byte[]>.AddListener(RoomApplicationEventType.PlayerDisconnect, OnPlayerDisconnect);
        }
        async public void InitRoomProcessWs(string gateServerName,int port)
        {
            m_RoomProcessSocket = new WebSocket($"ws://127.0.0.1:{port}/{gateServerName}");
            await System.Threading.Tasks.Task.Run(()=> m_RoomProcessSocket.Connect());
        }
        async void OnPlayerDisconnect(byte[] raw)
        {
            if (m_RoomProcessSocket != null)
            {
                await System.Threading.Tasks.Task.Run(()=>
                    m_RoomProcessSocket.Send(PtMessagePackage.Write(PtMessagePackage.Build((ushort)RequestMessageId.UGS_RoomPlayerDisconnect, raw))));              
            }
        }
        protected override void SetUp()
        {
            base.SetUp();
            SetUpSimulation();
            NetworkModule.AddModule(new BattleNetworkModule(m_WebsocketServer));
            m_WebsocketServer.AddWebSocketService("/BattleBehaviour", ()=> new BattleBehaviour());

        }
        void SetUpSimulation()
        {
            Simulation simulation = new Simulation(1);
            SimulationManager.Instance.SetSimulation(simulation);
            ServerLogicFrameBehaviour frame = new ServerLogicFrameBehaviour(m_WebsocketServer);
            simulation.AddBehaviour(frame);
            Logger.Log("Simulation has been Created.");
        }
    }
}
