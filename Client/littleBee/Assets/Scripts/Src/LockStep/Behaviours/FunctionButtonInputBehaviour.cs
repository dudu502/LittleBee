
using NetServiceImpl;
using NetServiceImpl.OnlineMode.Room;
using Synchronize.Game.Lockstep.Behaviours.Data;
using Synchronize.Game.Lockstep.Behaviours.Frame;
using Synchronize.Game.Lockstep.Frame;

namespace Synchronize.Game.Lockstep.Behaviours
{
    public class FunctionButtonInputBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { set; get; }
        uint Id;
        LogicFrameBehaviour logic;
        FunctionButtonInputRecord currentRecord;
        FunctionButtonInputRecord record;
        public void Quit()
        {
            
        }

        public void Start()
        {
            Id = ClientService.Get<RoomServices>().Session.Id;
            logic = Sim.GetBehaviour<LogicFrameBehaviour>();
            currentRecord = new FunctionButtonInputRecord();
            currentRecord.EntityId = Id;
            record = new FunctionButtonInputRecord();
            record.EntityId = Id;
        }

        public void Update()
        {
            if (Id > 0)
            {
                FunctionButtonInputManager.Function func = FunctionButtonInputManager.Instance.Func;
                if (func == FunctionButtonInputManager.Function.FIRE)
                {
                    FunctionButtonInputManager.Instance.Reset();
                    KeyFrameSender.AddCurrentFrameCommand(logic.CurrentFrameIdx, FrameCommand.SYNC_CREATE_ENTITY, Id, new ByteBuffer().WriteByte((byte)Misc.EntityType.Bullet).Getbuffer());
                }
            }
        }
    }
}
