using TrueSync;

namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    /// <summary>
    /// 在引力场中受到万有引力作用的物体必须包含质点属性
    /// </summary>
    public class Particle: AbstractComponent
    {
        /// <summary>
        /// 质量
        /// TODO
        /// </summary>
        public FP Mass;
        public Particle(FP mass)
        {
            Mass = mass;
        }
        public Particle() { }
        public override AbstractComponent Clone()
        {
            Particle particle = new Particle(Mass);
            particle.Enable = Enable;
            particle.EntityId = EntityId;
            return particle; 
        }
        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Particle particle = component as Particle;
            Enable = particle.Enable;
            Mass = particle.Mass;
        }
        public override string ToString()
        {
            return $"[Particle Id:{EntityId} Mass:{Mass}]";
        }
        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId).WriteBool(Enable).WriteInt64(Mass._serializedValue).Getbuffer();
            }
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                Mass._serializedValue = buffer.ReadInt64();
                return this;
            }
        }
    }
}
