using System;
using System.Collections.Generic;

namespace Synchronize.Game.Lockstep.Ecsr.Components
{
    /// <summary>
    /// 所有Component的基类
    /// 实现IComponent
    /// </summary>
    public abstract class AbstractComponent : IComparable
    {
        public uint EntityId { set; get; }
        public bool Enable = true;
        public abstract AbstractComponent Clone();
        public abstract void CopyFrom(AbstractComponent component);
        public abstract byte[] Serialize();
        public abstract AbstractComponent Deserialize(byte[] bytes);
        public virtual ushort GetCommand() { return 0; }
        public virtual void UpdateParams(byte[] content) { }

        public int CompareTo(object obj)
        {
            int entitySortState = EntityId.CompareTo(((AbstractComponent)obj).EntityId);
            if(entitySortState==0)
            {
                return GetType().Name.CompareTo(obj.GetType().Name);
            }
            else
            {
                return entitySortState;
            }
        }
    }
}
