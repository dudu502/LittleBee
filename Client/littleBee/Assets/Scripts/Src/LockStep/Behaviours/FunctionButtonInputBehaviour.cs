using Frame;
using LogicFrameSync.Src.LockStep.Behaviours.Data;
using LogicFrameSync.Src.LockStep.Frame;
using NetServiceImpl;
using NetServiceImpl.OnlineMode.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicFrameSync.Src.LockStep.Behaviours
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
