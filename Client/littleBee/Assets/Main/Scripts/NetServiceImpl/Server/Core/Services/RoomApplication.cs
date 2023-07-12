using LiteNetLib;
using Net;
using Net.ServiceImpl;
using Synchronize.Game.Lockstep.Evt;
using Synchronize.Game.Lockstep.RoomServer.Modules;
using Synchronize.Game.Lockstep.RoomServer.Services.Sim;
using Synchronize.Game.Lockstep.Service.Core;
using Synchronize.Game.Lockstep.Service.Event;
using System.Net;
using System.Net.Sockets;

namespace Synchronize.Game.Lockstep.RoomServer.Services
{
    public class RoomApplication:BaseApplication
    {
        public const int MAX_CONNECT_COUNT = 128;
        public RoomApplication(string key) : base(key)
        {
            m_Network.UnconnectedMessagesEnabled = true;
        }
        protected override void SetUp()
        {
            base.SetUp();
            SetUpSimulation();
            AddModule(new BattleModule(this));       
        }

        public override void Dispose()
        {
            Synchronize.Game.Lockstep.RoomServer.Services.Sim.SimulationManager.Instance.Stop();
            base.Dispose();
        }

        void SetUpSimulation()
        {
            Synchronize.Game.Lockstep.RoomServer.Services.Sim.Simulation simulation = new Synchronize.Game.Lockstep.RoomServer.Services.Sim.Simulation(1);
            Synchronize.Game.Lockstep.RoomServer.Services.Sim.SimulationManager.Instance.SetSimulation(simulation);
            ServerLogicFrameBehaviour frame = new ServerLogicFrameBehaviour();
            simulation.AddBehaviour(frame);
            Logger.Log("Simulation has been Created.");
        }
        protected override void CallCustomEvent()
        {
            base.CallCustomEvent();
        }
        protected override void OnConnectionRequestEvent(ConnectionRequest request)
        {
            base.OnConnectionRequestEvent(request);
            Logger.Log("OnConnection...MAX_CONNECT_COUNT="+ MAX_CONNECT_COUNT);
            if (m_Network.ConnectedPeerList.Count< MAX_CONNECT_COUNT)
            {
                NetPeer peer = request.AcceptIfKey(m_ApplicationKey);
                NetStreamUtil.Send(peer, (ushort)ResponseMessageId.RS_ClientConnected, null);
                Logger.Log(string.Format("PeerId:{0} AcceptConnectRequest.[{1}/{2}]", peer.Id, m_Network.ConnectedPeerList.Count, MAX_CONNECT_COUNT));
            }
            else
            {
                request.Reject();
            }
        }

        protected override void OnDeliveryEvent(NetPeer peer, object userData)
        {
            base.OnDeliveryEvent(peer, userData);
            //Logger.Log("PeerId " + peer.Id + "DeliveryEvent "+userData);
        }
        protected override void OnNetworkErrorEvent(IPEndPoint endPoint, SocketError socketError)
        {
            base.OnNetworkErrorEvent(endPoint, socketError);
            Logger.LogError("NetworkError "+socketError.ToString()+" EndPoint "+ endPoint.ToString());
        }
        protected override void OnNetworkLatencyUpdateEvent(NetPeer peer, int latency)
        {
            base.OnNetworkLatencyUpdateEvent(peer, latency);
        }
        protected override void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            if(peer.ConnectionState == ConnectionState.Connected)
            {
                base.OnNetworkReceiveEvent(peer, reader, deliveryMethod);
                byte[] bytes = new byte[reader.AvailableBytes];
                reader.GetBytes(bytes, reader.AvailableBytes);
                PtMessagePackage package = PtMessagePackage.Read(bytes);
                EventMgr<RequestMessageId, NetMessageEvt>.TriggerEvent((RequestMessageId)package.MessageId, new NetMessageEvt(peer, package.Content));
                reader.Recycle();
                Logger.Log("PeerId " + peer.Id + " ReceiveEvent MessageID " + (RequestMessageId)package.MessageId + " ContentSize " + (package.Content != null ? package.Content.Length : 0));
            }
        }
        protected override void OnNetworkReceiveUnconnectedEvent(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            base.OnNetworkReceiveUnconnectedEvent(remoteEndPoint, reader, messageType);
            byte[] bytes = new byte[reader.AvailableBytes];
            reader.GetBytes(bytes, reader.AvailableBytes);
            PtMessagePackage package = PtMessagePackage.Read(bytes);
            EventMgr<RequestMessageId, UnconnectedNetMessageEvt>.TriggerEvent((RequestMessageId)package.MessageId, new UnconnectedNetMessageEvt(remoteEndPoint, package.Content));
            reader.Recycle();
        }
        protected override void OnPeerConnectedEvent(NetPeer peer)
        {
            base.OnPeerConnectedEvent(peer);
            Logger.Log(string.Format("PeerId:{0} Connected.[{1}/{2}]", peer.Id, m_Network.ConnectedPeerList.Count, MAX_CONNECT_COUNT));
        }
        protected override void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            base.OnPeerDisconnectedEvent(peer, disconnectInfo); 
            Logger.LogWarning(string.Format("PeerId:{0} DisConnected.[{1}/{2}] DisconnectReason:{3} SocketError:{4}", peer.Id, m_Network.ConnectedPeerList.Count, MAX_CONNECT_COUNT,disconnectInfo.Reason.ToString(),disconnectInfo.SocketErrorCode.ToString()));
        }
    }
}
