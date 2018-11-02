using Entitas;
using LogicFrameSync.Src.LockStep.Behaviours;
using LogicFrameSync.Src.LockStep.Frame;
using Src.Replays;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;


namespace LogicFrameSync.Src.LockStep
{
    public class Simulation
    {           
        EntityWorld m_EntityWorld;
        Stopwatch m_StopWatch;
        List<ISimulativeBehaviour> m_Behaviours;
        double m_Accumulator = 0;
        bool m_Running = false;
        double m_FrameMsLength = 100;
        string m_SimulationName;
        double m_FrameLerp = 0;
        public Simulation(string name)
        {
            m_SimulationName = name;
            m_StopWatch = new Stopwatch();
            m_Behaviours = new List<ISimulativeBehaviour>();
            m_EntityWorld = EntityWorld.Create();
        }
        public double GetFrameMsLength() { return m_FrameMsLength; }
        public double GetFrameLerp() { return m_FrameLerp; }
        public EntityWorld GetEntityWorld() { return m_EntityWorld; }
        public string GetSimulationName() { return m_SimulationName; }
        public void Start()
        {
            m_Running = true;
            m_StopWatch.Restart();
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
        public double GetElapsedTime()
        {
            double time = m_StopWatch.Elapsed.TotalMilliseconds;
            m_StopWatch.Restart();
            if (time > m_FrameMsLength)
                time = m_FrameMsLength;
            return time;
        }
        public void Stop()
        {
            m_Running = false;
        }
        public void Run()
        {
            if (m_Running)
            {
                m_Accumulator += GetElapsedTime();
                while (m_Accumulator >= m_FrameMsLength)
                {
                    Update();
                    m_Accumulator -= m_FrameMsLength;
                }
                m_FrameLerp = m_Accumulator / m_FrameMsLength;
            }
        }
        
        private void Update()
        {
            foreach (ISimulativeBehaviour beh in m_Behaviours)
                beh.Update();
        }

 
    }
}

