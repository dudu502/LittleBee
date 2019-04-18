using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Components;
using Entitas;
using UnityEngine;

namespace EntitySystems
{
    /// <summary>
    /// 碰撞检测系统
    /// 包含Components/Physics/ 内组件
    /// </summary>
    public class EntityCollisionSystem : IEntitySystem
    {
        public EntityWorld World
        {
            set;get;
        }

        public void Execute()
        {
            List<IComponent> colliders = new List<IComponent>();
            var boxcolliders = World.GetComponents<BoxColliderComponent>();
            if (boxcolliders != null)
                colliders.AddRange(boxcolliders);
            var spherecolliders = World.GetComponents<SphereColliderComponent>();
            if (spherecolliders != null)
                colliders.AddRange(spherecolliders);

            foreach (ICollisionUpdatable com in colliders)
            {
                foreach (ICollisionUpdatable com1 in colliders)
                {
                    if (com == com1) continue;
                    if (com.Collider.Intersects(com1.Collider))
                    {
                        DoingInersectsImpl(World.GetEntity(((AbstractComponent)com).EntityId),
                            World.GetEntity(((AbstractComponent)com1).EntityId));
                        break;
                    }
                }
            }

            while(list.Count>0)
            {
                var id = list[0];
                World.NotifyRemoveEntity(id);
                list.RemoveAt(0);
            }
        }

        List<Guid> list = new List<Guid>();
        void DoingInersectsImpl(Entity e1,Entity e2)
        {
            if (e1 == null || e2 == null) return;
            PlayerInfoComponent playerInfo = e1.GetComponent<PlayerInfoComponent>();
            if (playerInfo != null)
            {
                IntValueComponent intvalue = e2.GetComponent<IntValueComponent>();
                if (intvalue != null)
                {
                    playerInfo.Value += intvalue.Value;
                    list.Add(e2.Id);
                }                                
            }
        }
    }
}
