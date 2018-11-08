
using Components;
using LogicFrameSync.Src.LockStep.Frame;
using System.Collections.Generic;
using UnityEngine;
namespace LogicFrameSync.Src.LockStep.Behaviours
{
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
            
            if(list!=null)
            {               
                foreach (FrameIdxInfo info in list)
                {
                    if (info.Cmd == FrameCommand.SYNC_MOVE)
                    {                        
                        Entitas.Entity ent= Sim.GetEntityWorld().GetEntity(info.EntityId);
                        if(ent != null)
                        {
                            ent.GetComponent<MoveComponent>().SetDir (new Vector2(
                                float.Parse(info.Params[0]), float.Parse(info.Params[1])
                                ));
                            Debug.Log(string.Format("EntityId: {0} Dir:{1} {2} ",info.EntityId, float.Parse(info.Params[0]), float.Parse(info.Params[1])));
                        }
                    }
                    else if(info.Cmd == FrameCommand.SYNC_CREATE_ENTITY)
                    {
                        Entitas.Entity ent = Sim.GetEntityWorld().AddEntity(info.EntityId);
                        if (ent != null)
                        {
                            ent.AddComponent(new MoveComponent(20, Vector2.zero)).AddComponent(new PositionComponent(Vector2.zero));
                            Notify.Notifier.Instance.Send(Entitas.EntityWorld.NotifierType.CreateEntity, info.EntityId);
                        }
                    }
                }
            }            
        }
    }
}
