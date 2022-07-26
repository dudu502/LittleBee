using Components;
using Components.Common;
using Components.Star;
using Entitas;
using TrueSync;
using TrueSync.Collision;

namespace EntitySystems
{
    public class NineBlockBoxCollisionSystem : IEntitySystem
    {
        public EntityWorld World { set; get; }

        public void Execute()
        {
            ICollisionProvider provider = World.GetCollisionProvider();
            if (provider != null)
            {
                provider.Clear();

                World.ForEachComponent<Transform2D>((transform) =>
                {
                    provider.Add(transform.Position, transform.EntityId);
                });

                World.ForEachComponent<Transform2D>((transform) =>
                {
                    if (transform.ActiveDetection)
                    {
                        transform.ClearCollisionEntityIds();
                        provider.CollisionDetection(transform.Position, id =>
                        {
                            if (id != transform.EntityId)
                            {
                                Transform2D otherTransform = World.GetComponentByEntityId<Transform2D>(id);
                                if (TSVector2.DistanceSquared(transform.Position, otherTransform.Position) <
                                    (otherTransform.Radius+ transform.Radius) * (otherTransform.Radius + transform.Radius))
                                {
                                    transform.OnCollisionEnter(id);
                                    return true;
                                }
                            }
                            return false;
                        });
                    }
                });
            }
        }
    }
}
