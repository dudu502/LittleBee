using Components;
using LogicFrameSync.Src.LockStep.Frame;
using NetServiceImpl.Client.Data;
using UnityEngine;

namespace LogicFrameSync.Src.LockStep.Behaviours
{
    public class TestRandomInputBehaviour : ISimulativeBehaviour
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
        int randomStepFrame = 20;
        public void Update()
        {
            LogicFrameBehaviour logic = Sim.GetBehaviour<LogicFrameBehaviour>();            
            if(logic.CurrentFrameIdx % randomStepFrame == 0)
            {
                //randomStepFrame = new System.Random().Next(10, 30);
                Entitas.Entity entity = Sim.GetEntityWorld().GetEntity(GameClientData.SelfControlEntityId);
                if (entity != null)
                {
                    MoveComponent comp = entity.GetComponent<MoveComponent>();
                    if (comp != null)
                    {
                        Vector2 dir = comp.GetDirVector2();
                        int value = new System.Random().Next(0, 5);
                        if (value==0)
                        {
                            dir.x = -1;
                        }
                        else if (value==1)
                        {
                            dir.y = -1;
                        }
                        else if (value==2)
                        {
                            dir.y = 1;
                        }
                        else if (value ==3)
                        {
                            dir.x = 1;
                        }
                        else if (value==4)
                        {
                            dir.x = 0;
                            dir.y = 0;
                        }
                        if (dir != comp.GetDirVector2())
                        {
                            //Debug.Log(string.Format("keyframeIdx:{0} roleid:{1}", logic.CurrentFrameIdx, comp.EntityId));

                            KeyFrameSender.AddFrameCommand(new FrameIdxInfo(logic.CurrentFrameIdx, FrameCommand.SYNC_MOVE, comp.EntityId, new string[] { dir.x + "", dir.y + "", "0" }));
                        }
                    }
                }
            }
            
        }
    }
}
