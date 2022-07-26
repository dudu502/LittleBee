namespace Net.ServiceImpl
{
    public enum ResponseMessageId : ushort
    {
        US_DebugQuery,
        UIS_WebMessage,
        UGS_SearchAvailableGate,
        UGS_RoomPlayerDisconnect,
        RS_ClientConnected,
        GS_ClientConnected,
        GS_EnterGate,
        GS_RoomList,
        GS_CreateRoom,
        GS_UpdateRoom,
        GS_JoinRoom,
        GS_LeaveRoom,
        GS_LaunchGame,
        GS_LaunchRoomInstance,
        GS_ErrorCode,

        RS_EnterRoom,
        RS_SyncKeyframes,
        RS_PlayerReady,
        RS_InitPlayer,
        RS_InitSelfPlayer,
        RS_AllUserState,
        RS_HistoryKeyframes,
    }

    public enum RequestMessageId : ushort
    {
        ULS_LogMessage,
        US_DebugQuery,
        UIS_WebMessage,
        UGS_SearchAvailableGate,
        UGS_RoomPlayerDisconnect,
        GS_EnterGate,
        GS_RoomList,
        GS_CreateRoom,
        GS_UpdateRoom,
        GS_JoinRoom,
        GS_LeaveRoom,
        GS_LaunchGame,

        RS_EnterRoom,
        RS_SyncClientKeyframes,
        RS_InitPlayer,
        RS_PlayerReady,
        RS_HeartBeat,
        RS_HistoryKeyframes,
    }
}
