using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public class AutoRemovingEntityComponent : AbstractComponent
    {
        public int Count = 0;

        public int Max = 0;
        public AutoRemovingEntityComponent(int maxCount)
        {
            Max = maxCount;
        }
        override public IComponent Clone()
        {
            AutoRemovingEntityComponent comp = new AutoRemovingEntityComponent(Max);
            comp.Enable = Enable;
            comp.EntityId = EntityId;
            comp.Count = Count;
            return comp;
        }
        public override string ToString()
        {
            return string.Format("[AutoRemovingEntityComponent Id:{0} Count:{1}]",EntityId,Count);
        }
        public void AddCount()
        {
            ++Count;
        }
        public bool OverMaxCount() { return Count >= Max; }

        override public int GetCommand()
        {
            return 0;
        }
    }
}
