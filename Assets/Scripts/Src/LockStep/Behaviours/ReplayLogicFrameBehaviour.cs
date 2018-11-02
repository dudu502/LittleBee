using LogicFrameSync.Src.LockStep.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicFrameSync.Src.LockStep.Behaviours
{
    public class ReplayLogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim
        {
            get;set;
        }
        public int CurrentFrameIdx { private set; get; }
        List<List<FrameIdxInfo>> m_FrameIdxInfos;
        public void Quit()
        {
            
        }
        public void SetFrameIdxInfos(List<List<FrameIdxInfo>> infos)
        {
            m_FrameIdxInfos = infos;
        }
        public List<FrameIdxInfo> GetFrameIdxInfoAtCurrentFrame()
        {
            return m_FrameIdxInfos[CurrentFrameIdx];
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
