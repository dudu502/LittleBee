
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
                    var entity = World.GetEntity(com.EntityId);
                    if (entity != null)
                    {
                        PositionComponent posc = entity.GetComponent<PositionComponent>();
                        if (posc != null)
                            posc.SetPosition(com.GetPathV2());
                    }
                }
            }
        }
    }
}
