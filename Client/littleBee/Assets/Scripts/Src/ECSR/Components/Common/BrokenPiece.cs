using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class BrokenPiece : AbstractComponent
    {
        public uint ParentId;
        public byte Index;
        public bool IsMainBody;
        public BrokenPiece() { }
        public BrokenPiece(uint parentId,byte idx,bool isMainBody)
        {
            ParentId = parentId;
            Index = idx;
            IsMainBody = isMainBody;
        }
        public override AbstractComponent Clone()
        {
            BrokenPiece brokenPiece = new BrokenPiece(ParentId, Index, IsMainBody);
            brokenPiece.EntityId = EntityId;
            brokenPiece.Enable = Enable;
            return brokenPiece;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            Enable = component.Enable;
            EntityId = component.EntityId;
            BrokenPiece brokenPiece = component as BrokenPiece;
            brokenPiece.ParentId = ParentId;
            brokenPiece.Index = Index;
            brokenPiece.IsMainBody = IsMainBody;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                ParentId = buffer.ReadUInt32();
                Index = buffer.ReadByte();
                IsMainBody = buffer.ReadBool();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId)
                    .WriteBool(Enable)
                    .WriteUInt32(ParentId)
                    .WriteByte(Index)
                    .WriteBool(IsMainBody).Getbuffer();
            }
        }
    }
}
