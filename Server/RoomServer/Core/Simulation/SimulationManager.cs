using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Synchronize.Game.Lockstep.RoomServer.Services.Sim
{
    public class SimulationManager
    {
        static SimulationManager ins;
        Simulation m_SimulationInstance;
        long m_AccumulatorTicks = 0;
        const int m_FrameMsLength = 20;
        int m_FrameMsTickCount = m_FrameMsLength * 10000;
        double m_FrameLerp = 0;
        public int GetFrameMsLength() { return m_FrameMsLength; }
        public double GetFrameLerp() { return m_FrameLerp; }
        bool m_StopState = false;
        Thread m_Thread;
        DateTime m_CurrentDateTime;
        public static SimulationManager Instance
        {
            get
            {
                if (ins == null) ins = new SimulationManager();
                return ins;
            }
        }


        private SimulationManager()
        {

        }

        public void Start()
        {
            m_SimulationInstance.Start();
            m_Thread = new Thread(Run);
            m_Thread.IsBackground = true;
            m_Thread.Start();
        }

        public void Stop()
        {
            m_StopState = false;
        }

        void Run()
        {
            m_CurrentDateTime = DateTime.Now;
            while (!m_StopState)
            {
                DateTime Now = DateTime.Now;
                TimeSpan sp = (Now - m_CurrentDateTime);
                m_AccumulatorTicks += sp.Ticks;
                m_CurrentDateTime = Now;
                while (m_AccumulatorTicks >= m_FrameMsTickCount)
                {
                    m_SimulationInstance.Run();
                    m_AccumulatorTicks -= m_FrameMsTickCount;
                }
                m_FrameLerp = m_AccumulatorTicks / m_FrameMsTickCount;
                Thread.Sleep(30);
            }
        }
        public void SetSimulation(Simulation sim)
        {
            m_SimulationInstance= sim;
        }
        public void RemoveSimulation()
        {
            m_SimulationInstance = null;
        }
       
       
        public Simulation GetSimulation()
        {
            return m_SimulationInstance;
        }
    }
}
