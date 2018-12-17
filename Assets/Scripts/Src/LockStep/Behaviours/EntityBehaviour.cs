using EntitySystems;
using System.Collections.Generic;

namespace LogicFrameSync.Src.LockStep.Behaviours
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
       
        public EntityBehaviour()
        {
        }

        public virtual void Quit()
        {
            
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
            
        }

        public virtual void Update()
        {
            for (int i = 0; i < Systems.Count; ++i)
                Systems[i].Execute();
        }
    }
}
