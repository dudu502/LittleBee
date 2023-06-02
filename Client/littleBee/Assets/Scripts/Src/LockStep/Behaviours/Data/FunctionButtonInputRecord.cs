
using Synchronize.Game.Lockstep.Frame;

namespace Synchronize.Game.Lockstep.Behaviours.Data
{
    public class FunctionButtonInputRecord : IInputRecord
    {

        public uint EntityId { set; get; }
        public FunctionButtonInputManager.Function Func { set; get; }
        public FunctionButtonInputRecord()
        {
            Func = FunctionButtonInputManager.Function.None;
        }
        public void CopyFrom(IInputRecord record)
        {
            FunctionButtonInputRecord funcRecord = record as FunctionButtonInputRecord;
            if(funcRecord!=null)
            {
                EntityId = funcRecord.EntityId;
                Func = funcRecord.Func;
            }
        }

        public int GetRecordType()
        {
            return FrameCommand.SYNC_CREATE_ENTITY;
        }

        public bool IsDirty(IInputRecord record)
        {
            FunctionButtonInputRecord funcRecord = record as FunctionButtonInputRecord;
            if (funcRecord == null) return false;
            return funcRecord.EntityId == EntityId
                 && funcRecord.Func != Func;
        }
    }
}
