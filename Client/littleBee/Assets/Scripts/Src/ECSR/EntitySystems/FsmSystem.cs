
using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Entitas;

namespace Synchronize.Game.Lockstep.Ecsr.Systems
{
    /// <summary>
    /// 有限状态机系统
    /// </summary>
    public class FsmSystem : IEntitySystem
    {
        public EntityWorld World { set; get; }
        
        public void Execute()
        {
            //World.ForEachComponent<FsmInfo>(ForEachFsmInfo);
            //CleanUp();
        }

        private void CleanUp()
        {
            
        }

        private void ForEachFsmInfo(FsmInfo fsm)
        {

        }
    }
}
