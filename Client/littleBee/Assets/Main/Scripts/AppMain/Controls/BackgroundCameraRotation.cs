
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;
using Synchronize.Game.Lockstep.Ecsr.Renderer;
using Synchronize.Game.Lockstep.Ecsr.Components.Common;

namespace Synchronize.Game.Lockstep.Ecsr.Renderer
{
    public class BackgroundCameraRotation : ActionRenderer
    {
        protected override void OnRender()
        {
            base.OnRender();
            if (m_Simulation == null)
                TryFindSimulation();
            if (m_Simulation == null) return;
            if (m_Simulation.GetEntityWorld() == null) return;
            RotationValue rotationValue = m_Simulation.GetEntityWorld().GetComponentByEntityId<RotationValue>(EntityId);
            if (rotationValue != null)
                transform.rotation = rotationValue.Rotation.ToQuaternion();
        }
    }
}
