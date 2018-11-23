using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public class IntValueComponent:AbstractComponent
    {
        public int Value { private set; get; }
        public IntValueComponent(int initValue)
        {
            Value = initValue;
        }
        public void AddDelta(int delta)
        {
            Value += delta;
        }
        public override IComponent Clone()
        {
            IntValueComponent com = new IntValueComponent(Value);
            com.EntityId = EntityId;
            com.Enable = Enable;
            return com;
        }
        public override string ToString()
        {
            return string.Format("[IntValueComponent Id:{0} Value:{1}]",EntityId,Value);
        }
    }
}
