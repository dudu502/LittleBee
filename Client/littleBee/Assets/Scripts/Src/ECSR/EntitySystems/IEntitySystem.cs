
using Synchronize.Game.Lockstep.Ecsr.Entitas;

namespace Synchronize.Game.Lockstep.Ecsr.Systems
{
    /// <summary>
    /// 实体方法接口 ECSR 中的（S）
    /// 实体的具体方法只能在System中是Execute中实现
    /// 通过World可以获取Components和Entities
    /// 集合于EntityBehaviour中
    /// </summary>
    public interface IEntitySystem
    {
        EntityWorld World { set; get; }
        void Execute();
    }
}
