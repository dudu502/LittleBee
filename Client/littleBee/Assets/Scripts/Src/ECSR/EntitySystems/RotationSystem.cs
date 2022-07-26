using Components.Common;
using Entitas;
using TrueSync;
namespace EntitySystems
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