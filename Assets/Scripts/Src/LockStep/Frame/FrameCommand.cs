namespace LogicFrameSync.Src.LockStep.Frame
{
    /// <summary>
    /// 关键帧同步命令类型
    /// 绑定FrameIdxInfo.Cmd字段
    /// </summary>
    public class FrameCommand
    {
        public const int SYNC_MOVE = 0;
        public const int SYNC_POSITION = 1;
    }
}
