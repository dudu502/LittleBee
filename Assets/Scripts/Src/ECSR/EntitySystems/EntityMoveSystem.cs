using System.Collections.Generic;
using Components;
using Entitas;
using Unity.Mathematics;

namespace EntitySystems
{
    /// <summary>
    /// 移动系统 执行所有MoveComponent
    /// </summary>
    public class EntityMoveSystem : IEntitySystem
    {
        public EntityWorld World { set; get; }
        public void Execute()
        {
            var list = World.GetComponents<MoveComponent>();
            if (list != null)
            {
                foreach (MoveComponent com in list)
                {
                    TransformComponent pos = World.GetComponentByEntityId(com.EntityId, typeof(TransformComponent)) as TransformComponent;
                    if(pos!=null)
                    {
                        pos.Translate(com.GetPathV2());                        
                    }

                    BoxColliderComponent box = World.GetComponentByEntityId(com.EntityId, typeof(BoxColliderComponent)) as BoxColliderComponent;
                    if (box != null)
                    {
                        box.UpdateCollision(new VInt3((int)pos.LocalPosition.x, (int)pos.LocalPosition.y, 0));
                    }

                    SphereColliderComponent sphere = World.GetComponentByEntityId(com.EntityId, typeof(SphereColliderComponent)) as SphereColliderComponent;
                    if (sphere != null)
                    {
                        sphere.UpdateCollision(new VInt3((int)pos.LocalPosition.x, (int)pos.LocalPosition.y, 0));
                    }
                }
            }
        }
    }
}
