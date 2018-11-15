
using Components;
using Entitas;

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
                }
            }
        }
    }
}
