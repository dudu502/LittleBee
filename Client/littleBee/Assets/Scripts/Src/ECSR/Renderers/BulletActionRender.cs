using UnityEngine;
using System.Collections;
using TrueSync;
using Synchronize.Game.Lockstep.Managers;

namespace Synchronize.Game.Lockstep.Ecsr.Renderer
{
    public class BulletActionRender : ActionRenderer
    {
        /// <summary>
        /// 发射口
        /// </summary>
        public GameObject m_GoMuzzle;

        /// <summary>
        /// 子弹
        /// </summary>
        public GameObject m_GoBullet;

        /// <summary>
        /// 爆炸
        /// </summary>
        public GameObject m_GoExplosion;
        public float m_Scale=2;
        private GameObject m_GoBulletInstance;
        private GameObject m_GoMuzzleInstance;
        private GameObject m_GoExplosionInstance;
        //0:normal 1:hit 2:wait for gc
        byte m_LifeStateRender = 0;
        protected override void OnInit()
        {
            base.OnInit();
            m_GoBulletInstance = GameObject.Instantiate(m_GoBullet) as GameObject;
            m_GoBulletInstance.transform.SetParent(transform);
            m_GoBulletInstance.transform.localPosition = Vector3.zero;
            m_GoBulletInstance.transform.localRotation = Quaternion.identity;
            Common.Utils.SetChildrenLayer(m_GoBulletInstance, gameObject.layer);
            m_GoBulletInstance.SetActive(false);
            transform.localScale = m_Scale * Vector3.one;
        }
        void PlayMuzzle()
        {
            if (m_GoMuzzleInstance == null)
            {
                m_GoMuzzleInstance = GameObject.Instantiate(m_GoMuzzle, Vector3.zero, Quaternion.identity, transform) as GameObject;
                m_GoMuzzleInstance.transform.localScale = Vector3.one;
                Common.Utils.SetChildrenLayer(m_GoMuzzleInstance, gameObject.layer);
            }
            m_GoMuzzleInstance.SetActive(true);
            ParticleSystem muzzleParticleSystem = m_GoMuzzleInstance.GetComponent<ParticleSystem>();
            
            float during = 1;
            if (muzzleParticleSystem != null)
                during = muzzleParticleSystem.main.duration;
            StartCoroutine(HideObjectDelay(m_GoMuzzleInstance,during));
        }
        IEnumerator HideObjectDelay(GameObject obj, float during)
        {
            yield return new WaitForSeconds(during);
            if (obj != null)
                obj.SetActive(false);
        }
        void PlayExplosion(Components.Common.Transform2D bulletTransform,Components.Common.Bullet bullet)
        {
            if(m_LifeStateRender==0)
            {
                m_LifeStateRender = 1;
                m_GoExplosionInstance = GameObject.Instantiate(m_GoExplosion,
                    new Vector3(bulletTransform.Position.x.AsFloat(), 0, bulletTransform.Position.y.AsFloat()) , Quaternion.identity,transform) as GameObject;
                m_GoExplosionInstance.transform.localScale = Vector3.one*0.5f;
                Common.Utils.SetChildrenLayer(m_GoExplosionInstance, gameObject.layer);

                StartCoroutine(HideObjectDelay(m_GoExplosionInstance,1));
                m_LifeStateRender = 2;
                m_GoBulletInstance.SetActive(false);
            }           
        }

        private void OnDisable()
        {
            if(m_GoBulletInstance!=null)m_GoBulletInstance.SetActive(false);
        }
        protected override void OnRender()
        {
            base.OnRender();
            if (m_GoBulletInstance!=null&& !m_GoBulletInstance.activeSelf)
            {
                m_GoBulletInstance.SetActive(true);
                PlayMuzzle();
            }
          
            Components.Common.Transform2D com_Pos = m_Simulation.GetEntityWorld().GetComponentByEntityId<Components.Common.Transform2D>(EntityId);
            Components.Common.Movement2D com_Move = m_Simulation.GetEntityWorld().GetComponentByEntityId<Components.Common.Movement2D>(EntityId);
            Components.Common.Bullet com_Bullet = m_Simulation.GetEntityWorld().GetComponentByEntityId<Components.Common.Bullet>(EntityId);
            
            if (com_Pos != null && com_Move!=null)
            {
                var dir = com_Move.Dir;
                var pos1 = com_Pos.Position;
                transform.position = Vector3.Lerp(transform.position, new Vector3(pos1.x.AsFloat(),0, pos1.y.AsFloat()),0.5f);
                if (dir != TSVector2.zero)
                {
                    transform.forward = Vector3.Lerp(transform.forward, new Vector3(dir.x.AsFloat(), 0, dir.y.AsFloat()).normalized, 0.25f);
                }
            }
            else
            {
                ModuleManager.GetModule<PoolModule>().Recycle(GetComponent<PoolObject>().GetFullName(), gameObject);
            }
        }
    }
}