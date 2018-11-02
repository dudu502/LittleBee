using BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicFrameSync.Src.LockStep.Behaviours
{
    /// <summary>
    /// behaviour_tree nodes input
    /// </summary>
    public class BTInputBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim
        {
            set;get;
        }

        List<BTRoot> m_BTRoots = new List<BTRoot>();

        public void Quit()
        {
            
        }

        public void Start()
        {

        }

        public void Update()
        {
            
            foreach(BTRoot root in m_BTRoots)
            {
                root.Update((int)Sim.GetFrameMsLength());
            }
        }
    }
}
