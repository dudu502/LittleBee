
using Synchronize.Game.Lockstep.Ecsr.Components.Star;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using TrueSync;

namespace Synchronize.Game.Lockstep.Ecsr.Systems
{
    class StarMovingSystem : IEntitySystem
    {
        public EntityWorld World { set; get; }
        private readonly FP f360 = new FP(360);
        public void Execute()
        {
            World.ForEachComponent<StarObjectRevolution>(CalcRevolution);        
            World.ForEachComponent<StarObjectRotation>(CalcRotation);
        }
        void CalcRevolution(StarObjectRevolution rev)
        {
            Components.Common.Transform2D parentTransform = World.GetComponentByEntityId<Components.Common.Transform2D>(rev.ParentEntityId);
            rev.Degree += rev.Speed;
            FP rad = rev.Degree * FP.Deg2Rad;
            TSVector2 dir = new TSVector2(TSMath.Cos(rad), TSMath.Sin(rad));
            Components.Common.Transform2D selfTransform = World.GetComponentByEntityId<Components.Common.Transform2D>(rev.EntityId);
            selfTransform.Position = parentTransform.Position + dir * rev.Radius;
        }
        void CalcRotation(StarObjectRotation rot)
        {
            rot.Rotation *= TSQuaternion.AngleAxis(rot.Speed, TSVector.up);
            rot.Degree += rot.Speed;
            rot.Degree %= f360;
        }
    }
}
