
using Net.Pt;
using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using Synchronize.Game.Lockstep.Frame;
using System.Collections.Generic;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Behaviours
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
        ReplayLogicFrameBehaviour replayLogic;

        public void Start()
        {
            replayLogic = Sim.GetBehaviour<ReplayLogicFrameBehaviour>();
        }

        public void Update()
        {             
            List<FrameIdxInfo> list = replayLogic.GetFrameIdxInfoAtCurrentFrame();
            if (list != null)
            {
                foreach (FrameIdxInfo info in list)
                {
                    switch (info.Cmd)
                    {
                        case FrameCommand.SYNC_MOVE:
                            if (info.EntityId != 0)
                            {
                                Movement2D moveComponent = Sim.GetEntityWorld().GetComponentByEntityId<Movement2D>(info.EntityId);
                                if(moveComponent!=null)
                                    moveComponent.UpdateParams(info.ParamsContent);
                                //Debug.Log(string.Format("EntityId: {0} Dir:{1} {2} ", info.EntityId, float.Parse(info.Params[0]), float.Parse(info.Params[1])));
                            }
                            break;
                        case FrameCommand.SYNC_CREATE_ENTITY:
                            //Sim.GetEntityWorld().NotifyCreateEntity(info);
                            EntityManager.CreateEntityBySyncFrame(Sim.GetEntityWorld(), info);
                            break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Replay play complete....");
                BattleEntryPoint.StopReplay();
            }
        }
    }
}
