using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Components.Star;
using Components.Common;
using Renderers;
using TrueSync;
using Components;

namespace Map.Ecsr.R
{
    class StarObjectRender:MonoBehaviour
    {
        public uint EntityId;
        public Entitas.EntityWorld World;
        private void Update()
        {
            if(EntityId>0)
            {
                Components.Common.Transform2D pos = World.GetComponentByEntityId<Components.Common.Transform2D>(EntityId);
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
