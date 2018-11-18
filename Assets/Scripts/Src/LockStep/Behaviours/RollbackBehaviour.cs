using LogicFrameSync.Src.LockStep.Frame;
using LogicFrameSync.Src.LockStep.Net.Pt;
using NetServiceImpl;
using NetServiceImpl.Client;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LogicFrameSync.Src.LockStep.Behaviours
{
    /// <summary>
    /// 回滚关键帧信息
    /// </summary>
    public class RollbackBehaviour : EntityBehaviour
    {

        public string DebugFrameIdx;
        public RollbackBehaviour() 
        {
            keyframes = new List<PtKeyFrameCollection>();
        }
        LogicFrameBehaviour logicBehaviour;
        ComponentsBackupBehaviour backupBehaviour;
        List<PtKeyFrameCollection> keyframes;
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

            foreach(var keys in keyframes)
                RollImpl(keys);
            keyframes.Clear();
        }
        void Roll(PtKeyFrameCollection collection)
        {
            bool hasExist = false;
            foreach (PtKeyFrameCollection keys in keyframes)
            {
                if (keys.FrameIdx == collection.FrameIdx)
                {
                    hasExist = true;
                    keys.MergeKeyFrames(collection);
                }
            }
            if (!hasExist)
            {
                keyframes.Add(collection);
            }
            keyframes.Sort((a, b) => a.FrameIdx - b.FrameIdx);
        }
       
        void RollImpl(PtKeyFrameCollection collection)
        {
            if (collection.FrameIdx == -1) return;
            int frameIdx = collection.FrameIdx;
            collection.KeyFrames.Sort((a,b)=>new System.Guid(a.EntityId).CompareTo(new System.Guid(b.EntityId)));
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
