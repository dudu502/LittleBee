using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TrueSync;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Components.Star;

namespace Synchronize.Game.Lockstep.MapEditor
{
    class StarObjectRender:MonoBehaviour
    {
        public uint EntityId;
        public EntityWorld World;
        private void Update()
        {
            if(EntityId>0)
            {
                Transform2D pos = World.GetComponentByEntityId<Transform2D>(EntityId);
                if(pos!=null)
                {
                    transform.position = new Vector3(pos.Position.x.AsFloat(),0,pos.Position.y.AsFloat());
                }

                StarObjectRotation rot = World.GetComponentByEntityId<StarObjectRotation>(EntityId);
                if(rot!=null)
                {
                    transform.rotation = rot.Rotation.ToQuaternion();
                }
            }
        }
    }
}
