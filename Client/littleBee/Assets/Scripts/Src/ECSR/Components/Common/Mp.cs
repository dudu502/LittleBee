using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class Mp:AbstractComponent
    {
        public int Value;
        public Mp(int value)
        {
            Value = value;
        }
        public Mp() { }
        public override AbstractComponent Clone()
        {
            Mp mp = new Mp(Value);
            mp.Enable = Enable;
            mp.EntityId = EntityId;
            return mp;
        }
        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Enable = component.Enable;
            Mp target = component as Mp;           
            Value = target.Value;
        }
        public override string ToString()
        {
            return $"[Mp Id:{EntityId} Value:{Value}]";
        }
        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                Value = buffer.ReadInt32();
                return this;
            }

        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId).WriteBool(Enable).WriteInt32(Value).Getbuffer();
            }
        }
    }
}
