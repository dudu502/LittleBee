
using Managers;
using Net.Pt;
using NetServiceImpl;
using Src.Replays;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicFrameSync.Src.LockStep.Behaviours
{
    public class LogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { set; get; }
        public int CurrentFrameIdx { private set; get; }
        NetworkRoomModule networkRoomModule;
        List<List<FrameIdxInfo>> m_FrameIdxInfos;

        private readonly List<FrameIdxInfo> m_DefaultEmptyFrameIdxInfos = new List<FrameIdxInfo>();
        public LogicFrameBehaviour()
        {
            
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
            m_FrameIdxInfos = new List<List<FrameIdxInfo>>();
            networkRoomModule = ModuleManager.GetModule<NetworkRoomModule>();
            CurrentFrameIdx = networkRoomModule.Session.InitIndex;
            Debug.Log("CurrentFrameIndex "+CurrentFrameIdx);
            if(CurrentFrameIdx > -1)
            {
                for(int i=0;i<=CurrentFrameIdx;++i)
                {
                    m_FrameIdxInfos.Add(m_DefaultEmptyFrameIdxInfos);
                    if(networkRoomModule.Session.DictKeyframeCollection!=null && networkRoomModule.Session.DictKeyframeCollection.TryGetValue(i,out PtKeyFrameCollection ptKeyFrameCollection))
                    {
                        UpdateKeyFrameIdxInfoCollectionAtFrameIdx(ptKeyFrameCollection);
                    }
                }
            }
            Debug.LogWarning(0);
        }
        public void UpdateKeyFrameIdxInfoCollectionAtFrameIdx(PtKeyFrameCollection keyFrameCollection)
        {
            foreach (FrameIdxInfo info in keyFrameCollection.KeyFrames)
                UpdateKeyFrameIdxInfoAtFrameIdx(keyFrameCollection.FrameIdx, info);
        }
        public void UpdateKeyFrameIdxInfoAtFrameIdx(int frameIdx,FrameIdxInfo info)
        {
            if (frameIdx >= m_FrameIdxInfos.Count)
                throw new Exception("Error "+frameIdx);
            if (m_FrameIdxInfos[frameIdx] == m_DefaultEmptyFrameIdxInfos)
                m_FrameIdxInfos[frameIdx] = new List<FrameIdxInfo>();
            List<FrameIdxInfo> frames = m_FrameIdxInfos[frameIdx];
            bool updateState = false;
            foreach (FrameIdxInfo keyframe in frames)
            {
                if (keyframe.EqualsInfo(info))
                {
                    updateState = true;
                    keyframe.ParamsContent = info.ParamsContent;
                    break;
                }  
            }
            if (!updateState)
                frames.Add(info);
            //frames.Sort((a, b) => a.EntityId.CompareTo(b.EntityId));
        }
        public void Update() 
        {
            ++CurrentFrameIdx;
            m_FrameIdxInfos.Add(m_DefaultEmptyFrameIdxInfos);
            #region re-join
            if (networkRoomModule.Session.DictKeyframeCollection != null && networkRoomModule.Session.WriteKeyframeCollectionIndex > CurrentFrameIdx)
            {
                if (networkRoomModule.Session.DictKeyframeCollection.TryGetValue(CurrentFrameIdx, out PtKeyFrameCollection keyframeCollection))
                {
                    Sim.GetEntityWorld().RestoreKeyframes(keyframeCollection);
                    UpdateKeyFrameIdxInfoCollectionAtFrameIdx(keyframeCollection);
                }
            }
            else
            {
                networkRoomModule.Session.WriteKeyframeCollectionIndex = -1;
                networkRoomModule.Session.DictKeyframeCollection = null;
            }
            #endregion
        }
    }
}
