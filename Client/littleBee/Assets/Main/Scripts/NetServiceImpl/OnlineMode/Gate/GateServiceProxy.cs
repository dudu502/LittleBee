using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Net;
using Net.Pt;
using Net.ServiceImpl;
using Synchronize.Game.Lockstep;
using Synchronize.Game.Lockstep.Evt;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.Managers.UI;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Net;
using Synchronize.Game.Lockstep.Notification;
using Synchronize.Game.Lockstep.Proxy;
using Synchronize.Game.Lockstep.UI;

namespace Synchronize.Game.Lockstep.Gate
{
    public class GateServiceProxy:NetServiceProxy
    {
        private List<GateAddressVO> HostResults;
        private GateAddressVO currentGateHost;
        List<IPAddress> LocalIPAddresses = new List<IPAddress>();
        public PtRoom SelfRoom { private set; get; }
        public GateServiceProxy()
        {
            HostResults = new List<GateAddressVO>();

            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_ClientConnected, OnResponseGateServerCliented);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.UGS_SearchAvailableGate, OnSearchAvailableGate);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_UpdateRoom, OnResponseUpdateRoom);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_RoomList, OnResponseRoomList);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_CreateRoom, OnResponseCreateRoom);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_JoinRoom, OnResponseJoinRoom);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_ErrorCode, OnResponseErrorCode);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LeaveRoom, OnResponseLeaveRoom);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LaunchGame,OnResponseLaunchGame);
            EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LaunchRoomInstance, OnResponseLaunchRoomInstance);
        }

        protected override void OnInit()
        {
            base.OnInit();
            CollectIpAddress();
        }
        public void UpdateCurrentGateAddress(GateAddressVO gate)
        {
            currentGateHost = gate;
        }
        #region Req-更新房间 0:改变队伍 1:改变颜色 2:改变地图
        public void RequestUpdatePlayerTeam(uint roomId,string userId,byte teamId)
        {
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.Build((ushort)RequestMessageId.GS_UpdateRoom, new ByteBuffer().WriteByte(0).WriteUInt32(roomId).WriteString(userId).WriteByte(teamId).Getbuffer()));
        }
        public void RequestUpdatePlayerColor(uint roomId,string userId)
        {
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.Build((ushort)RequestMessageId.GS_UpdateRoom, new ByteBuffer().WriteByte(1).WriteUInt32(roomId).WriteString(userId).Getbuffer()));
        }
        public void RequestUpdateMap(uint roomId,uint mapId,byte cfgMaxPlayerCount)
        {
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.Build((ushort)RequestMessageId.GS_UpdateRoom, new ByteBuffer().WriteByte(2).WriteUInt32(roomId).WriteUInt32(mapId).WriteByte(cfgMaxPlayerCount).Getbuffer()));
        }
        #endregion

        #region 请求创建房间
        public void RequestCreateRoom(uint mapId)
        {
            string userId = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].GetId();
            string userName = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].name;
            string userPwd = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].GetPassword();
            byte teamId = 1;
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.BuildParams((ushort)RequestMessageId.GS_CreateRoom, mapId, userId, userName, userPwd, teamId));
        }

        void OnResponseCreateRoom(PtMessagePackage package)
        {
            PtRoom room = PtRoom.Read(package.Content);
            SelfRoom = room;
            TriggerMainThreadEvent(EvtGate.UpdateCreateRoom, room);
            //TriggerMainThreadEvent(EventType.UpdateCurrentRoom, room);
        }
        #endregion
        #region 请求加入房间
        public void RequestJoinRoom(PtRoom room)
        {
            string userId = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].GetId();
            string userName = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].name;
            string userPwd = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].GetPassword();
            byte teamId = 1;
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.BuildParams((ushort)RequestMessageId.GS_JoinRoom,room.RoomId, userId, userName, userPwd, teamId));
        }
        void OnResponseUpdateRoom(PtMessagePackage package)
        {
            PtRoom room = PtRoom.Read(package.Content);
            SelfRoom = room;
            TriggerMainThreadEvent(EvtGate.UpdateCurrentRoom, room);
        }
        void OnResponseJoinRoom(PtMessagePackage package)
        {
            using(ByteBuffer buffer =new ByteBuffer(package.Content))
            {
                byte errorCode = buffer.ReadByte();
                if (0 == errorCode)
                    TriggerMainThreadEvent<EvtGate, object>(EvtGate.OpenRoomPanel, null);
                else
                    ToastManager.Instance.ShowToast(Localization.Localization.GetTranslation("Error") + errorCode);
            }
        }

        #endregion
        #region 错误码处理
        void OnResponseErrorCode(PtMessagePackage package)
        {
            PtErrorCode error = PtErrorCode.Read(package.Content);
            ToastManager.Instance.ShowToast(Localization.Localization.GetTranslation("Error") + error.Id) ;
            //TriggerMainThreadEvent<Tips.Alert, int>(Tips.Alert.Error, error.Id);
        }
        #endregion
        #region 请求离开房间
        public void RequestLeaveRoom()
        {
            if(SelfRoom!=null)
                GameClientNetwork.Instance.SendRequest(PtMessagePackage.BuildParams((ushort)RequestMessageId.GS_LeaveRoom,SelfRoom.RoomId));
        }
        void OnResponseLeaveRoom(PtMessagePackage package)
        {
            SelfRoom = null;
        }
        #endregion
        #region 启动游戏
        public void RequestLaunchGame()
        {
            if(SelfRoom != null)
                GameClientNetwork.Instance.SendRequest(RequestMessageId.GS_LaunchGame,SelfRoom.RoomId);
        }
        void OnResponseLaunchGame(PtMessagePackage package)
        {
            UnityEngine.Debug.Log("OnResponseLaunchGame");
            Handler.Run((pack) => 
            {
                ModuleManager.GetModule<GameContentRootModule>().SetWorldEnable(false);
                ModuleManager.GetModule<UIModule>().Push(UITypes.LoadingPanel, Layer.Top, new LoadingPanel.LoadingInfo(Localization.Localization.GetTranslation("Loading"), 0));
             
                EventMgr<LoadingPanel.EventType, LoadingPanel.LoadingInfo>.TriggerEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Localization.Localization.GetTranslation("Loading"), 0.1f));
            }, package);         
        }
        void OnResponseLaunchRoomInstance(PtMessagePackage package)
        {
            UnityEngine.Debug.Log("OnResponseLaunchRoomInstance");
            PtLaunchGameData ptLaunchGameData = PtLaunchGameData.Read(package.Content);

            TriggerMainThreadEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Localization.Localization.GetTranslation("Create room service complete"), 0.4f));

            Handler.Run(_ => BattleEntryPoint.Start(ptLaunchGameData,GateServerIP),null);        
        }
        #endregion
        #region 采集IP
        void CollectIpAddress()
        {
            try
            {
                string hostName = System.Net.Dns.GetHostName();
                System.Net.IPAddress[] addresses = System.Net.Dns.GetHostAddresses(hostName);
                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        LocalIPAddresses.Add(address);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Collect Local ip "+ ex.ToString());
            }
        }
        #endregion
        #region 搜索可用服务器
        public async void RefreshLanGates(ushort atPort)
        {
            HostResults.Clear();
            logger.Log("Search start");
            await Task.Yield();
            await Task.Run(() =>
            {
                foreach (IPAddress ip in LocalIPAddresses)
                {
                    byte[] ipbytes = ip.GetAddressBytes();
                    for (int i = 0; i <= 255; ++i)
                    {
                        if (i != ipbytes[3])
                        {
                            byte[] remoteIpBytes = new byte[4];
                            Buffer.BlockCopy(ipbytes, 0, remoteIpBytes, 0, 3);
                            remoteIpBytes[3] = (byte)i;
                            IPEndPoint remotePoint = new IPEndPoint(new IPAddress(remoteIpBytes), atPort);
                            logger.Log("Search "+ remotePoint.Address.ToString()+" at "+atPort );
                            GameClientNetwork.Instance.SendUnconnectedRequest(PtMessagePackage.Build((ushort)RequestMessageId.UGS_SearchAvailableGate), remotePoint);
                        }
                    }
                }
            });
            logger.Log("Search finish");
        }
        public void SearchGates(ushort atPort)
        {
            GameClientNetwork.Instance.CloseClient();
            GameClientNetwork.Instance.Launch();
            GameClientNetwork.Instance.Start();
            RefreshLanGates(atPort);
        }
        void OnSearchAvailableGate(PtMessagePackage package)
        {
            logger.Log(package.ExtraObj.ToString());
            PtSearchHostResult searchResult = PtSearchHostResult.Read(package.Content);
            logger.Log(searchResult.hashCode.ToString());
;
            var result = HostResults.Find((host) => host.HashCode == searchResult.hashCode);
            if(result==null)
            {
                HostResults.Add(new GateAddressVO() {HashCode = searchResult.hashCode,RemotePoint = package.ExtraObj as IPEndPoint,ConnectKey = searchResult.connectKey });
            }
            TriggerMainThreadEvent(EvtGate.UpdateLANGateServerList, HostResults);
        }
        #endregion
        #region 请求连接gate服务器
        public string GateServerIP;
        public void Connect2GateServer()
        {
            if (currentGateHost != null)
            {
                GateServerIP = currentGateHost.RemotePoint.Address.ToString();
                int port = currentGateHost.RemotePoint.Port;
                string key = currentGateHost.ConnectKey;
                GameClientNetwork.Instance.CloseClient();
                GameClientNetwork.Instance.Launch();
                GameClientNetwork.Instance.Start(GateServerIP, port, key);
            }           
        }
      


        void OnResponseGateServerCliented(PtMessagePackage package)
        {
            TriggerMainThreadEvent<EvtGate, object>(EvtGate.GateServerConnected,null);
            using(ByteBuffer buffer = new ByteBuffer(package.Content))
            {
                int hashcode = buffer.ReadInt32();
                logger.LogWarning("client connected at hashcode "+hashcode);
           
                if (!DataProxy.Get<UserDataProxy>().UserLoginInfo.IsEmpty())
                {
                    GameClientNetwork.Instance.SendRequest(RequestMessageId.GS_EnterGate, DataProxy.Get<UserDataProxy>().GetUserId());
                }
               
            }
        }
        #endregion
        #region 请求房间列表
        public void RequestRoomList()
        {
            GameClientNetwork.Instance.SendRequest(PtMessagePackage.Build((ushort)RequestMessageId.GS_RoomList));
        }
        void OnResponseRoomList(PtMessagePackage package)
        {
            PtRoomList roomList = PtRoomList.Read(package.Content);
            //logger.Log(roomList.ToString()+"");
            TriggerMainThreadEvent(EvtGate.UpdateRoomList, roomList);
        }
        #endregion
    }
}
