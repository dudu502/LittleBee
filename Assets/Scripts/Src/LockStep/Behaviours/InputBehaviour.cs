
using Components;
using LogicFrameSync.Src.LockStep.Frame;
using NetServiceImpl;
using NetServiceImpl.Client;
using NetServiceImpl.Client.Data;

using System.Collections.Generic;
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
                    else if (InputManager.Instance.IsKeyCodeActive(KeyCode.S))
                    {
                        dir.y = -1;
                    }
                    else if (InputManager.Instance.IsKeyCodeActive(KeyCode.W))
                    {
                        dir.y = 1;
                    }
                    else if (InputManager.Instance.IsKeyCodeActive(KeyCode.D))
                    {
                        dir.x = 1;
                    }
                    else if (!InputManager.Instance.IsAnyKeyCodeActive())
                    {
                        dir.x = 0;
                        dir.y = 0;
                    }
                    if (dir != comp.GetDirVector2())
                    {
                        Debug.Log(string.Format("keyframeIdx:{0} roleid:{1}", logic.CurrentFrameIdx, comp.EntityId));

                        KeyFrameSender.AddFrameCommand(new FrameIdxInfo(logic.CurrentFrameIdx,FrameCommand.SYNC_MOVE,comp.EntityId,new string[] { dir.x + "", dir.y + "", "0" }));
                    }                    
                }
            }
        }
    }
}
