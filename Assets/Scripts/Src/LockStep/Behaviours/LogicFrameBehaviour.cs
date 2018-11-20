using LogicFrameSync.Src.LockStep.Frame;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicFrameSync.Src.LockStep.Behaviours
{
    public class LogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { set; get; }
        public int CurrentFrameIdx { private set; get; }

        List<List<FrameIdxInfo>> m_FrameIdxInfos;

        public LogicFrameBehaviour()
        {
            m_FrameIdxInfos = new List<List<FrameIdxInfo>>();
        }
        public List<List<FrameIdxInfo>> GetFrameIdxInfos()
        {
            return m_FrameIdxInfos;
        }
        public void Quit()
        {
            
        }
        public void Start()
        {
            CurrentFrameIdx = -1;           
        }
        public void UpdateKeyFrameIdxInfoAtFrameIdx(int frameIdx,FrameIdxInfo info)
        {
            info.Idx = frameIdx;
            List<FrameIdxInfo> frames = m_FrameIdxInfos[frameIdx];
            bool updateState = false;
            foreach (FrameIdxInfo keyframe in frames)
            {
                if (keyframe.EqualsInfo(info))
                {
                    updateState = true;
                    keyframe.Params = info.Params;
                    break;
                }                    
            }
            if (!updateState)
                frames.Add(info);
        }
        public List<FrameIdxInfo> GetCurrentFrameIdxInfos()
        {
            return m_FrameIdxInfos[CurrentFrameIdx];
        }
        public void Update() 
        {
            ++CurrentFrameIdx;
            if (m_FrameIdxInfos.Count > CurrentFrameIdx)
                m_FrameIdxInfos[CurrentFrameIdx].Clear();
            else
                m_FrameIdxInfos.Add(new List<FrameIdxInfo>());
        }
    }
}
