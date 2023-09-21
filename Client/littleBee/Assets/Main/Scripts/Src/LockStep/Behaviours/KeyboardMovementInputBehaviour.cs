
using Synchronize.Game.Lockstep.Behaviours.Data;
using Synchronize.Game.Lockstep.Frame;
using Synchronize.Game.Lockstep.Proxy;
using Synchronize.Game.Lockstep.Room;
using TrueSync;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Behaviours
{
    public class KeyboardMovementInputBehaviour : ISimulativeBehaviour
    {
        KeyboardMovementInputRecord currentRecord;
        KeyboardMovementInputRecord record;
        uint Id;
        public Simulation Sim { set; get; }
        RoomServiceProxy roomService;
        public void Quit()
        {

        }
        LogicFrameBehaviour logic;
        public void Start()
        {
            roomService = DataProxy.Get<RoomServiceProxy>();
            Id = roomService.Session.Id;
            logic = Sim.GetBehaviour<LogicFrameBehaviour>();
            currentRecord = new KeyboardMovementInputRecord();
            record = new KeyboardMovementInputRecord();
            record.EntityId = Id;
        }
    
        public void Update()
        {
            if (Id >= 0) 
            {
                //Movement comp = Sim.GetEntityWorld().GetComponentByEntityId<Movement>(Id);
                //if (comp != null)
                {
                    Vector2 dir = Vector2.zero;
                    if (InputManager.Instance.IsKeyCodeActive(KeyCode.A))
                    {
                        dir.x = -1;
                    }
                    if (InputManager.Instance.IsKeyCodeActive(KeyCode.S))
                    {
                        dir.y = -1;
                    }
                    if (InputManager.Instance.IsKeyCodeActive(KeyCode.W))
                    {
                        dir.y = 1;
                    }
                    if (InputManager.Instance.IsKeyCodeActive(KeyCode.D))
                    {
                        dir.x = 1;
                    }
                    if (!InputManager.Instance.IsAnyKeyCodeActive())
                    {
                        dir.x = 0;
                        dir.y = 0;
                    }
                    currentRecord.EntityId = Id;
                    currentRecord.x = dir.x;
                    currentRecord.y = dir.y;

                    if (record.IsDirty(currentRecord))
                    {
                        record.CopyFrom(currentRecord);
                        roomService.Session.AddCurrentFrameCommand(logic.CurrentFrameIdx,FrameCommand.SYNC_MOVE, Id, 
                            new ByteBuffer().WriteByte(Const.INPUT_TYPE_KEYBOARD).WriteInt16((short)currentRecord.x).WriteInt16((short)currentRecord.y).Getbuffer());
                    }
                }
            }
        }
    }
}
