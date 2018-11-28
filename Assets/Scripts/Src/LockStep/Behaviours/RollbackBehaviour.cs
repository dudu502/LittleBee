using LogicFrameSync.Src.LockStep.Net.Pt;
using NetServiceImpl;
using NetServiceImpl.Client;

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

        /// <summary>
        /// 回滚关键帧数据
        /// </summary>
        /// <param name="collection"></param>
        void RollImpl(PtKeyFrameCollection collection)
        {
            int frameIdx = collection.FrameIdx;
            if (frameIdx < 1) return;

            collection.KeyFrames.Sort((a, b) => new System.Guid(a.EntityId).CompareTo(new System.Guid(b.EntityId)));
            //回放命令存储;
            foreach (var frame in collection.KeyFrames)
                logicBehaviour.UpdateKeyFrameIdxInfoAtFrameIdx(collection.FrameIdx, frame);

            //从frameIdx-1数据中深度拷贝一份作为frameIdx的数据
            EntityWorldFrameData framePrevData = backupBehaviour.GetEntityWorldFrameByFrameIdx(frameIdx - 1);
            EntityWorldFrameData frameData = framePrevData.Clone();
            if (frameData != null)
            {
                //回滚整个entityworld数据
                Sim.GetEntityWorld().RollBack(frameData, collection);
          
                //迅速从frameIdx开始模拟至当前客户端frameIdx
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
