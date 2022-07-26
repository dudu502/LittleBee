using System.Collections.Generic;
using Components;
using Components.Common;
using Entitas;
using TrueSync;
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
            World.ForEachComponent<Movement2D>(Move);
        }

        void Move(Movement2D moveComponent)
        {
            Transform2D transform = World.GetComponentByEntityId<Transform2D>(moveComponent.EntityId);
            if (transform != null)
            {
                transform.Position += moveComponent.GetMoveVector();
                if (moveComponent.Dir != TSVector2.zero)
                    transform.Toward = moveComponent.Dir;
            }
        }
    }
}
