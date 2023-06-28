
using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
namespace Synchronize.Game.Lockstep.Ecsr.Systems
{
    public class RemoveEntitySystem : IEntitySystem
    {        
        public EntityWorld World { set; get; }
        Queue<uint> removeQueues = new Queue<uint>();
        public void Execute()
        {
            try
            {
                World.ForEachComponent<Countdown>((countdown) =>
                {
                    countdown.CountdownOnce();
                    if (countdown.Count <= 0)
                        removeQueues.Enqueue(countdown.EntityId);
                });

                World.ForEachComponent<Hp>(hp =>
                {
                    if (hp.Value <= 0)
                        removeQueues.Enqueue(hp.EntityId);
                });
                World.ForEachComponent<Bullet>(bullet =>
                {
                    if (bullet.State == 1)
                        removeQueues.Enqueue(bullet.EntityId);
                });

                while(removeQueues.TryDequeue(out var entityId))
                {
                    World.RemoveEntity(entityId);
                    UnityEngine.Debug.Log($"[RemoveEntitySystem] REMOVE {entityId} {DateTime.Now.ToString()}");
                }

            }catch(Exception exc)
            {
                UnityEngine.Debug.LogError($"[RemoveEntitySystem] Error {exc.ToString()} {DateTime.Now.ToString()}");
            }            
        }
    }
}
