using LogicFrameSync.Src.LockStep.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Components
{
    public class PositionComponent:IComponent
    {
        Vector2 Pos;
        public PositionComponent(Vector2 v3)
        {
            Pos = v3;
        }
        public override string ToString()
        {
            return string.Format("[PositionComponent Id:{0} Pos:{1},{2}]",EntityId, Pos.x, Pos.y);
        }
        public bool SetPosition(Vector2 pos)
        {
            Pos += pos;
            return true;
        }
        public Vector2 GetPosition() { return Pos; }

        public bool Enable { set; get; }
        public string EntityId { set; get; }
        public IComponent Clone()
        {
            PositionComponent com = new PositionComponent(Pos);
            com.Enable = Enable;
            com.EntityId = EntityId;
            return com;
        }
        public int GetCommand()
        {
            return FrameCommand.SYNC_POSITION;
        }
    }
}
