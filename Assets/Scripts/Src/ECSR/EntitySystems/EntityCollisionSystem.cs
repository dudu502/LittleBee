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
                    Debug.Log(com.Collider.Intersects(com1.Collider));
                }
            }

        }
    }
}
