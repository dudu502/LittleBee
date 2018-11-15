using NetServiceImpl;
using NetServiceImpl.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicFrameSync.Src.LockStep.Behaviours
{
    public class ServerLogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim
        {
            set;get;
        }
        public int CurrentFrameIdx { private set; get; }

        public void Quit()
        {
            
        }

        public void Start()
        {
            CurrentFrameIdx = -1;
        }

        public void Update()
        {
            ++CurrentFrameIdx;
        }
    }
}
