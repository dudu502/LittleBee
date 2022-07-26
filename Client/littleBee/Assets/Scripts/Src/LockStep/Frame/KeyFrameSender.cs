using Net.Pt;
using System;
using System.Collections.Generic;
namespace LogicFrameSync.Src.LockStep.Frame
{
    /// <summary>
    ///  关键帧同步记录
    /// </summary>
    public class KeyFrameSender
    {
        private static PtKeyFrameCollection KeyFrameCollection = new PtKeyFrameCollection() { KeyFrames = new List<FrameIdxInfo>()};
        public static void AddCurrentFrameCommand(int currentFrameIdx,ushort cmd,uint entityId,byte[] paramsContent)
        {
            FrameIdxInfo info = new FrameIdxInfo(cmd,entityId, paramsContent);

            KeyFrameCollection.FrameIdx = currentFrameIdx;
            KeyFrameCollection.KeyFrames.Add(info);
        }
        public static PtKeyFrameCollection GetFrameCommand()
        {
            return KeyFrameCollection;
        }
        public static void ClearFrameCommand()
        {
            KeyFrameCollection.FrameIdx = 0;
            KeyFrameCollection.KeyFrames.Clear();
        }
    }
}
