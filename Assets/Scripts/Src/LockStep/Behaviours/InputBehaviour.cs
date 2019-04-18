using Components;
using Entitas;
using LogicFrameSync.Src.LockStep.Frame;
using NetServiceImpl.Client.Data;
using UnityEngine;

namespace LogicFrameSync.Src.LockStep.Behaviours
{
    public class InputBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { set; get; }
        public void Quit()
        {

        }

        public void Start()
        {

        }
    
        public void Update()
        {
            LogicFrameBehaviour logic = Sim.GetBehaviour<LogicFrameBehaviour>();
            Entitas.Entity entity = Sim.GetEntityWorld().GetEntity(GameClientData.SelfControlEntityId);
            if (entity != null) 
            {
                MoveComponent comp = entity.GetComponent<MoveComponent>();
                if (comp != null)
                {
                    Vector2 dir = comp.GetDirVector2();
                    if (InputManager.Instance.IsKeyCodeActive(KeyCode.A))
                    {
                        dir.x = -1;
                    }
                    if (InputManager.Instance.IsKeyCodeActive(KeyCode.S))
                    {
                        dir.y = -1;
                    }
                    if (InputManager.Instance.IsKeyCodeActive(KeyCode.W))
                    {
                        dir.y = 1;
                    }
                    if (InputManager.Instance.IsKeyCodeActive(KeyCode.D))
                    {
                        dir.x = 1;
                    }
                    if (!InputManager.Instance.IsAnyKeyCodeActive())
                    {
                        dir.x = 0;
                        dir.y = 0;
                    }
                    if (dir != comp.GetDirVector2())
                    {
                        Debug.Log(string.Format("keyframeIdx:{0} roleid:{1}", logic.CurrentFrameIdx, comp.EntityId));

                        //KeyFrameSender.AddFrameCommand(new FrameIdxInfo(logic.CurrentFrameIdx,FrameCommand.SYNC_MOVE,comp.EntityId,new string[] { dir.x + "", dir.y + "", "0" }));

                        KeyFrameSender.AddCurrentFrameCommand(FrameCommand.SYNC_MOVE, comp.EntityId.ToString(), new string[] { dir.x + "", dir.y + "", "0" });
                    }   
                    
                    //bullet
                    if(InputManager.Instance.IsKeyCodeActive(KeyCode.Space))
                    {
                        KeyFrameSender.AddCurrentFrameCommand(FrameCommand.SYNC_CREATE_ENTITY, Common.Utils.GuidToString(), new string[] { ((int)EntityWorld.EntityOperationEvent.CreateBullet) + "" ,comp.EntityId.ToString()});
                    }
                }
            }
        }
    }
}
