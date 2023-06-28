using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;
using Synchronize.Game.Lockstep.Managers;

namespace Synchronize.Game.Lockstep.Ecsr.Renderer
{
    public class BrokenPiecePlanetRenderer : ActionRenderer
    {
        byte m_PieceCount;
        List<Transform> m_PieceTrans = new List<Transform>();
        Dictionary<string, Pose> m_PiecePoseConfigs = new Dictionary<string, Pose>();
        protected override void OnInit()
        {
            base.OnInit();   
            GetComponent<PoolObject>().RecycleEvent.AddListener(() => {
                for (int i = 0; i < transform.childCount; ++i)
                {
                    Transform pieceTransform = transform.GetChild(i);
                    if(m_PiecePoseConfigs.TryGetValue(pieceTransform.name,out Pose pose))
                    {
                        pieceTransform.position = pose.position;
                        pieceTransform.rotation = pose.rotation;
                    }
                }
            });
            //GetComponent<PoolObject>().ReuseEvent.AddListener(()=> { });

            for(int i=0;i<transform.childCount;++i)
            {
                Transform pieceTransform = transform.GetChild(i);
                m_PiecePoseConfigs[pieceTransform.name] = new Pose(pieceTransform.position, pieceTransform.rotation);
            }
        }
        public void SetPieceCount(byte count)
        {
            m_PieceCount = count;
            if (m_PieceTrans.Count == 0)
            {
                for (int i = 0; i < m_PieceCount; ++i)
                {
                    m_PieceTrans.Add(transform.GetChild(i));
                }
            }
        }

        protected override void OnRender()
        {
            base.OnRender();
            bool allPieceDestroy = true;

            for (int i = 0; i < m_PieceCount; ++i)
            {
                Transform pieceTrans = m_PieceTrans[i];
                if (pieceTrans != null)
                {
                    var pieceT2d = m_Simulation.GetEntityWorld().GetComponentByEntityId<Components.Common.Transform2D>((uint)(EntityId + i));
                    if (pieceT2d != null)
                    {
                        pieceTrans.position = Vector3.Lerp(pieceTrans.position, new Vector3(pieceT2d.Position.x.AsFloat(), 0, pieceT2d.Position.y.AsFloat()), 1f);
                        allPieceDestroy = false;
                        pieceTrans.gameObject.SetActive(true);

                        var rotateValue = m_Simulation.GetEntityWorld().GetComponentByEntityId<Components.Common.RotationValue>((uint)(EntityId + i));
                        if (rotateValue != null)
                            pieceTrans.rotation = rotateValue.Rotation.ToQuaternion();
                    }
                    else
                    {
                        pieceTrans.gameObject.SetActive(false);
                    }
                }
            }
            if(allPieceDestroy)
                ModuleManager.GetModule<PoolModule>().Recycle(GetComponent<PoolObject>().GetFullName(), gameObject);
        }
    }
}