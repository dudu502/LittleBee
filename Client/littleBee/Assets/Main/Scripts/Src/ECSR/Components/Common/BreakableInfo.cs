using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class BreakableInfo : AbstractComponent
    {
        public byte PieceCount;
        public uint PieceStartEntityId;
        public BreakableInfo(byte count,uint startId)
        {
            PieceCount = count;
            PieceStartEntityId = startId;
        }
        public BreakableInfo() { }
        public override AbstractComponent Clone()
        {
            BreakableInfo breakableInfo = new BreakableInfo(PieceCount,PieceStartEntityId);
            breakableInfo.Enable = Enable;
            breakableInfo.EntityId = EntityId;
            return breakableInfo;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Enable = component.Enable;
            BreakableInfo breakableInfo = component as BreakableInfo;
            PieceCount = breakableInfo.PieceCount;
            PieceStartEntityId = breakableInfo.PieceStartEntityId;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                PieceCount = buffer.ReadByte();
                PieceStartEntityId = buffer.ReadUInt32();
                return this;
            }

        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteByte(PieceCount)
                    .WriteUInt32(PieceStartEntityId).Getbuffer();
            }
        }
    }
}
