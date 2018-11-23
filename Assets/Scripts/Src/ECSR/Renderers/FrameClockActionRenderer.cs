using UnityEngine;
using System.Collections;
using Components;
namespace Renderers
{
    public class FrameClockActionRenderer : ActionRenderer
    {
        public float m_Rate;
        protected override void OnRender()
        {
            base.OnRender();
            var sim = GetSimulation(Const.CLIENT_SIMULATION_ID);
            FrameClockComponent com = m_Entity.GetComponent<FrameClockComponent>();
            if (com != null)
            {
                m_Rate = com.GetRate();
            }
        }
    }
}