
using Synchronize.Game.Lockstep.RoomServer.Modules;
using Synchronize.Game.Lockstep.Service.Modules;
using System.Collections.Generic;

namespace Synchronize.Game.Lockstep.RoomServer.Services.Sim
{
    public class ServerLogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim
        {
            set; get;
        }
        
        private int m_CurrentFrameIdx;
        private BattleModule battleModule = null;
        public void Quit()
        {

        }
        public void Start()
        {
            battleModule = BaseModule.GetModule<BattleModule>();
            m_CurrentFrameIdx = -1;
        }

        public void Update()
        {
            // flush the syncframe_data to all clients at same frame.
            battleModule.FlushKeyFrame(++m_CurrentFrameIdx);
        }
    }
}
