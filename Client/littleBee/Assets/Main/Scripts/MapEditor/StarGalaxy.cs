using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using Synchronize.Game.Lockstep.Ecsr.Systems;
using Synchronize.Game.Lockstep.Ecsr.Renderer;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.Config.Static;
using Synchronize.Game.Lockstep.Ecsr.Components.Common;

namespace Synchronize.Game.Lockstep.MapEditor
{
    public class StarGalaxy : MonoBehaviour
    {
        public enum Mode
        { 
            Edit,
            View,
            Game,
        }
        public bool m_EnableDrawGizmos;
        public Mode m_Mode = Mode.Edit;

        public string m_GalaxyName = "";
        public int m_MapWidth = 300;
        public int m_MapHeight = 300;
        public TextAsset m_ImportMapStream;
        EntityWorld world;
        List<IEntitySystem> m_Systems = new List<IEntitySystem> ();
        StarMovingSystem movingSystem;

        private void Awake()
        {
            if(Mode.Edit == m_Mode)
            {
                world = EntityWorld.Create();
                movingSystem = new StarMovingSystem();
                movingSystem.World = world;

                m_Systems.Add(movingSystem);
            }

        }
        public void SetEntityWorld(EntityWorld w)
        {
            world = w;

        }
        void Start()
        {
            
        }
        
        public void AddGamePrefab()
        {
            var stars = gameObject.GetComponents<StarObject>();
            List<StarObjectInfo> starInfos = new List<StarObjectInfo>();
            foreach(var star in stars)
            {
                starInfos.Add(star.m_Info);
            }
            var asteroids = gameObject.GetComponents<AsteroidBelt>();
            List<AsteroidBeltInfo> beltInfos = new List<AsteroidBeltInfo>();
            foreach(var ast in asteroids)
            {
                beltInfos.Add(ast.m_Info);
            }

            EntityManager.CreateMapEntity(world, starInfos, beltInfos,true);
        }
       
        void CreateGO(UnityEngine.Object templateGo,string name, uint entityId,bool visiable, Vector3 size)
        {
            var go = Instantiate(templateGo) as GameObject;
            go.name = string.Format("[Name:{0} Id:{1}]",name, entityId);
            go.transform.localScale = size;
            if(m_Mode == Mode.Edit)
            {
                var render = go.AddComponent<StarObjectRender>();
                render.World = world;
                render.EntityId = entityId;
            }
            else
            {
                var render = go.AddComponent<StarMoveActionRenderer>();
                render.SetEntityId(entityId);
                render.transform.SetParent(transform);               
            }
          
            go.SetActive(visiable);
            go.transform.SetParent(transform);
        }
        void Update()
        {
            if(Mode.Edit == m_Mode)
            {
                movingSystem.Execute();
            }

        }

        public StarGalaxyInfo AddImportMaySteam()
        {
            if(m_ImportMapStream!=null)
            {
                StarGalaxyInfo info = LitJson.JsonMapper.ToObject<StarGalaxyInfo>(m_ImportMapStream.text);
                m_GalaxyName = info.GalaxyName;
                m_MapWidth = info.MapWidth;
                m_MapHeight = info.MapHeight;
                foreach (var starInfo in info.Stars)
                {
                    StarObject starObj = gameObject.AddComponent<StarObject>();
                    starObj.m_Info = starInfo;
                }

                foreach (var asteriod in info.Belts)
                {
                    AsteroidBelt belt = gameObject.AddComponent<AsteroidBelt>();
                    belt.m_Info = asteriod;
                }
                return info;
            }
            return null;
        }
        private void OnDrawGizmos() 
        {
            if (!m_EnableDrawGizmos) return;
            if (m_Mode == Mode.Game) return;
            if(world!=null)
            {
                Gizmos.color = new Color(1, 0, 1);
                Vector2 size = new Vector2(m_MapWidth, m_MapHeight);
                DrawRect(Vector2.zero, size);
                

                world.ForEachComponent<Synchronize.Game.Lockstep.Ecsr.Components.Star.StarObjectInfo>((starObj) => 
                {
                    var config = ModuleManager.GetModule<ConfigModule>().GetConfig<MapElementCFG>(starObj.ConfigId);
                    var pos = world.GetComponentByEntityId<Transform2D>(starObj.EntityId);
                    if (config != null)
                        Gizmos.DrawWireSphere(new Vector3(pos.Position.x.AsFloat(), 0, pos.Position.y.AsFloat()), config.Diameter * 0.51f);
                    else
                        Gizmos.DrawWireSphere(new Vector3(pos.Position.x.AsFloat(), 0, pos.Position.y.AsFloat()), 1 * 0.51f);
                });
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
    }
}