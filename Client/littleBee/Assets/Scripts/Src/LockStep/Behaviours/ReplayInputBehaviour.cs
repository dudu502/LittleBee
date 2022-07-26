
using Components;
using Components.Common;
using Frame;
using LogicFrameSync.Src.LockStep.Frame;
using Net.Pt;
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
                            Entitas.EntityManager.CreateEntityBySyncFrame(Sim.GetEntityWorld(), info);
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
