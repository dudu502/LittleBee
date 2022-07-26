using Components.Common;
using Entitas;


namespace EntitySystems
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
