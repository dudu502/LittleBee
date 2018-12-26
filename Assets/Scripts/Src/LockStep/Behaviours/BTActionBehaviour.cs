using BehaviorTree;
using System;
using System.Collections.Generic;
namespace LogicFrameSync.Src.LockStep.Behaviours
{
    /// <summary>
    /// behaviour_tree nodes input
    /// </summary>
    public class BTActionBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim
        {
            set;get;
        }

        List<BTRoot> m_BTRoots = new List<BTRoot>();
        public void AddBtRoot(string Id)
        {
            BTRoot bt = new BTRoot();
            bt.Id = Id;
            m_BTRoots.Add(bt);
        }

        public void Quit()
        {

        }

        public void Start()
        {
            
        }

        public void Update()
        {
            foreach (BTRoot root in m_BTRoots)
            {
                root.Update((int)SimulationManager.Instance.GetFrameMsLength());
            }
        }
    }
}
