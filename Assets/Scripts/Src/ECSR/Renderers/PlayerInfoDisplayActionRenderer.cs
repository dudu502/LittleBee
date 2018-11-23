using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Components;
namespace Renderers
{
    public class PlayerInfoDisplayActionRenderer : ActionRenderer
    {
        public Text m_Txt;
        protected override void OnRender()
        {
            base.OnRender();
            var sim = GetSimulation(Const.CLIENT_SIMULATION_ID);
            PlayerInfoComponent com = m_Entity.GetComponent<PlayerInfoComponent>();
            if (com != null)
            {
                m_Txt.text = com.Value.ToString();
            }
        }
    }
}