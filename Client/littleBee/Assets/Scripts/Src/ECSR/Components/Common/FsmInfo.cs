
using Synchronize.Game.Lockstep.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class FsmInfo : AbstractComponent
    {
        public FsmType InfoType { private set; get; }
        public FsmInfo(FsmType types)
        {
            InfoType = types;
        }
        public FsmInfo() { }
        public override AbstractComponent Clone()
        {
            FsmInfo fsm = new FsmInfo(InfoType);
            fsm.Enable = Enable;
            fsm.EntityId = EntityId;
            return fsm;
        }
        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            FsmInfo target = component as FsmInfo;
            Enable = target.Enable;
            InfoType = target.InfoType;
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId).WriteBool(Enable).WriteUInt16((ushort)InfoType).Getbuffer();
            }
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                InfoType = (FsmType)buffer.ReadUInt16();
                return this;
            }
        }
    }
}
