using UnityEngine;
using Entitas;
using LogicFrameSync.Src.LockStep;

namespace Renderers
{
    /// <summary>
    /// 动作变化渲染
    /// 由Component数据驱动显示
    /// </summary>
    public class ActionRenderer:MonoBehaviour
    {
        protected Entity m_Entity;
        protected string m_EntityId="";
        private void Awake()
        {
            
        }

        private void Start()
        {
            
        }
        public void SetEntityId(string entityId)
        {
            m_EntityId = entityId;
        }
        protected Simulation GetSimulation(byte id)
        {
            return SimulationManager.Instance.GetSimulation(id);
        }

        private void Update()
        {
            if (string.IsNullOrEmpty(m_EntityId)) return;
            Simulation sim = GetSimulation(Const.CLIENT_SIMULATION_ID);
            if (sim == null) return;
            if (!sim.GetEntityWorld().IsActive) return;
            m_Entity = sim.GetEntityWorld().GetEntity(m_EntityId);
            if (m_Entity == null) return;
            if (!m_Entity.IsActive) return;
            OnRender();
        }

        protected virtual void OnRender()
        {

        }
    }
}
