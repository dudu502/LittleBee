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
            PositionComponent com_Pos = m_Entity.GetComponent<PositionComponent>();
            if (com_Pos != null)
            {
                double lerp = sim.GetFrameLerp() * sim.GetFrameMsLength() / 1000 / Time.deltaTime;
                var pos1 = com_Pos.GetPosition();// new Vector2(pos.Pos.x,pos.Pos.y);
                var dir = m_Entity.GetComponent<MoveComponent>().GetDir();// new Vector2(Ent.GetComponent<MoveComponent>().Dir.x, Ent.GetComponent<MoveComponent>().Dir.y);
                var nextPos = pos1 + dir * (m_Entity.GetComponent<MoveComponent>().GetSpeed() * (float)(Time.deltaTime / sim.GetFrameMsLength() / 1000));

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
