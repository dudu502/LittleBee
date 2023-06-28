using System;
using System.Collections.Generic;
using System.Text;

namespace Synchronize.Game.Lockstep.Frame
{
    public class FrameCommand
    {
        /// <summary>
         /// 同步创建实体
         /// </summary>
        public const ushort SYNC_CREATE_ENTITY = 1;

        /// <summary>
        /// 同步移除实体
        /// </summary>
        public const ushort SYNC_REMOVE_ENTITY = 0;

        /// <summary>
        /// 同步移动
        /// </summary>
        public const ushort SYNC_MOVE = 20001;

        public const ushort SYNC_FIRE = 20002;
    }
}
