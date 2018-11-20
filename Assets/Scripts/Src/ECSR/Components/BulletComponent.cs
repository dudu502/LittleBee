using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    /// <summary>
    /// 子弹组件
    /// </summary>
    public class BulletComponent : IComponent
    {
        public string EntityId
        {
            set;get;
        }

        /// <summary>
        /// 目标者EntityId
        /// </summary>
        public string TargetEntityId { private set; get; }

        public BulletComponent(string targetEntityId)
        {
            TargetEntityId = targetEntityId;
        }
        public bool Enable { set; get; }

        public IComponent Clone()
        {
            BulletComponent comp = new BulletComponent(TargetEntityId);
            comp.Enable = Enable;
            comp.EntityId = EntityId;
            return comp;
        }

        public int GetCommand()
        {
            throw new NotImplementedException();
        }
    }
}
