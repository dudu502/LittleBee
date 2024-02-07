using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace Synchronize.Game.Lockstep
{
    public class SimulationManager
    {
        static SimulationManager ins;
        Simulation m_SimulationInstance;
        long m_AccumulatorTicks = 0;
        const int c_DefaultFrameMsLength = 20;
        int m_FrameMsLength = c_DefaultFrameMsLength;
        int FrameMsTickCount { get { return m_FrameMsLength * 10000; } }
        double m_FrameLerp = 0;
        public int GetFrameMsLength() { return m_FrameMsLength; }
        public double GetFrameLerp() { return m_FrameLerp; }

        Thread m_Thread;
        DateTime m_CurrentDateTime;
        public bool NeedStop { private set; get; } = false;
        public bool IsRunning { private set; get; }
        public static SimulationManager Instance
        {
            get
            {
                if (ins == null) ins = new SimulationManager();
                return ins;
            }
        }
        public void UpdateFrameMsLength(float factor)
        {
            m_FrameMsLength = (int)(c_DefaultFrameMsLength / (factor + 0.5f));
        }
        private SimulationManager()
        {

        }

        public void Start(DateTime startDateTime,
                            int history_keyframes_count = 0,
                            Action<float> processCaller = null,
                            Action startRunningCaller = null)
        {
            m_CurrentDateTime = startDateTime;

            m_SimulationInstance.Start();
            for (int i = 0; i < history_keyframes_count; ++i)
            {
                m_SimulationInstance.Run();
                processCaller?.Invoke(1f * i / history_keyframes_count);
            }
            IsRunning = true;
            m_Thread = new Thread(Run);
            m_Thread.IsBackground = true;
            m_Thread.Priority = ThreadPriority.Highest;
            m_Thread.Start(startRunningCaller);
        }


        public void Stop()
        {
            NeedStop = true;
        }

        public void Run(object obj)
        {
            Action startRunningCaller = obj as Action;
            NeedStop = false;
            while (!NeedStop)
            {
                DateTime Now = DateTime.Now;
                m_AccumulatorTicks += (Now - m_CurrentDateTime).Ticks;
                m_CurrentDateTime = Now;
                while (m_AccumulatorTicks >= FrameMsTickCount)
                {
                    m_SimulationInstance.Run();
                    m_AccumulatorTicks -= FrameMsTickCount;
                }
                m_FrameLerp = m_AccumulatorTicks / FrameMsTickCount;
                Thread.Sleep(30);
                if (startRunningCaller != null)
                {
                    startRunningCaller();
                    startRunningCaller = null;
                }
            }
            IsRunning = false;
        } 

        public void SetSimulation(Simulation sim)
        {
            m_SimulationInstance = sim;
            m_FrameMsLength = c_DefaultFrameMsLength;
        }
        public void RemoveSimulation()
        {
            m_SimulationInstance.Dispose();
            m_SimulationInstance =null;
        }
        
        public Simulation GetSimulation()
        {
            return m_SimulationInstance;
        }
    }
}
