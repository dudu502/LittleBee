namespace LogicFrameSync.Src.LockStep.Frame
{
    /// <summary>
    /// 关键帧同步命令类型
    /// 绑定FrameIdxInfo.Cmd字段
    /// </summary>
    public class FrameCommand
    {
        /// <summary>
        /// 同步创建实体
        /// </summary>
        public const int SYNC_CREATE_ENTITY = 10000;

        /// <summary>
        /// 同步移除实体
        /// </summary>
        public const int SYNC_REMOVE_ENTITY = 10001;

        /// <summary>
        /// 同步移动
        /// </summary>
        public const int SYNC_MOVE = 20001;

        /// <summary>
        /// 同步Transform
        /// </summary>
        public const int SYNC_TRANSFORM = 1;
    }
}
