
using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using System;
using System.Collections.Generic;

namespace Synchronize.Game.Lockstep.Ecsr.Systems
{
    public class StateObject
    {
        protected uint EntityId;
        protected List<Type> Types;
        public StateObject(uint entityId,List<Type> types) 
        {
            EntityId = entityId;
            Types = types;
        }
    }
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
