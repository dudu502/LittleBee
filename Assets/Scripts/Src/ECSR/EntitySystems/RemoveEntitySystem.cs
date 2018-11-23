using System;
using System.Collections.Generic;
using Entitas;

namespace EntitySystems
{
    public class RemoveEntitySystem : IEntitySystem
    {
        public EntityWorld World { set; get; }

        public void Execute()
        {
            var entities = World.GetEntities();
            if (entities != null)
            {
                for (int i = entities.Count - 1; i > -1; --i)
                {
                    Entity ent = entities[i];
                    if (ent.LifeState == 0)
                    {
                        World.NotifyRemoveEntity(ent.Id);
                    }
                }
            }
        }
    }
}
