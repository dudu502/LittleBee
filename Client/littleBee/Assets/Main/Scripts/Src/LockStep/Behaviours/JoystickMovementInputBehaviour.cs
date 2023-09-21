using Synchronize.Game.Lockstep.Behaviours.Data;
using Synchronize.Game.Lockstep.Frame;
using Synchronize.Game.Lockstep.Proxy;
using Synchronize.Game.Lockstep.Room;

using TrueSync;


namespace Synchronize.Game.Lockstep.Behaviours
{
    public class JoystickMovementInputBehaviour : ISimulativeBehaviour
    {
        uint Id;
        JoystickMovementInputRecord currentRecord;
        JoystickMovementInputRecord record;
        public Simulation Sim { set; get; }
        LogicFrameBehaviour logic;
        RoomServiceProxy roomService;
        public void Quit()
        {
            
        }

        public void Start()
        {
            roomService = DataProxy.Get<RoomServiceProxy>();
            Id = roomService.Session.Id;
            logic = Sim.GetBehaviour<LogicFrameBehaviour>();
            currentRecord = new JoystickMovementInputRecord();
            currentRecord.EntityId = Id;
            record = new JoystickMovementInputRecord();
            record.EntityId = Id;
        }

        public void Update()
        {
            if (Id > 0)
            {
                if( Joystick.Instance != null)
                {
                    TSVector2 currentJoystickDir = Joystick.Instance.GetCurrentDirection();

                    currentRecord.x = currentJoystickDir.x;
                    currentRecord.y = currentJoystickDir.y;
                    if (record.IsDirty(currentRecord))
                    {
                        record.CopyFrom(currentRecord);
                        roomService.Session.AddCurrentFrameCommand(logic.CurrentFrameIdx, FrameCommand.SYNC_MOVE, Id, 
                            new ByteBuffer().WriteByte(Const.INPUT_TYPE_JOYSTICK).WriteInt64(currentRecord.x._serializedValue).WriteInt64(currentRecord.y._serializedValue).Getbuffer());
                    }
                }
            }
        }
    }
}
