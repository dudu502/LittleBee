
using TrueSync;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Ecsr.Components.Star
{
    public class StarObjectRotation : AbstractComponent
    {
        public FP Speed = 0;
        public FP Degree = 0;
        public TSQuaternion Rotation = TSQuaternion.Euler(0,0,0);
        public StarObjectRotation() { }
        public override AbstractComponent Clone()
        {
            StarObjectRotation comp = new StarObjectRotation();
            comp.EntityId = EntityId;
            comp.Enable = Enable;
            comp.Speed = Speed;
            comp.Degree = Degree;
            comp.Rotation = Rotation;
            return comp;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            StarObjectRotation target = component as StarObjectRotation;
            Speed = target.Speed;
            Degree = target.Degree;
            Rotation = target.Rotation;
            Enable = target.Enable;
        }
        public override string ToString()
        {
            return $"[StarObjectRotation Id:{EntityId} Speed:{Speed._serializedValue}({Speed}) Degree:{Degree._serializedValue}({Degree}) Rotation:{Rotation.ToString()}]";
        }
        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                Speed._serializedValue = buffer.ReadInt64();
                Degree._serializedValue = buffer.ReadInt64();

                FP qx, qy, qz, qw;
                qx._serializedValue = buffer.ReadInt64();
                qy._serializedValue = buffer.ReadInt64();
                qz._serializedValue = buffer.ReadInt64();
                qw._serializedValue = buffer.ReadInt64();
                Rotation = new TSQuaternion(qx,qy,qz,qw);
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteInt64(Speed._serializedValue)
                    .WriteInt64(Degree._serializedValue)
                    .WriteInt64(Rotation.x._serializedValue)
                    .WriteInt64(Rotation.y._serializedValue)
                    .WriteInt64(Rotation.z._serializedValue)
                    .WriteInt64(Rotation.w._serializedValue).Getbuffer();
            }
        }
    }
}
