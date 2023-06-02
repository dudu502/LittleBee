
using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using TrueSync;

namespace Synchronize.Game.Lockstep.Ecsr.Systems
{
    /// <summary>
    /// 宏观层面战争系统
    /// 这部分战斗发生在太空，属于可见可操控的部分
    /// 主要处理包括子弹炮弹，飞行物体等参战元素
    /// </summary>
    public class MacroWarSystem : IEntitySystem
    {
        public EntityWorld World { set; get; }
     
        public void Execute()
        {
            World.ForEachComponent<Bullet>(bullet =>
            {
                if (bullet.State == 0)
                {
                    //Find all bullet and check collision ids .
                    Transform2D bulletTransform = World.GetComponentByEntityId<Transform2D>(bullet.EntityId);
                    if (bulletTransform.CollisionEntityId > 0)
                    {
                        if (bulletTransform.CollisionEntityId != bullet.OwnerEntityId)
                        {
                            uint collisionEntityId = bulletTransform.CollisionEntityId;
                            Attack bulletAttack = World.GetComponentByEntityId<Attack>(bullet.EntityId);
                            Hp collisionHp = World.GetComponentByEntityId<Hp>(collisionEntityId);
                            if (collisionHp != null)
                            {
                                Defence collisionDefence = World.GetComponentByEntityId<Defence>(collisionEntityId);
                                //Hurt=克制比例*AttackValue*（1-（DefanseValue*0.06）/（DefanseValue*0.06+1））*相克系数
                                FP hurt = bulletAttack.BaseValue - collisionDefence.BaseValue;
                                collisionHp.Hurt(TSMath.Ceiling(hurt).AsInt());
                                bullet.State = 1;
                            }
                        }
                        bulletTransform.ClearCollisionEntityIds();
                    }
                }
            });
        }
    }
}
