
using Synchronize.Game.Lockstep.Misc;
using TrueSync;
namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class Attack:AbstractComponent
    {
        public AttackType AttackType;
        public FP BaseValue;
        public byte AttackSpeed=1;
        public Attack(FP value,AttackType type)
        {
            BaseValue = value;
            AttackType = type;
        }
        public Attack() { }

        public override AbstractComponent Clone()
        {
            Attack att = new Attack(BaseValue, AttackType);
            att.EntityId = EntityId;
            att.Enable = Enable;
            att.AttackSpeed = AttackSpeed;
            return att;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Attack target = component as Attack;
            Enable = target.Enable;
            AttackType = target.AttackType;
            BaseValue = target.BaseValue;
            AttackSpeed = target.AttackSpeed;
        }
        public override string ToString()
        {
            return $"[Attack Id:{EntityId} AttackType:{AttackType} BaseValue:{BaseValue._serializedValue} AttackSpeed:{AttackSpeed}]";
        }
        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                AttackType = (AttackType)buffer.ReadByte();
                BaseValue._serializedValue = buffer.ReadInt64();
                AttackSpeed = buffer.ReadByte();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteByte((byte)AttackType)
                    .WriteInt64(BaseValue._serializedValue)
                    .WriteByte(AttackSpeed).Getbuffer();
            }
        }
    }
}
