using UnityEngine;
using Entitas;
using LogicFrameSync.Src.LockStep;
using System;

namespace Renderers
{
    /// <summary>
    /// 动作变化渲染
    /// 由Component数据驱动显示
    /// </summary>
    public class ActionRenderer:MonoBehaviour
    {
        protected string m_EntityId="";
        protected Simulation m_Simulation;
        private void Awake()
        {
            
        }

        private void Start()
        {
            m_Simulation = SimulationManager.Instance.GetSimulation(Const.CLIENT_SIMULATION_ID);
        }
        public void SetEntityId(string entityId)
        {
            m_EntityId = entityId;
        }
        
        private void Update()
        {
            if (string.IsNullOrEmpty(m_EntityId)) return;
            Entity entity = m_Simulation.GetEntityWorld().GetEntity(new Guid(m_EntityId));
            if (entity == null) return;
            if (!entity.IsActive) return;
            lock (EntityWorld.SyncRoot)
            {
                OnRender(entity);
            }                      
        }

        protected virtual void OnRender(Entity entity)
        {

        }
    }
}
