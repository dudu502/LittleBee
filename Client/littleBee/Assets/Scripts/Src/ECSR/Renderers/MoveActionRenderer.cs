using Synchronize.Game.Lockstep.Managers;
using TrueSync;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Ecsr.Renderer
{
    /// <summary>
    /// 移动行为
    /// 视图显示器
    /// </summary>
    public class MoveActionRenderer:ActionRenderer
    {
        const string COLOR = "_Color";
        protected override void OnRender()
        {
            Components.Common.Transform2D com_Pos = m_Simulation.GetEntityWorld().GetComponentByEntityId<Components.Common.Transform2D>(EntityId);
            Components.Common.Movement2D com_Move = m_Simulation.GetEntityWorld().GetComponentByEntityId<Components.Common.Movement2D>(EntityId);
            if (com_Pos != null)
            {
                //var dir = com_Move.Dir;
                var dir = com_Pos.Toward;
                var pos1 = com_Pos.Position;
                var nextPos = pos1 + dir * (com_Move.Speed * (Time.deltaTime / SimulationManager.Instance.GetFrameMsLength() / 1000));

                transform.position = Vector3.Lerp(transform.position, new Vector3(nextPos.x.AsFloat(), 0, nextPos.y.AsFloat()),0.25f);
                if (dir != TSVector2.zero)
                {
                    transform.forward = Vector3.Lerp(transform.forward, new Vector3(dir.x.AsFloat(),0, dir.y.AsFloat()), 0.25f);
                }

                //todo GC(40B)
                if (com_Pos.CollisionEntityId > 0)
                {
                    GetComponent<MeshRenderer>().materials[0].SetColor(COLOR, Color.red);
                }
                else
                {
                    GetComponent<MeshRenderer>().materials[0].SetColor(COLOR, Color.white);
                }
            }
            else
            {
                ModuleManager.GetModule<PoolModule>().Recycle(GetComponent<PoolObject>().GetFullName(), gameObject);
            }
        }       
    }
}
