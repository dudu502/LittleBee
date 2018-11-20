using LogicFrameSync.Src.LockStep.Frame;
using LogicFrameSync.Src.LockStep.Net.Pt;
using NetServiceImpl;
using NetServiceImpl.Client;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

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
           
        }
        LogicFrameBehaviour logicBehaviour;
        ComponentsBackupBehaviour backupBehaviour;
        
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
                        RollImpl(keyframeCollection);
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

        void RollImpl(PtKeyFrameCollection collection)
        {
            int frameIdx = collection.FrameIdx;
            collection.KeyFrames.Sort((a, b) => new System.Guid(a.EntityId).CompareTo(new System.Guid(b.EntityId)));
            EntityWorldFrameData frameData = backupBehaviour.GetEntityWorldFrameByFrameIdx(frameIdx);
            if (frameData != null)
            {
                //replay frameidx cmd;
                foreach (var frame in collection.KeyFrames)
                    logicBehaviour.UpdateKeyFrameIdxInfoAtFrameIdx(collection.FrameIdx, frame);

                //roll back entityworld
                Sim.GetEntityWorld().RollBack(frameData, collection);

                //simulation entitybehaviours in entitysystem
                while (frameIdx < logicBehaviour.CurrentFrameIdx)
                {
                    base.Update();
                    backupBehaviour.SetEntityWorldFrameByFrameIdx(frameIdx, new EntityWorldFrameData(Sim.GetEntityWorld().FindAllEntitiesIds(), Sim.GetEntityWorld().FindAllCloneComponents()));
                    ++frameIdx;
                }
            }
        }
    }
}
