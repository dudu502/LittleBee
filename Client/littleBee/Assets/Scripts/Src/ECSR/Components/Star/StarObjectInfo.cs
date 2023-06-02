
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Ecsr.Components.Star
{
    public class StarObjectInfo:AbstractComponent
    {
        public int ConfigId;
        public StarObjectInfo()
        {

        }
        public override AbstractComponent Clone()
        {
            StarObjectInfo comp = new StarObjectInfo();
            comp.EntityId = EntityId;
            comp.Enable = Enable;
            comp.ConfigId = ConfigId;
            return comp;
        }
        public override string ToString()
        {
            return $"[StarObjectInfo Id:{EntityId} ConfigId:{ConfigId}]";
        }
        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            StarObjectInfo target = component as StarObjectInfo;
            Enable = target.Enable;
            ConfigId = target.ConfigId;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                ConfigId = buffer.ReadInt32();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteInt32(ConfigId).Getbuffer();            
            }
        }
    }
}
