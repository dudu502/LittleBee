using LogicFrameSync.Src.LockStep.Net.Pt;
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
        public static void AddFrameCommand(FrameIdxInfo info)
        {
            KeyFrameCollection.FrameIdx = info.Idx;
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
