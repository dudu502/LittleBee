using Components;
using Components.Common;
using Entitas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;

namespace EntitySystems
{
    public class RemoveEntitySystem : IEntitySystem
    {        
        public EntityWorld World { set; get; }
        List<uint> removingList = new List<uint>();
        public void Execute()
        {
            try
            {
                removingList.Clear();
                World.ForEachComponent<Countdown>((countdown) =>
                {
                    countdown.CountdownOnce();
                    if (countdown.Count <= 0)
                        removingList.Add(countdown.EntityId);
                });

                World.ForEachComponent<Hp>(hp =>
                {
                    if (hp.Value <= 0)
                        removingList.Add(hp.EntityId);
                });
                World.ForEachComponent<Bullet>(bullet =>
                {
                    if (bullet.State == 1)
                        removingList.Add(bullet.EntityId);
                });
                while (removingList.Count > 0)
                {
                    uint entityId = removingList[removingList.Count - 1];
                    removingList.RemoveAt(removingList.Count - 1);
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
