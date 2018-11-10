
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
                    PositionComponent pos = World.GetComponentByEntityId(com.EntityId, typeof(PositionComponent)) as PositionComponent;
                    if(pos!=null)
                        pos.SetPosition(com.GetPathV2());
                }
            }
        }
    }
}
