using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TrueSync.Collision;
using TrueSync;
using Synchronize.Game.Lockstep.Config.Static;
using Synchronize.Game.Lockstep.MapEditor;
using Synchronize.Game.Lockstep;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using Synchronize.Game.Lockstep.Ecsr.Components.Common;

namespace Synchronize.Game.Lockstep.DebugTools
{
    /// <summary>
    /// 四叉树可视化调试
    /// </summary>
    public class CollisionViewDebug : MonoBehaviour
    {
        [Range(0.5f, 1)]
        public float m_VisualLinethickness = 0.51f;
        public TextAsset m_TxtMapConfig;
        public TextAsset m_TxtMapSource;
        List<MapElementCFG> mapElementsConfigs;
        StarGalaxyInfo galaxyInfo;

        // Use this for initialization
        void Start()
        {
            mapElementsConfigs = LitJson.JsonMapper.ToObject<List<MapElementCFG>>(m_TxtMapConfig.text);
            galaxyInfo = LitJson.JsonMapper.ToObject<StarGalaxyInfo>(m_TxtMapSource.text);
        }

        private void OnDrawGizmos()
        {
            if (galaxyInfo == null) return;
            Gizmos.color = new Color(1, 0, 1);
            Vector2 size = new Vector2(galaxyInfo.MapWidth, galaxyInfo.MapHeight);
            DrawRect(Vector2.zero, size);
            if (mapElementsConfigs == null) return;

            var simulation = SimulationManager.Instance.GetSimulation();

            if (simulation != null)
            {
                var world = simulation.GetEntityWorld();
                if (world != null)
                {
                    lock (EntityWorld.SyncRoot)
                    {
                        world.ForEachComponent<Transform2D>(transform =>
                        {
                            Gizmos.DrawWireSphere(new Vector3(transform.Position.x.AsFloat(), 0, transform.Position.y.AsFloat()), transform.Radius.AsFloat());
                        });

                        ICollisionProvider collisionProvider = world.GetCollisionProvider();
                        if (collisionProvider != null)
                        {
                            collisionProvider.ForEachBlock((box) =>
                            {
                                Gizmos.color = new Color(0, 1 - Mathf.Min(1, box.GetCount() / 5f), 0);
                                DrawRect(new Vector2(box.Rect.x.AsFloat() + box.Rect.width.AsFloat() / 2, box.Rect.y.AsFloat() + box.Rect.height.AsFloat() / 2), Vector2.one * world.GetCollisionProvider().BlockSize.AsInt());
                            });
                        }
                    }
                }
            }
        }

        void DrawRect(Vector2 centerPos, Vector2 size)
        {
            Rect rect = new Rect(centerPos - size / 2, size);
            Gizmos.DrawLine(new Vector3(rect.xMin, 0, rect.yMin), new Vector3(rect.xMin, 0, rect.yMax));
            Gizmos.DrawLine(new Vector3(rect.xMin, 0, rect.yMax), new Vector3(rect.xMax, 0, rect.yMax));
            Gizmos.DrawLine(new Vector3(rect.xMax, 0, rect.yMax), new Vector3(rect.xMax, 0, rect.yMin));
            Gizmos.DrawLine(new Vector3(rect.xMax, 0, rect.yMin), new Vector3(rect.xMin, 0, rect.yMin));
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}