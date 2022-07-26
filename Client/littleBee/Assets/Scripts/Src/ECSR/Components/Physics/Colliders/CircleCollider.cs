using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

namespace Components.Physics.Colliders
{
    public class CircleCollider : AbstractComponent
    {
        public TSVector2 Center;
        public FP Radius;
        public CircleCollider() { }
        public CircleCollider(TSVector2 center, FP radius)
        {
            Center = center;
            Radius = radius;
        }
        public override AbstractComponent Clone()
        {
            CircleCollider collider = new CircleCollider(Center,Radius);
            collider.Enable = Enable;
            collider.EntityId = EntityId;
            return collider;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Enable = component.Enable;

            CircleCollider target = component as CircleCollider;
            Center = target.Center;
            Radius = target.Radius;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                FP cx, cy;
                cx._serializedValue = buffer.ReadInt64();
                cy._serializedValue = buffer.ReadInt64();
                Center = new TSVector2(cx, cy);
                Radius._serializedValue = buffer.ReadInt64();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteInt64(Center.x._serializedValue)
                    .WriteInt64(Center.y._serializedValue)
                    .WriteInt64(Radius._serializedValue)
                    .Getbuffer();
            }
        }
    }
}
