using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class HudInfo:AbstractComponent
    {
        public HudInfo()
        {

        }
        public override AbstractComponent Clone()
        {
            HudInfo hudInfo = new HudInfo();
            hudInfo.EntityId = EntityId;
            hudInfo.Enable = Enable;
            return hudInfo;
        }
        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Enable = component.Enable;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId).WriteBool(Enable).Getbuffer();
            }
        }
    }
}
