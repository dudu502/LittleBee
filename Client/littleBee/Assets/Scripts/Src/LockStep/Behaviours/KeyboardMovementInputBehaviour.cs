using Components;
using Components.Common;
using Frame;
using LogicFrameSync.Src.LockStep.Behaviours.Data;
using LogicFrameSync.Src.LockStep.Frame;
using NetServiceImpl;
using NetServiceImpl.OnlineMode.Room;
using TrueSync;
using UnityEngine;

namespace LogicFrameSync.Src.LockStep.Behaviours
{
    public class KeyboardMovementInputBehaviour : ISimulativeBehaviour
    {
        KeyboardMovementInputRecord currentRecord;
        KeyboardMovementInputRecord record;
        uint Id;
        public Simulation Sim { set; get; }
        public void Quit()
        {

        }
        LogicFrameBehaviour logic;
        public void Start()
        {
            Id = ClientService.Get<RoomServices>().Session.Id;
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
                        KeyFrameSender.AddCurrentFrameCommand(logic.CurrentFrameIdx,FrameCommand.SYNC_MOVE, Id, 
                            new ByteBuffer().WriteByte(Const.INPUT_TYPE_KEYBOARD).WriteInt16((short)currentRecord.x).WriteInt16((short)currentRecord.y).Getbuffer());
                    }
                }
            }
        }
    }
}
