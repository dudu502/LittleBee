
using Synchronize.Game.Lockstep.Frame;
using TrueSync;


namespace Synchronize.Game.Lockstep.Behaviours.Data
{
    public class JoystickMovementInputRecord : IInputRecord
    {
        public uint EntityId { set; get; }
        public FP x { set; get; }
        public FP y { set; get; }
        public JoystickMovementInputRecord()
        {

        }
        public void CopyFrom(IInputRecord record)
        {
            JoystickMovementInputRecord moveRecord = record as JoystickMovementInputRecord;
            if (moveRecord != null)
            {
                EntityId = moveRecord.EntityId;
                x = moveRecord.x;
                y = moveRecord.y;
            }
        }


        public int GetRecordType()
        {
            return FrameCommand.SYNC_MOVE;
        }

        public bool IsDirty(IInputRecord record)
        {
            JoystickMovementInputRecord moveRecord = record as JoystickMovementInputRecord;
            if (moveRecord == null) return false;
            if (moveRecord.EntityId != EntityId) return true;
            return (moveRecord.x != x || moveRecord.y != y);
        }
    }
}
