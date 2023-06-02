
using Synchronize.Game.Lockstep.Ecsr.Components.Star;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Ecsr.Renderer
{
    public class StarMoveActionRenderer: ActionRenderer
    {
        protected override void OnRender()
        {
            if (EntityId > 0)
            {
                Components.Common.Transform2D transform2d = m_Simulation.GetEntityWorld().GetComponentByEntityId<Components.Common.Transform2D>(EntityId);
                if (transform2d != null)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform2d.Position.x.AsFloat(),0,transform2d.Position.y.AsFloat()),0.5f);
                    StarObjectRotation rot = m_Simulation.GetEntityWorld().GetComponentByEntityId<StarObjectRotation>(EntityId);
                    if (rot != null)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, rot.Rotation.ToQuaternion(),0.5f);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
