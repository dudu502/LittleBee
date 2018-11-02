using Entitas;

namespace EntitySystems
{
    /// <summary>
    /// 实体方法接口
    /// 集合于EntityBehaviour中
    /// </summary>
    public interface IEntitySystem
    {
        EntityWorld World { set; get; }
        void Execute();
    }
}
