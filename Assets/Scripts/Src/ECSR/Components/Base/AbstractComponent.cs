using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    /// <summary>
    /// 所有Component的基类
    /// 实现IComponent
    /// </summary>
    public abstract class AbstractComponent : IComponent
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
