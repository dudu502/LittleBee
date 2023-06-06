
using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using TrueSync;
namespace Synchronize.Game.Lockstep.Ecsr.Systems
{
    public class RotationSystem : IEntitySystem
    {
        public EntityWorld World { set; get; }

        public void Execute()
        {
            World.ForEachComponent<RotationParameter>(param =>
            {
                RotationValue rotationValue = World.GetComponentByEntityId<RotationValue>(param.EntityId);
                if (rotationValue != null)
                {
                    rotationValue.Rotation *= TSQuaternion.AngleAxis(param.Speed, param.Axis);
                }
            });
        }
    }
}