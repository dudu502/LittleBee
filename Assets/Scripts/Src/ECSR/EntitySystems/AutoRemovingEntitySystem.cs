using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Components;
using Entitas;

namespace EntitySystems
{
    public class AutoRemovingEntitySystem : IEntitySystem
    {
        public EntityWorld World { set; get; }
        

        public void Execute()
        {
            var list = World.GetComponents<AutoRemovingEntityComponent>();
            if (list != null)
            {
                foreach(AutoRemovingEntityComponent comp in list)
                {
                    comp.AddCount();
                    UnityEngine.Debug.Log(comp.Count);
                }

                for(int i=list.Count-1;i>-1;--i)
                {
                    AutoRemovingEntityComponent comp = list[i] as AutoRemovingEntityComponent;
                    if (comp != null && comp.OverMaxCount())
                    {
                        World.NotifyRemoveEntity(comp.EntityId);
                    }
                }
            }
        }
    }
}
