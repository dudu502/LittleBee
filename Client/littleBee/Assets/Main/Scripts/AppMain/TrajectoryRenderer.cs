using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Misc;
using System;
using System.Threading;
using TrueSync;
using UnityEngine;
namespace Synchronize.Game.Lockstep.Ecsr.Renderer
{
    /// <summary>
    /// 弹道轨迹
    /// </summary>
    public class TrajectoryRenderer : ActionRenderer
    {
        LineRenderer TrackLineRenderer;
        public int MaxPointCount;
        bool IsDrawing = false;

        Particle Particle;
        Transform2D ParticleTransform;
        Movement2D ParticleMovement;
        Vector3[] LinePositionArray;

        private void Awake()
        {
            LinePositionArray = new Vector3[MaxPointCount];
            TrackLineRenderer = GetComponent<LineRenderer>();
            ParticleTransform = new Transform2D();
            ParticleMovement = new Movement2D();
            Particle = new Particle(1);
        }
        protected override void OnInit()
        {
            base.OnInit();
        }
        /// <summary>
        ///  Use Cloned Components
        /// </summary>
        /// <param name="particle"></param>
        /// <param name="particleMovement"></param>
        public void StartDraw(Transform2D componentTransform)
        {
            IsDrawing = true;
            Particle.EntityId = uint.MaxValue;
      
            ParticleTransform.EntityId = Particle.EntityId;
            ParticleTransform.Position = componentTransform.Position;
            ParticleTransform.Toward = componentTransform.Toward;
            ParticleTransform.Radius = 1;

            ParticleMovement.EntityId = uint.MaxValue;
            ParticleMovement.Speed = 1.2f;
            ParticleMovement.Dir = ParticleTransform.Toward;

            TrackLineRenderer.positionCount = MaxPointCount;
        }
        public void StopDraw()
        {
            IsDrawing = false;
        }
        // Start is called before the first frame update
        protected override void OnRender()
        {            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                IsDrawing = true;
                Debug.Log("Key Down");
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                IsDrawing = false;
                Debug.Log("Key Up");
            }
            base.OnRender();
            if (m_Simulation == null) TryFindSimulation();
            if (IsDrawing)
            {
                Transform2D componentTransform = m_Simulation.GetEntityWorld().GetComponentByEntityId<Transform2D>(EntityId);
                if (componentTransform != null)
                {
                    StartDraw(componentTransform);
                    CalcParticleTrack();
                }
            }
            else
            {
                TrackLineRenderer.positionCount = 0;
                Array.Clear(LinePositionArray, 0, MaxPointCount);
            }
        }
        void CalcParticleTrack()
        {
            ThreadPool.QueueUserWorkItem((obj)=> {
                lock (this)
                {
                    for (int i = 0; i < MaxPointCount; ++i)
                    {
                        if (Particle != null && ParticleTransform != null && ParticleMovement != null)
                        {
                            m_Simulation.GetEntityWorld().ForEachComponent<GravitationalField>(gravitationField =>
                            {
                                Transform2D gravityTransform = m_Simulation.GetEntityWorld().GetComponentByEntityId<Transform2D>(gravitationField.EntityId);
                                if (gravityTransform != null)
                                {
                                    TSVector2 interactiveDir = gravityTransform.Position - ParticleTransform.Position;
                                    FP lengthSquard = interactiveDir.LengthSquared();
                                    FP gravity = CalGravity(gravitationField.Mass, Particle.Mass, TSMath.Max(Const.MIN_LENGTHSQUARED, lengthSquard));
                                    TSVector2 newMoveDir = ParticleMovement.GetMoveVector() + interactiveDir.normalized * gravity;
                                    ParticleMovement.Dir = newMoveDir.normalized;
                                    ParticleMovement.Speed = TSMath.Min(newMoveDir.magnitude, Const.MAX_WORLD_SPEED);
                                }
                            });
                            ParticleTransform.Position += ParticleMovement.GetMoveVector();
                            if (ParticleMovement.Dir != TSVector2.zero)
                                ParticleTransform.Toward = ParticleMovement.Dir;
                            LinePositionArray[i] = new Vector3(ParticleTransform.Position.x.AsFloat(), 0, ParticleTransform.Position.y.AsFloat());
                        }
                    }
                    Handler.Run(_ => { lock (this) { TrackLineRenderer.SetPositions(LinePositionArray); } }, null);
                }
            });
        }
        private FP CalGravity(FP mass0, FP mass1, FP squareRaduis)
        {
            FP gravity = Const.GRAVITY * mass0 * mass1 / squareRaduis;
            if (gravity < Const.MIN_GRAVITY)
                return 0;
            return gravity;
        }
  
    }
}