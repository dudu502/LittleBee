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



                //Vector2 dirr = dir.normalized;
                //float target = Mathf.Atan2(dirr.y, dirr.x) * Mathf.Rad2Deg;
                ////transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, target), 2 * Time.deltaTime);
                //if(dirr!=Vector2.zero)
                //    transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, target+360), 1);
                //print(target);
            }
        }
    }
}
