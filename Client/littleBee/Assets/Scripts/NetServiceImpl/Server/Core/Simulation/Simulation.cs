using System.Collections.Generic;

namespace Synchronize.Game.Lockstep.RoomServer.Services.Sim
{
    /// <summary>
    /// 模拟器
    /// 可以存在多个Id
    /// </summary>
    public class Simulation
    {           
        List<ISimulativeBehaviour> m_Behaviours;
        byte m_SimulationId;
        public Simulation(byte id)
        {
            m_SimulationId = id;
            m_Behaviours = new List<ISimulativeBehaviour>();            
        }

        public byte GetSimulationId() { return m_SimulationId; }
        public void Start()
        {
            foreach (ISimulativeBehaviour beh in m_Behaviours)
                beh.Start();
        }
        public T GetBehaviour<T>()where T:ISimulativeBehaviour
        {
            foreach(ISimulativeBehaviour beh in m_Behaviours)
            {
                if (beh.GetType() == typeof(T)) return (T)beh;
            }
            return default(T);
        }
        public bool ContainBehaviour(ISimulativeBehaviour beh)
        {
            foreach(ISimulativeBehaviour item in m_Behaviours)
            {
                if (item == beh) return true;
                if (item.GetType() == beh.GetType()) return true;
            }
            return false;
        }
        public void AddBehaviour(ISimulativeBehaviour beh)
        {
            if (!ContainBehaviour(beh))
            {
                m_Behaviours.Add(beh);
                beh.Sim = this;
            }                
        }
        public void RemoveBehaviour(ISimulativeBehaviour beh)
        {
            if (ContainBehaviour(beh))
            {
                m_Behaviours.Remove(beh);
                beh.Sim = null;
            }              
        }

        
        public void Run()
        {    
            for (int i = 0; i < m_Behaviours.Count; ++i)
                m_Behaviours[i].Update();          
        } 
    }
}

