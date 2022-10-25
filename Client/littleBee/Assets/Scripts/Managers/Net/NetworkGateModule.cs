using Evt;
using Localization;
using Managers;
using Managers.UI;
using Misc;
using Net;
using Net.Pt;
using Net.ServiceImpl;
using Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameClientNetwork;

public class NetworkGateModule : IModule
{
    public PtRoom SelfRoom { private set; get; }
    protected void TriggerMainThreadEvent<T, P>(T type, P paramObj)
    {
        Misc.Handler.Run((item) => { Evt.EventMgr<T, P>.TriggerEvent(type, paramObj); }, null);
    }
    public void Init()
    {
        EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_ClientConnected, OnResponseClientConnected);
        EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_CreateRoom, OnResponseCreateRoom);
        EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_UpdateRoom, OnResponseUpdateRoom);
        EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_RoomList, OnResponseRoomList);
        EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_JoinRoom, OnResponseJoinRoom);
        EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_ErrorCode, OnResponseErrorCode);
        EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LeaveRoom, OnResponseLeaveRoom);
        EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LaunchGame, OnResponseLaunchGame);
        EventMgr<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LaunchRoomInstance, OnResponseLaunchRoomInstance);
    }
    void OnResponseClientConnected(PtMessagePackage value)
    {
        TriggerMainThreadEvent<EvtGate, object>(EvtGate.GateServerConnected, null);
        GameClientNetwork.Instance.SendAsync(PtMessagePackage.BuildParams((ushort)RequestMessageId.GS_EnterGate, DataProxy.Get<UserDataProxy>().GetUserId()), null);
        
    }
    public void ConnectToGateServer()
    {
        string gateWs = DataProxy.Get<UserDataProxy>().GateWS;
        if (!string.IsNullOrEmpty(gateWs))
        {
            GameClientNetwork.Instance.Start(gateWs,State.Gate);
        }
    }

    #region 请求创建房间
    public void RequestCreateRoom(uint mapId)
    {
        string userId = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].GetId();
        string userName = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].name;
        string userPwd = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].GetPassword();
        byte teamId = 1;
        GameClientNetwork.Instance.SendAsync(PtMessagePackage.BuildParams((ushort)RequestMessageId.GS_CreateRoom, mapId, userId, userName, userPwd, teamId), null);
    }

    void OnResponseCreateRoom(PtMessagePackage package)
    {
        PtRoom room = PtRoom.Read(package.Content);
        SelfRoom = room;
        TriggerMainThreadEvent(EvtGate.UpdateCreateRoom, room);
        Debug.Log("OnResponseCreateRoom");
    }
    #endregion

    #region Req-更新房间 0:改变队伍 1:改变颜色 2:改变地图
    public void RequestUpdatePlayerTeam(uint roomId, string userId, byte teamId)
    {
        GameClientNetwork.Instance.SendAsync(PtMessagePackage.Build((ushort)RequestMessageId.GS_UpdateRoom, new ByteBuffer().WriteByte(0).WriteUInt32(roomId).WriteString(userId).WriteByte(teamId).GetRawBytes()),null);
    }
    public void RequestUpdatePlayerColor(uint roomId, string userId)
    {
        GameClientNetwork.Instance.SendAsync(PtMessagePackage.Build((ushort)RequestMessageId.GS_UpdateRoom, new ByteBuffer().WriteByte(1).WriteUInt32(roomId).WriteString(userId).GetRawBytes()),null);
    }
    public void RequestUpdateMap(uint roomId, uint mapId, byte cfgMaxPlayerCount)
    {
        GameClientNetwork.Instance.SendAsync(PtMessagePackage.Build((ushort)RequestMessageId.GS_UpdateRoom, new ByteBuffer().WriteByte(2).WriteUInt32(roomId).WriteUInt32(mapId).WriteByte(cfgMaxPlayerCount).GetRawBytes()),null);
    }
    #endregion
    #region 请求加入房间
    public void RequestJoinRoom(PtRoom room)
    {
        string userId = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].GetId();
        string userName = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].name;
        string userPwd = DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].GetPassword();
        byte teamId = 1;
        GameClientNetwork.Instance.SendAsync(PtMessagePackage.BuildParams((ushort)RequestMessageId.GS_JoinRoom,room.RoomId, userId, userName, userPwd, teamId),null);
    }
    void OnResponseUpdateRoom(PtMessagePackage package)
    {
        PtRoom room = PtRoom.Read(package.Content);
        SelfRoom = room;
        TriggerMainThreadEvent(EvtGate.UpdateCurrentRoom, room);
    }
    void OnResponseJoinRoom(PtMessagePackage package)
    {
        using (ByteBuffer buffer = new ByteBuffer(package.Content))
        {
            byte errorCode = buffer.ReadByte();
            if (0 == errorCode)
                TriggerMainThreadEvent<EvtGate, object>(EvtGate.OpenRoomPanel, null);
            else
                ToastRoot.Instance.ShowToast(Language.GetText(200000) + errorCode);
        }
    }

    #endregion

    #region 错误码处理
    void OnResponseErrorCode(PtMessagePackage package)
    {
        PtErrorCode error = PtErrorCode.Read(package.Content);
        ToastRoot.Instance.ShowToast(Language.GetText(200000) + error.Id);
    }
    #endregion
    #region 请求离开房间
    public void RequestLeaveRoom()
    {
        if(SelfRoom!=null)
            GameClientNetwork.Instance.SendAsync(PtMessagePackage.BuildParams((ushort)RequestMessageId.GS_LeaveRoom,SelfRoom.RoomId),null);
    }
    void OnResponseLeaveRoom(PtMessagePackage package)
    {
        SelfRoom = null;
    }
    #endregion

    #region 请求房间列表
    public void RequestRoomList()
    {
        GameClientNetwork.Instance.SendAsync(PtMessagePackage.Build((ushort)RequestMessageId.GS_RoomList),null);
    }
    void OnResponseRoomList(PtMessagePackage package)
    {
        PtRoomList roomList = PtRoomList.Read(package.Content);
        Debug.Log(roomList.ToString()+"");
        TriggerMainThreadEvent(EvtGate.UpdateRoomList, roomList);
    }
    #endregion


    #region 启动游戏
    public void RequestLaunchGame()
    {
        if(SelfRoom != null)
           GameClientNetwork.Instance.SendAsync(PtMessagePackage.BuildParams((ushort)RequestMessageId.GS_LaunchGame,SelfRoom.RoomId),null);
    }
    void OnResponseLaunchGame(PtMessagePackage package)
    {
        Debug.Log("OnResponseLaunchGame");
        Handler.Run((pack) =>
        {
            ModuleManager.GetModule<GameContentRootModule>().SetWorldEnable(false);
            ModuleManager.GetModule<UIModule>().Push(UITypes.LoadingPanel, Layer.Bottom, new LoadingPanel.LoadingInfo(Language.GetText(27), 0));

            Evt.EventMgr<LoadingPanel.EventType, LoadingPanel.LoadingInfo>.TriggerEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Language.GetText(27), 0.1f));
        }, package);
    }
    void OnResponseLaunchRoomInstance(PtMessagePackage package)
    {
        Debug.Log("OnResponseLaunchRoomInstance");
        PtLaunchGameData ptLaunchGameData = PtLaunchGameData.Read(package.Content);
        TriggerMainThreadEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Language.GetText(31), 0.4f));
        Handler.Run(_ => LogicFrameSync.Src.LockStep.BattleEntryPoint.Start(ptLaunchGameData), null);
    }
    #endregion

}
