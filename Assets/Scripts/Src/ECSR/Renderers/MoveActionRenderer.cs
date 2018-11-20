using Components;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Renderers
{
    public class MoveActionRenderer:ActionRenderer
    {
        Tweener t = null;
        protected override void OnRender()
        {
            base.OnRender();
            var sim = GetSimulation("client");
            TransformComponent com_Pos = m_Entity.GetComponent<TransformComponent>();
            MoveComponent com_Move = m_Entity.GetComponent<MoveComponent>();
            if (com_Pos != null)
            {
                double lerp = sim.GetFrameLerp() * sim.GetFrameMsLength() / 1000 / Time.deltaTime;
                var pos1 = com_Pos.GetPositionVector2();
                var dir = com_Move.GetDirVector2();
                var nextPos = pos1 + dir * (com_Move.GetSpeed() * (float)(Time.deltaTime / sim.GetFrameMsLength() / 1000));

                if (t != null)
                {
                    t.Kill();
                    t = null;
                }             
                t = transform.DOLocalMove(Vector2.Lerp(pos1, nextPos, (float)lerp), 1, false);
            }
        }
    }
}
