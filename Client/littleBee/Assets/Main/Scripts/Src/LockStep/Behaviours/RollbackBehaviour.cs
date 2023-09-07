using Net.Pt;
using Synchronize.Game.Lockstep.Ecsr.Entitas;

namespace Synchronize.Game.Lockstep.Behaviours
{
    /// <summary>
    /// 回滚关键帧信息
    /// </summary>
    public class RollbackBehaviour : EntityBehaviour
    {
        public RollbackBehaviour() 
        {
           
        }
        ComponentsBackupBehaviour backupBehaviour;
        public override void Start()
        {
            base.Start();         
            backupBehaviour = Sim.GetBehaviour<ComponentsBackupBehaviour>();
        }
        public override void Update()
        {
            if (roomServices.Session.DictKeyframeCollection != null) return;
            while (roomServices.Session.QueueKeyFrameCollection.Count > 0)
            {
                bool rollState = false;
                if (roomServices.Session.QueueKeyFrameCollection.TryPeek(out PtKeyFrameCollection pt) && pt.FrameIdx < logicBehaviour.CurrentFrameIdx)
                {
                    if (roomServices.Session.QueueKeyFrameCollection.TryDequeue(out PtKeyFrameCollection keyframeCollection))
                        rollState = RollImpl(keyframeCollection);
                    else
                        break;
                }
                else
                {
                    break;
                }
                if (!rollState)
                {
                    break;
                }
            }
        }       

        /// <summary>
        /// 回滚关键帧数据
        /// </summary>
        /// <param name="collection"></param>
        bool RollImpl(PtKeyFrameCollection collection)
        {
            if (collection == null || collection.FrameIdx == 0) return false;
            int frameIdx = collection.FrameIdx;
            //回放命令存储;
            logicBehaviour.UpdateKeyFrameIdxInfoCollectionAtFrameIdx(collection);
            //从frameIdx-1数据中深度拷贝一份作为frameIdx的数据
            EntityWorldFrameData framePrevData = backupBehaviour.GetEntityWorldFrameByFrameIdx(frameIdx - 1);
            if(framePrevData!=null)
            {
                //回滚整个entityworld数据
                Sim.GetEntityWorld().RollBack(framePrevData, collection);
               
                //迅速从frameIdx开始模拟至当前客户端frameIdx
                while (frameIdx < logicBehaviour.CurrentFrameIdx)
                {
                    base.Update();                                     
                    backupBehaviour.SetEntityWorldFrameByFrameIdx(frameIdx++, new EntityWorldFrameData( Sim.GetEntityWorld().GetAllCloneComponents()));
                }
                return true;
            }
            else
            {
                //保存的数据太久了，已经被删除了。
                //提示网络太糟蹋建议重新进入
                return false;
            }
        }
    }
}
