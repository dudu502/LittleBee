
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Synchronize.Game.Lockstep.Ecsr.Systems
{
    /// <summary>
    /// 微观层面的战争系统
    /// 战争发生在星球内 
    /// 这部分的战斗属于数据层面的微观战争，主要通过战斗力防御力等数值计算完成
    /// </summary>
    public class MicroWarSystem : IEntitySystem
    {
        public EntityWorld World { set; get; }

        public void Execute()
        {
            
        }
    }
}
