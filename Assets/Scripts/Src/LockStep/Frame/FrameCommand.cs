namespace LogicFrameSync.Src.LockStep.Frame
{
    /// <summary>
    /// 关键帧同步命令类型
    /// 绑定FrameIdxInfo.Cmd字段
    /// </summary>
    public class FrameCommand
    {
        public const int SYNC_CREATE_ENTITY = 10000;
        public const int SYNC_REMOVE_ENTITY = 10001;
        public const int SYNC_MOVE = 20001;
        public const int SYNC_POSITION = 1;
    }
}
