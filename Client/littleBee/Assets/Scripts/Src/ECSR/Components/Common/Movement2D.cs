
using Synchronize.Game.Lockstep.Frame;
using TrueSync;

namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class Movement2D : AbstractComponent
    {
        public FP Speed = 1;
        public TSVector2 Dir = TSVector2.zero;
        public Movement2D(FP speed, TSVector2 dir)
        {
            Dir = dir;
            Speed = speed;
        }
        public Movement2D() { }
        public TSVector2 GetMoveVector()
        {
            return Dir * Speed;
        }


        override public AbstractComponent Clone()
        {
            Movement2D com = new Movement2D(Speed, Dir);
            com.Enable = Enable;
            com.EntityId = EntityId;
            return com;
        }
        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Movement2D target = component as Movement2D;
            Enable = target.Enable;
            Speed = target.Speed;
            Dir = target.Dir;
        }
        override public ushort GetCommand()
        {
            return FrameCommand.SYNC_MOVE;
        }

        public override void UpdateParams(byte[] content)
        {
            using(ByteBuffer buffer= new ByteBuffer(content))
            {
                byte type = buffer.ReadByte();
                switch (type)
                {
                    case Const.INPUT_TYPE_KEYBOARD:
                        Dir = new TSVector2(buffer.ReadInt16(),buffer.ReadInt16());
                        break;
                    case Const.INPUT_TYPE_JOYSTICK:
                        FP serializedDirX, serializedDirY;
                        serializedDirX._serializedValue = buffer.ReadInt64();
                        serializedDirY._serializedValue = buffer.ReadInt64();
                        Dir = new TSVector2(serializedDirX, serializedDirY);                
                        break;
                    default:
                        break;
                }

            }   
        }
        public override string ToString()
        {
            return $"[Movement Id:{EntityId} Dir:{Dir.x._serializedValue}({Dir.x}) {Dir.y._serializedValue}({Dir.y}) Speed:{Speed._serializedValue}({Speed})]";
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteInt64(Speed._serializedValue)
                    .WriteInt64(Dir.x._serializedValue)
                    .WriteInt64(Dir.y._serializedValue).Getbuffer();
            }
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                Speed._serializedValue = buffer.ReadInt64();
                FP dirX, dirY;
                dirX._serializedValue = buffer.ReadInt64();
                dirY._serializedValue = buffer.ReadInt64();
                Dir = new TSVector2(dirX,dirY);
                return this;
            }
        }
    }
}


