using LogicFrameSync.Src.LockStep.Net.Pt;
using NetServiceImpl;
using NetServiceImpl.Client;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LogicFrameSync.Src.LockStep.Behaviours
{
    public class RollbackBehaviour : EntityBehaviour
    {

        public string DebugFrameIdx;
        public RollbackBehaviour() 
        {
            keyframe = new PtKeyFrameCollection();
            keyframe.KeyFrames = new List<Frame.FrameIdxInfo>();
            keyframe.FrameIdx = -1;
        }
        LogicFrameBehaviour logicBehaviour;
        ComponentsBackupBehaviour backupBehaviour;
        PtKeyFrameCollection keyframe;
        public override void Update()
        {
            logicBehaviour = Sim.GetBehaviour<LogicFrameBehaviour>();
            backupBehaviour = Sim.GetBehaviour<ComponentsBackupBehaviour>();
            while (Service.Get<LoginService>().QueueKeyFrameCollection.Count > 0)
            {
                PtKeyFrameCollection pt = null;
                if (Service.Get<LoginService>().QueueKeyFrameCollection.TryPeek(out pt) && pt.FrameIdx < logicBehaviour.CurrentFrameIdx)
                {
                    PtKeyFrameCollection keyframeCollection = null;
                    if (Service.Get<LoginService>().QueueKeyFrameCollection.TryDequeue(out keyframeCollection))
                        Roll(keyframeCollection);
                    else
                        break;
                    DebugFrameIdx = string.Format("{0} CollectionFrameIdx:{1}", logicBehaviour.CurrentFrameIdx, keyframeCollection.FrameIdx);
                }
                else
                {
                    break;
                }
            }
        }
        void Roll(PtKeyFrameCollection collection)
        {
            if (keyframe.FrameIdx != collection.FrameIdx)
            {
                RollImpl(keyframe);    
                keyframe.KeyFrames = collection.KeyFrames;
                keyframe.FrameIdx = collection.FrameIdx;
            }
            else
            {
                keyframe.MergeKeyFrames(collection);
            }         
        }  

        void RollImpl(PtKeyFrameCollection collection)
        {
            if (collection.FrameIdx == -1) return;
            int frameIdx = collection.FrameIdx;
            EntityWorldFrameData frameData = backupBehaviour.GetEntityWorldFrameByFrameIdx(frameIdx);
            if (frameData != null)
            {
                foreach (var frame in collection.KeyFrames)
                    logicBehaviour.UpdateKeyFrameIdxInfoAtFrameIdx(frameIdx, frame);
                Sim.GetEntityWorld().RollBack(frameData, collection);

                while (frameIdx < logicBehaviour.CurrentFrameIdx)
                {                  
                    base.Update();
                    backupBehaviour.SetEntityWorldFrameByFrameIdx(frameIdx, new EntityWorldFrameData(
                       Sim.GetEntityWorld().FindAllEntitiesIds(), Sim.GetEntityWorld().FindAllCloneComponents()));
                    ++frameIdx;
                }
            }
        }
    }
}
