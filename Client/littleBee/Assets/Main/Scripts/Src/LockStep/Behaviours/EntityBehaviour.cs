
using Synchronize.Game.Lockstep.Ecsr.Systems;
using Synchronize.Game.Lockstep.Proxy;
using Synchronize.Game.Lockstep.Room;
using System.Collections.Generic;


namespace Synchronize.Game.Lockstep.Behaviours
{
    /// <summary>
    /// 实体行为系统控制
    /// </summary>
    public class EntityBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim
        {
            set;get;
        }
        public List<IEntitySystem> Systems = new List<IEntitySystem>();
        protected RoomServiceProxy roomServices;
        protected LogicFrameBehaviour logicBehaviour;

        public EntityBehaviour()
        {
        
        }
        public virtual void Quit()
        {
            Systems.Clear();
        }
        public bool ContainSystem(IEntitySystem sys)
        {
            foreach (IEntitySystem item in Systems)
            {
                if (item == sys) return true;
                if (item.GetType() == sys.GetType()) return true;
            }
            return false;
        }
        public EntityBehaviour AddSystem(IEntitySystem sys)
        {
            if (!ContainSystem(sys))
            {
                Systems.Add(sys);
                sys.World = Sim.GetEntityWorld();
            }

            return this;
        }
        public virtual void Start()
        {
            roomServices = DataProxy.Get<RoomServiceProxy>();
            if (roomServices == null) throw new System.NullReferenceException();
            logicBehaviour = Sim.GetBehaviour<LogicFrameBehaviour>();
        }

        public virtual void Update()
        {
            Sim.GetEntityWorld().SortComponents();
            for (int i = 0; i < Systems.Count; ++i)
                Systems[i].Execute();
        }
    }
}
