using UnityEngine;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Config.Static;
using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Renderer;
using Synchronize.Game.Lockstep.Ecsr.Components.Star;
using Synchronize.Game.Lockstep.MapEditor;

/// <summary>
/// 生成Entity
/// </summary>
namespace Synchronize.Game.Lockstep.Managers
{
    public class EntitySpawnModule : MonoBehaviour, IModule
    {
        public class CreateResourceRequest
        {
            public Misc.EntityType Type;
            public uint EntityId;
            public int ConfigId;
            public int ResourceId;
            public bool IsSelfPlayerEntity;
            public object Data;
            public override string ToString()
            {
                return $"[{GetType().Name}] Type:{Type} EntityId:{EntityId} ConfigId:{ConfigId} ResourceId:{ResourceId} IsSelfPlayerEntity:{IsSelfPlayerEntity}";
            }
        }
        public class MapCreateResourceRequest : CreateResourceRequest
        {
            public bool IsEditorMode;
            public EntityWorld World;
            public object Info;

            public override string ToString()
            {
                return base.ToString() + $" IsEditorMode:{IsEditorMode} Info:{Info.ToString()}";
            }
        }
        public void Init()
        {

        }
        GameContentRootModule m_GameContentRootModule;
        PoolModule m_PoolModule;
        ConfigModule m_ConfigModule;
        void Start()
        {
            m_GameContentRootModule = ModuleManager.GetModule<GameContentRootModule>();
            m_ConfigModule = ModuleManager.GetModule<ConfigModule>();
            m_PoolModule = ModuleManager.GetModule<PoolModule>();
        }
        public void CreateGameObject(CreateResourceRequest value)
        {
            Handler.Run((obj) =>
            {
                EntityWorld world = null;
                CreateResourceRequest creation = obj as CreateResourceRequest;
                GameObject go = null;
                ResourceIdCFG resCfg = null;
                switch (creation.Type)
                {
                    case EntityType.Bullet:
                        world = SimulationManager.Instance.GetSimulation().GetEntityWorld();
                        Transform2D bulletTransform = world.GetComponentByEntityId<Transform2D>(creation.EntityId);
                        if (bulletTransform != null)
                        {
                            resCfg = m_ConfigModule.GetConfig<ResourceIdCFG>(creation.ResourceId);
                            m_PoolModule.CreatePoolIfNotExist(resCfg.Path);
                            go = m_PoolModule.Reuse(resCfg.Path);
                            var bulletRender = go.GetComponent<BulletActionRender>();
                            bulletRender.SetEntityId(creation.EntityId);
                            go.name = string.Format("[Name:Bullet-{0} Id:{1}]", creation.ResourceId, creation.EntityId);
                            go.transform.position = new Vector3(bulletTransform.Position.x.AsFloat(), 0, bulletTransform.Position.y.AsFloat());
                        }

                        break;
                    case EntityType.Star:
                        resCfg = m_ConfigModule.GetConfig<ResourceIdCFG>(creation.ResourceId);
                        m_PoolModule.CreatePoolIfNotExist(resCfg.Path);
                        go = m_PoolModule.Reuse(resCfg.Path);
                        MapCreateResourceRequest mapCreateResourceVO = creation as MapCreateResourceRequest;
                        world = mapCreateResourceVO.World;
                        var starCfg = m_ConfigModule.GetConfig<MapElementCFG>(creation.ConfigId);
                        var starInfo = mapCreateResourceVO.Info as MapEditor.StarObjectInfo;
                        go.name = string.Format("[Name:{0} Id:{1}]", starCfg.Name, creation.EntityId);
                        go.transform.localScale = starCfg.Diameter * Vector3.one / (float)resCfg.ModelScaleRate;
                        go.SetActive(starInfo.m_Visable);
                        if (mapCreateResourceVO.IsEditorMode)
                        {
                            var starRender = go.GetComponent<StarObjectRender>();
                            if (starRender == null)
                                starRender = go.AddComponent<StarObjectRender>();
                            starRender.World = world;
                            starRender.EntityId = creation.EntityId;
                        }
                        else
                        {
                            var starPlayingRender = go.GetComponent<StarMoveActionRenderer>();
                            if (starPlayingRender == null)
                                starPlayingRender = go.AddComponent<StarMoveActionRenderer>();
                            starPlayingRender.SetEntityId(creation.EntityId);
                        }
                        break;
                    case EntityType.Asteroid:
                        resCfg = m_ConfigModule.GetConfig<ResourceIdCFG>(creation.ResourceId);
                        m_PoolModule.CreatePoolIfNotExist(resCfg.Path);
                        go = m_PoolModule.Reuse(resCfg.Path);
                        MapCreateResourceRequest mapCreateResourceVOBelt = creation as MapCreateResourceRequest;
                        world = mapCreateResourceVOBelt.World;
                        var beltInfo = mapCreateResourceVOBelt.Info as AsteroidBeltInfo;
                        go.name = string.Format("[Name:{0} Id:{1}]", beltInfo.m_AsteroidBeltName, creation.EntityId);
                        go.transform.localScale = Vector3.one / (float)resCfg.ModelScaleRate;
                        if (mapCreateResourceVOBelt.IsEditorMode)
                        {
                            var beltRender = go.GetComponent<StarObjectRender>();
                            if (beltRender == null)
                                beltRender = go.AddComponent<StarObjectRender>();
                            beltRender.World = world;
                            beltRender.EntityId = creation.EntityId;
                        }
                        else
                        {
                            var beltPlayingRender = go.GetComponent<StarMoveActionRenderer>();
                            if (beltPlayingRender == null)
                                beltPlayingRender = go.AddComponent<StarMoveActionRenderer>();
                            beltPlayingRender.SetEntityId(creation.EntityId);
                        }
                        break;
                    case EntityType.Player:
                        resCfg = m_ConfigModule.GetConfig<ResourceIdCFG>(creation.ResourceId);
                        m_PoolModule.CreatePoolIfNotExist(resCfg.Path);
                        go = m_PoolModule.Reuse(resCfg.Path);
                        MoveActionRenderer renderers = go.GetComponent<MoveActionRenderer>();
                        if (renderers == null)
                            renderers = go.AddComponent<MoveActionRenderer>();
                        renderers.SetEntityId(creation.EntityId);
                        go.name = string.Format("[Name:Player-{0} Id:{1}]", creation.ResourceId, creation.EntityId);
                        go.transform.SetParent(transform);
                        go.transform.localPosition = new Vector3(0, 0, 0);
                        GameEnvironment.Instance.SetCameraFollow(go.transform, creation.IsSelfPlayerEntity);

                        if (creation.IsSelfPlayerEntity)
                            m_GameContentRootModule.Trajectory.SetEntityId(creation.EntityId);
                        break;
                    case EntityType.BackgroudCamera:
                        var backgroudCamera = GameEnvironment.Instance.m_BackgroundCamera.GetComponent<BackgroundCameraRotation>();
                        backgroudCamera.SetEntityId(creation.EntityId);

                        break;
                    case EntityType.BrokenPiece:
                        resCfg = m_ConfigModule.GetConfig<ResourceIdCFG>(creation.ResourceId);
                        m_PoolModule.CreatePoolIfNotExist(resCfg.Path);
                        go = m_PoolModule.Reuse(resCfg.Path);
                        BrokenPiecePlanetRenderer brokenPiece = go.GetComponent<BrokenPiecePlanetRenderer>();
                        if (brokenPiece == null)
                            brokenPiece = go.AddComponent<BrokenPiecePlanetRenderer>();
                        go.name = string.Format("[Name:Broken-{0} Id:{1}]", creation.ResourceId, creation.EntityId);
                        go.transform.SetParent(transform);
                        brokenPiece.SetEntityId(creation.EntityId);
                        brokenPiece.transform.localScale = Vector3.one * (float)resCfg.ModelScaleRate;
                        brokenPiece.SetPieceCount(byte.Parse(creation.Data.ToString()));
                        break;
                }

                if (m_GameContentRootModule != null)
                    m_GameContentRootModule.AddChild(go);
            }, value);
        }
    }
}