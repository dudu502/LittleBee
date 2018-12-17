using UnityEngine;
using System.Collections;
using Components;
namespace Renderers
{
    public class FrameClockActionRenderer : ActionRenderer
    {
        public float m_Rate;
        protected override void OnRender(Entitas.Entity entity)
        {
            base.OnRender(entity);
            FrameClockComponent com = entity.GetComponent<FrameClockComponent>();
            if (com != null)
            {
                m_Rate = com.GetRate();
            }
        }
    }
}