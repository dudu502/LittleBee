using System;
using System.Collections.Generic;

using System.Threading;


namespace LogicFrameSync.Src.LockStep
{
    public class SimulationManager
    {
        static SimulationManager ins;
        public static SimulationManager Instance
        {
            get{
                if (ins == null) ins = new SimulationManager();
                return ins;
            }         
        }
        private Thread m_Thread;
        private List<Simulation> m_Sims;

        private SimulationManager()
        {
            m_Sims = new List<Simulation>();
            
        }
        public void Start()
        {
            foreach (Simulation sim in m_Sims)
                sim.Start();
            m_Thread = new Thread(Run);
            m_Thread.Priority = ThreadPriority.Highest;
            m_Thread.IsBackground = true;
            m_Thread.Start();

            //ThreadPool.QueueUserWorkItem(ThreadPoolRunner);
        }
        public void Stop()
        {
            if (m_Thread != null)
            {
                m_Thread.Abort();
                m_Thread = null;
                foreach (Simulation sim in m_Sims)
                    sim.Stop();
            }
        }
        void ThreadPoolRunner(object state)
        {
            Run();
        }
        bool m_StopState = false;
        void Run()
        {
            while(!m_StopState)
            {
                foreach (Simulation sim in m_Sims)
                {
                    sim.Run();
                }
                Thread.Sleep(25);
            }
            
        }
        public void AddSimulation(Simulation sim)
        {
            if (!ContainSimulation(sim))
                m_Sims.Add(sim);
        }
        public void RemoveSimulation(Simulation sim)
        {
            if (ContainSimulation(sim))
                m_Sims.Remove(sim);
        }
        public void RemoveSimulation(string simulationName)
        {
            Simulation sim = GetSimulation(simulationName);
            if(sim!=null)
                RemoveSimulation(sim);
        }
        public bool ContainSimulation(Simulation sim)
        {
            return m_Sims.Contains(sim);
        }
        public Simulation GetSimulation(string simulationName)
        {
            foreach (Simulation sim in m_Sims)
                if (sim.GetSimulationName() == simulationName) return sim;
            return null;
        }
    }
}
