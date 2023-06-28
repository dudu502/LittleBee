public enum KeyboardDirectionInput
{
    Up = 1,
    Left = 2,
    Down = 4,
    Right = 8,
}

public enum PlayBattleMode
{
    PlayRealBattle,
    PlayReplayBattle,
}

public enum EvtReplay
{
    UpdateFrameCount,
}

#region EventType
public enum EvtGate
{
    GateServerConnected,
    UpdateLANGateServerList,
    UpdateWANGateServerList,
    UpdateRoomList,
    UpdateSelfRoom,
    UpdateCreateRoom,
    UpdateCurrentRoom,

    OpenRoomPanel,
}
#endregion