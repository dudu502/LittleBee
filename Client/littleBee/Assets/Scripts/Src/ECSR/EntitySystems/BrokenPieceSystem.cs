

using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Entitas;

namespace Synchronize.Game.Lockstep.Ecsr.Systems
{
    public class BrokenPieceSystem : IEntitySystem
    {
        public EntityWorld World { set; get; }
        public void Execute()
        {
            World.ForEachComponent<BreakableInfo>(info=> 
            {
                var hp = World.GetComponentByEntityId<Hp>(info.EntityId);
                if(hp.Value == 0)
                {
                    var transform = World.GetComponentByEntityId<Transform2D>(info.EntityId);
                    GravitationalField gravitationalField = World.GetComponentByEntityId<GravitationalField>(info.EntityId);
                    if (gravitationalField != null)
                        gravitationalField.Enable = false;
                    EntityManager.CreateBrokenPieceEntity( World,info.EntityId, info.PieceCount, info.PieceStartEntityId,transform.Position);//?
                }
            });
        }
    }
}
