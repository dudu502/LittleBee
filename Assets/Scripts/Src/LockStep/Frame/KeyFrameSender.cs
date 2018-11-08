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
        private static object SyncRoot = new object();
        private static PtKeyFrameCollection KeyFrameCollection = new PtKeyFrameCollection() { KeyFrames = new List<FrameIdxInfo>()};
        public static void AddFrameCommand(FrameIdxInfo info)
        {
            lock(SyncRoot)
            {
                KeyFrameCollection.FrameIdx = info.Idx;
                KeyFrameCollection.KeyFrames.Add(info);
            }       
        }
        public static void AddCurrentFrameCommand(int cmd,string entityId,string[] paramsString)
        {
            Simulation sim = SimulationManager.Instance.GetSimulation("client");
            if (sim != null)
            {
                var logic = sim.GetBehaviour<LogicFrameSync.Src.LockStep.Behaviours.LogicFrameBehaviour>();
                if (logic != null)
                {
                    FrameIdxInfo info = new FrameIdxInfo(logic.CurrentFrameIdx,cmd,entityId,paramsString);
                    AddFrameCommand(info);
                }
            }
        }
        public static PtKeyFrameCollection GetFrameCommand()
        {
            return KeyFrameCollection;
        }
        public static void ClearFrameCommand()
        {
            lock (SyncRoot)
            {
                KeyFrameCollection.FrameIdx = 0;
                KeyFrameCollection.KeyFrames.Clear();
            }
        }
    }
}
