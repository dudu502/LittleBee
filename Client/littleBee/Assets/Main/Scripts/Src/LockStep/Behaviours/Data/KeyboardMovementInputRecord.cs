
using Synchronize.Game.Lockstep.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Synchronize.Game.Lockstep.Behaviours.Data
{
    public class KeyboardMovementInputRecord : IInputRecord
    {
        public uint EntityId { set; get; }
        public float x {  set; get; }
        public float y {  set; get; }

        public KeyboardMovementInputRecord()
        {
           
        }

        public int GetRecordType()
        {
            return FrameCommand.SYNC_MOVE;
        }

        public bool IsDirty(IInputRecord record)
        {
            KeyboardMovementInputRecord moveRecord = record as KeyboardMovementInputRecord;
            if (moveRecord == null) return false;
            if (moveRecord.EntityId != EntityId) return true;
            return (moveRecord.x != x || moveRecord.y != y);
        }

        public void CopyFrom(IInputRecord record)
        {
            KeyboardMovementInputRecord moveRecord = record as KeyboardMovementInputRecord;
            if(moveRecord!=null)
            {
                EntityId = moveRecord.EntityId;
                x = moveRecord.x;
                y = moveRecord.y;
            }
         
        }
    }
}
