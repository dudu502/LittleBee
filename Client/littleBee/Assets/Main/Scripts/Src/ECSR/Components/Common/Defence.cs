
using Synchronize.Game.Lockstep.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class Defence:AbstractComponent
    {
        public FP BaseValue;
        public DefenceType DefenceType;
        public Defence(FP value,DefenceType type)
        {
            BaseValue = value;
            DefenceType = type;
        }
        public Defence() { }
        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Defence target = component as Defence;
            Enable = target.Enable;
            BaseValue = target.BaseValue;
            DefenceType = target.DefenceType;
        }
        public override AbstractComponent Clone()
        {
            Defence def = new Defence(BaseValue, DefenceType);
            def.EntityId = EntityId;
            def.Enable = Enable;
            return def;
        }
        public override string ToString()
        {
            return $"[Defence Id:{EntityId} DefenceType:{DefenceType}]";
        }
        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteInt64(BaseValue._serializedValue)
                    .WriteByte((byte)DefenceType).Getbuffer();
            }
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                BaseValue._serializedValue = buffer.ReadInt64();
                DefenceType = (DefenceType)buffer.ReadByte();
                return this;
            }
        }
    }
}
