using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public class AbstractComponent : IComponent
    {
        public string EntityId { set; get; }

        public bool Enable = true;

        public virtual IComponent Clone()
        {
            return null;
        }

        public virtual int GetCommand()
        {
            return 0;
        }
    }
}
