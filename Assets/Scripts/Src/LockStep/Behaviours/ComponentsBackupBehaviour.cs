
using System.Collections.Generic;
namespace LogicFrameSync.Src.LockStep.Behaviours
{
    
    public class ComponentsBackupBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim
        {
            set;get;
        }
        Dictionary<int, EntityWorldFrameData> m_DictEntityWorldFrameData;
        public ComponentsBackupBehaviour()
        {
            m_DictEntityWorldFrameData = new Dictionary<int, EntityWorldFrameData>();
        }
        public EntityWorldFrameData GetEntityWorldFrameByFrameIdx(int frameIdx)
        {
            if (m_DictEntityWorldFrameData.ContainsKey(frameIdx))
                return m_DictEntityWorldFrameData[frameIdx];
            return null;
        }
        public void SetEntityWorldFrameByFrameIdx(int frameIdx, EntityWorldFrameData data)
        {
            m_DictEntityWorldFrameData[frameIdx]= data; 
        }
        public Dictionary<int, EntityWorldFrameData> GetEntityWorldFrameData() { return m_DictEntityWorldFrameData; }

        public void Quit()
        {
            
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            var logic = Sim.GetBehaviour<LogicFrameBehaviour>();
            int frameIdx = logic.CurrentFrameIdx;

            SetEntityWorldFrameByFrameIdx(frameIdx, new EntityWorldFrameData(Sim.GetEntityWorld().FindAllEntitiesIds(),
                                                                             Sim.GetEntityWorld().FindAllCloneComponents()));
        }
    }
}
