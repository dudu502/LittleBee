using UnityEngine;
using System;
using System.Threading;

namespace Synchronize.Game.Lockstep.Ecsr.Renderer
{
    /// <summary>
    /// 动作变化渲染
    /// 由Component数据驱动显示
    /// </summary>
    public class ActionRenderer:MonoBehaviour
    {
        public uint EntityId { set; get; }
        protected Simulation m_Simulation;
        private void Awake()
        {
            
        }

        private void Start()
        {
            OnInit();
        }
        protected virtual void OnInit()
        {
            TryFindSimulation();
        }
        public void SetEntityId(uint entityId)
        {
            EntityId = entityId;
        }
        
        private void LateUpdate()
        {
            OnRender();
        }
        protected void TryFindSimulation()
        {
            m_Simulation = SimulationManager.Instance.GetSimulation();
        }
        protected virtual void OnRender()
        {

        }
    }
}
