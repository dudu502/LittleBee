
using TrueSync;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Ecsr.Components.Star
{
    public class StarObjectRevolution : AbstractComponent
    {
        public int Radius;
        public FP Speed = 0;
        public FP Degree = 0;
        public uint ParentEntityId;
        public StarObjectRevolution()
        {

        }
        public override AbstractComponent Clone()
        {
            StarObjectRevolution comp = new StarObjectRevolution();
            comp.EntityId = EntityId;
            comp.Enable = Enable;
            comp.Radius = Radius;
            comp.Speed = Speed;
            comp.Degree = Degree;
            comp.ParentEntityId = ParentEntityId;
            return comp;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            StarObjectRevolution target = component as StarObjectRevolution;
            Enable = target.Enable;
            Radius = target.Radius;
            Speed = target.Speed;
            Degree = target.Degree;
            ParentEntityId = target.ParentEntityId;
        }
        public override string ToString()
        {
            return $"[StarObjectRevolution Id:{EntityId} Radius:{Radius} Speed:{Speed._serializedValue}({Speed}) Degree:{Degree._serializedValue}({Degree}) ParentEntityId:{ParentEntityId}]";
        }
        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                Radius = buffer.ReadInt32();
                Speed._serializedValue = buffer.ReadInt64();
                Degree._serializedValue = buffer.ReadInt64();
                ParentEntityId = buffer.ReadUInt32();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteInt32(Radius)
                    .WriteInt64(Speed._serializedValue)
                    .WriteInt64(Degree._serializedValue)
                    .WriteUInt32(ParentEntityId).Getbuffer();
               
            }
        }
    }
}
