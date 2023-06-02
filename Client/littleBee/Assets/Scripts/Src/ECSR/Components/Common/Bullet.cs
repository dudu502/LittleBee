namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class Bullet:AbstractComponent
    {
        public uint OwnerEntityId { private set; get; }
        public byte State;//0 normal 1 fired
        public Bullet (uint ownerEntityId)
        {
            OwnerEntityId = ownerEntityId;
        }
        public Bullet() { }
        public override AbstractComponent Clone()
        {
            Bullet comp = new Bullet(OwnerEntityId);
            comp.EntityId = EntityId;
            comp.Enable = Enable;
            comp.State = State;
            return comp;
        }
        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Bullet target = component as Bullet;
            Enable = target.Enable;
            OwnerEntityId = target.OwnerEntityId;
            State = target.State;
        }
        public override string ToString()
        {
            return $"[Bullet Id:{EntityId} OwnerEntityId:{OwnerEntityId} State:{State}]";
        }
        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteByte(State)
                    .WriteUInt32(OwnerEntityId).Getbuffer();
            }
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                State = buffer.ReadByte();
                OwnerEntityId = buffer.ReadUInt32();
                return this;
            }
        }
    }
}
