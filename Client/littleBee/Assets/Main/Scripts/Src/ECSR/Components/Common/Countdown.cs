using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Synchronize.Game.Lockstep.Ecsr.Components.Common
{
    public class Countdown:AbstractComponent
    {
        public int Count { private set; get; }
        public Countdown(int count)
        {
            Count = count;
        }
        public Countdown() { }
        public void CountdownOnce()
        {
            --Count;
        }

        public override AbstractComponent Clone()
        {
            Countdown comp = new Countdown(Count);
            comp.EntityId = EntityId;
            comp.Enable = Enable;
            return comp;
        }
        public override string ToString()
        {
            return $"[CoundDown Id:{EntityId}]";
        }
        public override void CopyFrom(AbstractComponent component)
        {
            EntityId = component.EntityId;
            Countdown target = component as Countdown;
            Enable = target.Enable;
            Count = target.Count;
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer= new ByteBuffer())
            {
                return buffer.WriteUInt32(EntityId).WriteBool(Enable).WriteInt32(Count).Getbuffer();
            }
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                EntityId = buffer.ReadUInt32();
                Enable = buffer.ReadBool();
                Count = buffer.ReadInt32();
                return this;
            }
        }
    }
}
