using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;
namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class RotationParameter : AbstractComponent
    {
        public TSVector Axis;
        public FP Speed;
        public RotationParameter()
        {

        }

        public RotationParameter(TSVector axis,FP speed)
        {
            Axis = axis;
            Speed = speed;
        }
        public override AbstractComponent Clone()
        {
            RotationParameter rotationParameter = new RotationParameter(Axis,Speed);
            rotationParameter.EntityId = EntityId;
            rotationParameter.Enable = Enable;
            return rotationParameter;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Enable = component.Enable;
            RotationParameter rotationParameter = component as RotationParameter;
            Axis = rotationParameter.Axis;
            Speed = rotationParameter.Speed;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                FP x, y, z;
                x._serializedValue = buffer.ReadInt64();
                y._serializedValue = buffer.ReadInt64();
                z._serializedValue = buffer.ReadInt64();
                Axis = new TSVector(x, y, z);
                Speed._serializedValue = buffer.ReadInt64();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteInt64(Axis.x._serializedValue)
                    .WriteInt64(Axis.y._serializedValue)
                    .WriteInt64(Axis.z._serializedValue)
                    .WriteInt64(Speed._serializedValue).Getbuffer();
            }
        }
    }
    public class RotationValue : AbstractComponent
    {
        public TSQuaternion Rotation;
        public RotationValue()
        {
            
        }
        public RotationValue(TSQuaternion rot)
        {
            Rotation = rot;
        }

        public override AbstractComponent Clone()
        {
            RotationValue value = new RotationValue(Rotation);
            value.Enable = Enable;
            value.EntityId = EntityId;
            return value;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Enable = component.Enable;
            RotationValue value = component as RotationValue;
            Rotation = value.Rotation;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                FP x, y, z,w;
                x._serializedValue = buffer.ReadInt64();
                y._serializedValue = buffer.ReadInt64();
                z._serializedValue = buffer.ReadInt64();
                w._serializedValue = buffer.ReadInt64();
                Rotation = new TSQuaternion(x, y, z,w);
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteInt64(Rotation.x._serializedValue)
                    .WriteInt64(Rotation.y._serializedValue)
                    .WriteInt64(Rotation.z._serializedValue)
                    .WriteInt64(Rotation.w._serializedValue).Getbuffer();
            }
        }
    }

}
