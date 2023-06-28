using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class GravitationalField : AbstractComponent
    {
        /// <summary>
        /// 考虑到大质量物体黑洞和一般物体
        /// 还是希望用short
        /// </summary>
        public byte Mass;
        public byte EffectRadius;
        public GravitationalField(byte mass,byte effectRadius)
        {
            Mass = mass;
            EffectRadius = effectRadius;
        }
        public GravitationalField() { }

        public override AbstractComponent Clone()
        {
            GravitationalField comp = new GravitationalField(Mass, EffectRadius);
            comp.EntityId = EntityId;
            comp.Enable = Enable;
            return comp;
        }
        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            GravitationalField target = component as GravitationalField;
            Enable = target.Enable;
            Mass = target.Mass;
            EffectRadius = target.EffectRadius;
        }
        public override string ToString()
        {
            return $"[GravitationalField Id:{EntityId} Mass:{Mass} EffectRadius:{EffectRadius}]";
        }
        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                Mass = buffer.ReadByte();
                EffectRadius = buffer.ReadByte();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteByte(Mass)
                    .WriteByte(EffectRadius).Getbuffer();
            }
        }
    }
}
