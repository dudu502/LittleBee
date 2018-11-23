using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;
using Components;
namespace EntitySystems
{
    public class FrameClockSystem : IEntitySystem
    {
        public EntityWorld World { set; get; }

        public void Execute()
        {
            var list = World.GetComponents<FrameClockComponent>();
            if (list != null)
            {
                foreach(FrameClockComponent com in list)
                {
                    com.UpdateCount();
                    if(com.IsOver())
                    {
                        Entity ent = World.GetEntity(com.EntityId);
                        if (ent != null)
                        {
                            ent.LifeState = 0;
                        }
                    }
                    
                }
            }
        }
    }
}
