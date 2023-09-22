using System;
using TrueSync;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using Synchronize.Game.Lockstep.Ecsr.Components.Common;

namespace Synchronize.Game.Lockstep.Ecsr.Systems
{
    /// <summary>
    /// 引力效应系统
    /// 对所有质点施加引力
    /// 需要受到引力作用的物体必须添加Particle属性
    /// </summary>
    public class GravitationalSystem : IEntitySystem
    {
        public EntityWorld World { get; set; }
      
        public void Execute()
        {
            //return;
            World.ForEachComponent<Particle>(ForEachParticleComponents);
        }
        private FP CalGravity(FP mass0, FP mass1,FP squareRaduis)
        {
            FP gravity = Const.GRAVITY * mass0 * mass1 / squareRaduis;
            if (gravity < Const.MIN_GRAVITY)
                return 0;
            return gravity;
        }
        private void ForEachParticleComponents(Particle particleComponent)
        {
            Transform2D particleTransform = World.GetComponentByEntityId<Transform2D>(particleComponent.EntityId);
            Movement2D move = World.GetComponentByEntityId<Movement2D>(particleComponent.EntityId);
            if(particleTransform != null)
            {
                World.ForEachComponent<GravitationalField>((gravitationalField) =>
                {
                    if (gravitationalField.Enable)
                    {
                        Transform2D gravityTransform = World.GetComponentByEntityId<Transform2D>(gravitationalField.EntityId);
                        if (gravityTransform != null)
                        {
                            TSVector2 interactiveDir = gravityTransform.Position - particleTransform.Position;
                            FP lengthSquard = interactiveDir.LengthSquared();
                            //if (lengthSquard < gravitationalField.EffectRadius * gravitationalField.EffectRadius)
                            {
                                FP gravity = CalGravity(gravitationalField.Mass, particleComponent.Mass, TSMath.Max(Const.MIN_LENGTHSQUARED, lengthSquard));
                                TSVector2 newMoveDir = move.GetMoveVector() + interactiveDir.normalized * gravity;
                                move.Dir = newMoveDir.normalized;
                                move.Speed = TSMath.Min(newMoveDir.magnitude, Const.MAX_WORLD_SPEED);
                            }
                        }
                    }
                });               
            }          
        }
    }
}
