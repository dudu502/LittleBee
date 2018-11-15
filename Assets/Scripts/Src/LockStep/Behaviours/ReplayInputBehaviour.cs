
using Components;
using LogicFrameSync.Src.LockStep.Frame;
using System.Collections.Generic;
using UnityEngine;
namespace LogicFrameSync.Src.LockStep.Behaviours
{
    /// <summary>
    /// Replay的输入
    /// </summary>
    public class ReplayInputBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim
        {
            set;get;
        }

        public void Quit()
        {
            
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            ReplayLogicFrameBehaviour replayLogic = Sim.GetBehaviour<ReplayLogicFrameBehaviour>();       
            List<FrameIdxInfo> list = replayLogic.GetFrameIdxInfoAtCurrentFrame();

            if (list != null)
            {
                foreach (FrameIdxInfo info in list)
                {
                    switch (info.Cmd)
                    {
                        case FrameCommand.SYNC_MOVE:
                            Entitas.Entity ent = Sim.GetEntityWorld().GetEntity(info.EntityId);
                            if (ent != null)
                            {
                                ent.GetComponent<MoveComponent>().UpdateParams(info.Params);
                                Debug.Log(string.Format("EntityId: {0} Dir:{1} {2} ", info.EntityId, float.Parse(info.Params[0]), float.Parse(info.Params[1])));
                            }
                            break;
                        case FrameCommand.SYNC_CREATE_ENTITY:
                            Sim.GetEntityWorld().NotifyCreateEntity(info);
                            break;
                    }
                }
            }            
        }
    }
}
