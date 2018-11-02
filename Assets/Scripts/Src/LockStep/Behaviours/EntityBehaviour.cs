
using EntitySystems;
using System.Collections.Generic;

namespace LogicFrameSync.Src.LockStep.Behaviours
{
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
        public void AddSystem(IEntitySystem sys)
        {
            if (!ContainSystem(sys))
            {
                Systems.Add(sys);
                sys.World = Sim.GetEntityWorld();
            }               
        }
        public virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            foreach (IEntitySystem system in Systems)
                system.Execute();
        }
    }
}
