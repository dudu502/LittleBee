
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Net;
using Net.ServiceImpl;
using LitJson;
using Synchronize.Game.Lockstep.Service.Core;
using Synchronize.Game.Lockstep.Service.Event;
using Synchronize.Game.Lockstep.Service.Modules;
using Synchronize.Game.Lockstep.GateServer.Modules;

namespace Synchronize.Game.Lockstep.GateServer.Services
{
    public class GateApplication : BaseApplication
    {
        public Dictionary<string,string> StartupConfigs;
        public GateApplication(string appKey) : base(appKey)
        {
            m_Network.UnconnectedMessagesEnabled = true;
        }
        public GateApplication(string appKey,Dictionary<string,string> startUpCfgs) : base(appKey)
        {
            m_Network.UnconnectedMessagesEnabled = true;
            StartupConfigs = startUpCfgs;
        }
        protected override void CallCustomEvent()
        {
            base.CallCustomEvent();
        }
        protected override void OnConnectionRequestEvent(ConnectionRequest request)
        {
            base.OnConnectionRequestEvent(request);
            if (m_Network.ConnectedPeersCount < Convert.ToInt32(StartupConfigs["MaxConnectionCount"]))
            {
                NetPeer peer = request.AcceptIfKey(m_ApplicationKey);
                Logger.Log($"NetPeer Endpoint:{peer.EndPoint.ToString()}");
                NetStreamUtil.Send(peer, (ushort)ResponseMessageId.GS_ClientConnected,new ByteBuffer().WriteInt32(GetHashCode()).Getbuffer());
            }
            else
            {
                request.Reject();
            }
        }
        protected override void OnDeliveryEvent(NetPeer peer, object userData)
        {
            base.OnDeliveryEvent(peer, userData);
        }
        protected override void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            if (peer.ConnectionState == ConnectionState.Connected)
            {
                base.OnNetworkReceiveEvent(peer, reader, deliveryMethod);
                byte[] bytes = new byte[reader.AvailableBytes];
                reader.GetBytes(bytes, reader.AvailableBytes);
                PtMessagePackage package = PtMessagePackage.Read(bytes);
                Evt.EventMgr<RequestMessageId, NetMessageEvt>.TriggerEvent((RequestMessageId)package.MessageId, new NetMessageEvt(peer, package.Content));
                reader.Recycle();
                Logger.Log("PeerId " + peer.Id + " ReceiveEvent MessageID " + (RequestMessageId)package.MessageId + " ContentSize " + (package.Content != null ? package.Content.Length : 0));
            }    
        }
        protected override void OnPeerConnectedEvent(NetPeer peer)
        {
            
        }
        protected override void OnNetworkErrorEvent(IPEndPoint endPoint, SocketError socketError)
        {
            base.OnNetworkErrorEvent(endPoint, socketError);
        }
        protected override void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            base.OnPeerDisconnectedEvent(peer, disconnectInfo);
        }
        protected override void OnNetworkReceiveUnconnectedEvent(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            base.OnNetworkReceiveUnconnectedEvent(remoteEndPoint, reader, messageType);
            byte[] bytes = new byte[reader.AvailableBytes];
            reader.GetBytes(bytes, reader.AvailableBytes);
            PtMessagePackage package = PtMessagePackage.Read(bytes);
            Evt.EventMgr<RequestMessageId, UnconnectedNetMessageEvt>.TriggerEvent((RequestMessageId)package.MessageId, new UnconnectedNetMessageEvt(remoteEndPoint, package.Content));
            reader.Recycle();
        }
        protected override void OnNetworkLatencyUpdateEvent(NetPeer peer, int latency)
        {
            base.OnNetworkLatencyUpdateEvent(peer, latency);
        }
        protected override void SetUp()
        {
            base.SetUp();
#if WAN_MODE
            if (System.IO.File.Exists("Startup.json"))
            {
                string startUpJson = System.IO.File.ReadAllText("Startup.json");
                StartupConfigs = JsonMapper.ToObject<Dictionary<string,string>>(startUpJson);
                if (StartupConfigs == null)
                {
                    throw new Exception("Startup.json is not exist at " + Environment.CurrentDirectory);
                }
            }
#else
            
#endif
            AddModule(new UserModule(this)); 
            AddModule(new RoomModule(this));
            AddModule(new HeartbeatModule(this,15));

          
        }
    }
}
