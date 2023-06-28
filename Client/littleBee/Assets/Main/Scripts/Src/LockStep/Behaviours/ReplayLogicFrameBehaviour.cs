using Net.Pt;
using System;
using System.Collections.Generic;


namespace Synchronize.Game.Lockstep.Behaviours
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
            if(CurrentFrameIdx<m_FrameIdxInfos.Count)
                return m_FrameIdxInfos[CurrentFrameIdx];
            return null;
        }
        public void Start()
        {
            CurrentFrameIdx = -1;
        }

        public void Update()
        {
            ++CurrentFrameIdx;
            Evt.EventMgr<EvtReplay,float>.TriggerEvent(EvtReplay.UpdateFrameCount,1f*(1+CurrentFrameIdx)/m_FrameIdxInfos.Count);
        }
    }
}
