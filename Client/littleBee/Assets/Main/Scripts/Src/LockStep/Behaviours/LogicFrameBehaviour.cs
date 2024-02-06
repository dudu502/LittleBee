
using Net.Pt;
using NetServiceImpl;
using Synchronize.Game.Lockstep.Proxy;
using Synchronize.Game.Lockstep.Room;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Behaviours
{
    public class LogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { set; get; }
        public int CurrentFrameIdx { private set; get; }

        List<List<FrameIdxInfo>> m_FrameIdxInfos;
        private RoomServiceProxy roomServices;
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
            roomServices = DataProxy.Get<RoomServiceProxy>();
            CurrentFrameIdx = roomServices.Session.InitIndex;
            Debug.Log("CurrentFrameIndex "+CurrentFrameIdx);
            if(CurrentFrameIdx > -1)
            {
                for (int i = 0; i <= CurrentFrameIdx; ++i)
                {
                    m_FrameIdxInfos.Add(m_DefaultEmptyFrameIdxInfos);
                    if (roomServices.Session.DictKeyframeCollection != null && roomServices.Session.DictKeyframeCollection.TryGetValue(i, out PtKeyFrameCollection ptKeyFrameCollection))
                    {
                        UpdateKeyFrameIdxInfoCollectionAtFrameIdx(ptKeyFrameCollection);
                    }
                }
            }
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
            if (roomServices.Session.DictKeyframeCollection != null && roomServices.Session.WriteKeyframeCollectionIndex > CurrentFrameIdx)
            {
                if (roomServices.Session.DictKeyframeCollection.TryGetValue(CurrentFrameIdx, out PtKeyFrameCollection keyframeCollection))
                {
                    Sim.GetEntityWorld().RestoreKeyframes(keyframeCollection);
                    UpdateKeyFrameIdxInfoCollectionAtFrameIdx(keyframeCollection);
                }
            }
            else
            {
                roomServices.Session.WriteKeyframeCollectionIndex = -1;
                roomServices.Session.DictKeyframeCollection = null;
            }
            #endregion
        }
    }
}
