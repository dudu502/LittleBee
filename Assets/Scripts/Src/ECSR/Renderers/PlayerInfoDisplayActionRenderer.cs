using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Components;
namespace Renderers
{
    public class PlayerInfoDisplayActionRenderer : ActionRenderer
    {
        public Text m_Txt;
        protected override void OnRender(Entitas.Entity entity)
        {
            base.OnRender(entity);

            PlayerInfoComponent com = entity.GetComponent<PlayerInfoComponent>();
            if (com != null)
            {
                m_Txt.text = com.Value.ToString();
            }
        }
    }
}