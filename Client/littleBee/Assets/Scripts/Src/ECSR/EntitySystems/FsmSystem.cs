using Components.Common;
using Entitas;
using System.Collections.Generic;

namespace EntitySystems
{
    /// <summary>
    /// 有限状态机系统
    /// </summary>
    public class FsmSystem : IEntitySystem
    {
        public EntityWorld World { set; get; }
       
        public void Execute()
        {
            World.ForEachComponent<FsmInfo>(ForEachFsmInfo);
            CleanUp();
        }

        private void CleanUp()
        {
            
        }

        private void ForEachFsmInfo(FsmInfo fsm)
        {

        }
    }

    public class FsmTypeDefine
    {
       
    }
}
