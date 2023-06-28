
using Net.Pt;
using Synchronize.Game.Lockstep.Config.Static;
using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Components.Star;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.MapEditor;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Replays;
using System;
using System.Collections;
using System.Collections.Generic;
using TrueSync;
using TrueSync.Collision;

namespace Synchronize.Game.Lockstep.Ecsr.Entitas
{
    public class EntityManager
    {
        public const uint DEFAULT_ENTITY_ID_BACKGROUNDCAMERA = uint.MaxValue;
        #region CreateEntity
        public static void CreatePlayerEntity(EntityWorld world,uint entityId,bool isSelf)
        {      
            Components.Common.Movement2D move = new Components.Common.Movement2D(0.6f, TSVector2.zero);
            move.EntityId = entityId;
            Components.Common.Transform2D trans = new Components.Common.Transform2D(TSVector2.zero, 1, 1);
            trans.EntityId = entityId;

            Components.Common.HudInfo hudInfo = new HudInfo();
            hudInfo.EntityId = entityId;

            Hp hp = new Hp(5000);
            hp.EntityId = entityId;


            Defence defence = new Defence(1,DefenceType.Heavyweight);
            defence.EntityId = entityId;
            

            world.AddComponent(move);
            world.AddComponent(trans);
            world.AddComponent(hudInfo);
            world.AddComponent(hp);
            world.AddComponent(defence);
            ModuleManager.GetModule<EntitySpawnModule>().CreateGameObject(new EntitySpawnModule.CreateResourceRequest() { Type = Misc.EntityType.Player, EntityId = entityId, IsSelfPlayerEntity = isSelf, ResourceId = 999 });
            world.SortComponents();
        }
        public static System.Threading.Tasks.Task  CreateMapEntity(EntityWorld world,string mapName)
        {
            return Handler.WaitRunCompleteTask((value) => 
            {
                string mapStringAsset = UnityEngine.Resources.Load<UnityEngine.TextAsset>("Configs/Maps/" + mapName).text;
                StarGalaxyInfo galaxyInfo = LitJson.JsonMapper.ToObject<StarGalaxyInfo>(mapStringAsset);
                world.SetMeta(Meta.META_KEY_MAPASSET_HASH,ReplayInfo.ComputeHash(mapStringAsset));
                if (null != galaxyInfo)
                {
                    NineBlockBoxCollision nbbCollision = new NineBlockBoxCollision(15, galaxyInfo.MapWidth, galaxyInfo.MapHeight);
                    world.SetCollisionProvider(nbbCollision);
                    CreateMapEntity(world, galaxyInfo.Stars, galaxyInfo.Belts, false);
                }
                CreateBackgroundCameraEntity(world);
            }, null) ;           
        }
        public static void CreateBackgroundCameraEntity(EntityWorld world)
        {
            RotationParameter rotationParameter = new RotationParameter(new TSVector(1,1,1),0.01f);
            rotationParameter.EntityId = DEFAULT_ENTITY_ID_BACKGROUNDCAMERA;

            RotationValue value = new RotationValue(TSQuaternion.identity);
            value.EntityId = DEFAULT_ENTITY_ID_BACKGROUNDCAMERA;

            world.AddComponent(rotationParameter);
            world.AddComponent(value);
            ModuleManager.GetModule<EntitySpawnModule>().CreateGameObject(new EntitySpawnModule.CreateResourceRequest() { EntityId = DEFAULT_ENTITY_ID_BACKGROUNDCAMERA,Type = EntityType.BackgroudCamera});
            world.SortComponents();
        }

        public static void CreateBrokenPieceEntity(EntityWorld world, uint entityId,byte count,uint startEntityId,TSVector2 position)
        {
            TSRandom random = TSRandom.New((int)entityId);
            for (byte i=0;i<count;++i)
            {
                BrokenPiece brokenPiece = new BrokenPiece(entityId, i, i == 0 ) ;
                brokenPiece.EntityId = startEntityId + i;
                Countdown countdown = new Countdown(100+random.Next(-50,50));
                countdown.EntityId = brokenPiece.EntityId;
                world.AddComponent(countdown);

                Transform2D transformPiece = new Transform2D(position, 0.5f, 2); 
                transformPiece.EntityId = brokenPiece.EntityId;
                
                FP rad = i*((TSMath.Pi * 2) / count);
                Movement2D movementPiece = new Movement2D(new FP(random.Next(1, 10)) / 100, new TSVector2(TSMath.Cos(rad),TSMath.Sin(rad)));
                movementPiece.EntityId = brokenPiece.EntityId;

                RotationParameter rotationParameter = new RotationParameter(new TSVector(1, 1, 1), new FP(random.Next(1, 10)));
                rotationParameter.EntityId = brokenPiece.EntityId;

                RotationValue value = new RotationValue(TSQuaternion.identity);
                value.EntityId = brokenPiece.EntityId;
                Particle particle = new Particle(.1f);
                particle.EntityId = brokenPiece.EntityId;

                Bullet bullet = new Bullet(entityId);
                bullet.EntityId = brokenPiece.EntityId;
                Attack attack = new Attack(500, AttackType.Common);
                attack.EntityId = brokenPiece.EntityId;

                
                world.AddComponent(attack);
                world.AddComponent(bullet);
                world.AddComponent(particle);
                world.AddComponent(value);
                world.AddComponent(rotationParameter);
                world.AddComponent(movementPiece);
                world.AddComponent(transformPiece);
                world.AddComponent(brokenPiece);              
            }
            world.SortComponents();
            ModuleManager.GetModule<EntitySpawnModule>().CreateGameObject(new EntitySpawnModule.CreateResourceRequest()
            {
                Type = EntityType.BrokenPiece,
                EntityId = startEntityId,
                ResourceId = 100000,
                Data = 25,
            });
        }
        public static void CreateMapEntity(EntityWorld world, List<Synchronize.Game.Lockstep.MapEditor.StarObjectInfo> starInfos, List<AsteroidBeltInfo> beltInfos,bool isEditorMode)
        {
            foreach(Synchronize.Game.Lockstep.MapEditor.StarObjectInfo starInfo in starInfos)
            {
                var starCfg = ModuleManager.GetModule<ConfigModule>().GetConfig<MapElementCFG>(starInfo.m_ConfigId);
                world.AddComponent(new GravitationalField((byte)starCfg.Mass, (byte)(5 * starCfg.Diameter)) { EntityId=starInfo.m_EntityId});
                world.AddComponent(new Components.Common.Transform2D(TSVector2.zero, starCfg.Diameter / 2f) { EntityId=starInfo.m_EntityId});
                world.AddComponent(new StarObjectRotation() { EntityId=starInfo.m_EntityId,Speed= Convert.ToSingle(starInfo.m_RotationSpeed)});
                world.AddComponent(new Components.Star.StarObjectInfo() { EntityId = starInfo.m_EntityId,ConfigId = starInfo.m_ConfigId});
          
                if (starInfo.m_BrokenPieceCount >0 && starInfo.m_BrokenPieceStartId != 0)
                {
                    world.AddComponent(new Hp(600) { EntityId = starInfo.m_EntityId });
                    world.AddComponent(new Defence(100, DefenceType.Urbanweight) { EntityId = starInfo.m_EntityId });
                    world.AddComponent(new BreakableInfo() { EntityId = starInfo.m_EntityId, PieceCount = starInfo.m_BrokenPieceCount, PieceStartEntityId = starInfo.m_BrokenPieceStartId });
                }
                   
                if (Convert.ToSingle(starInfo.m_RevolutionSpeed) != 0)
                    world.AddComponent(new StarObjectRevolution() { EntityId = starInfo.m_EntityId,Degree = starInfo.m_InitRevolutionDegree,Speed = Convert.ToSingle(starInfo.m_RevolutionSpeed),Radius = starInfo.m_RevolutionRedius,ParentEntityId = starInfo.m_ParentEntityId});                
                ModuleManager.GetModule<EntitySpawnModule>().CreateGameObject(new EntitySpawnModule.MapCreateResourceRequest() { Type = Misc.EntityType.Star, EntityId = starInfo.m_EntityId, ConfigId = starInfo.m_ConfigId, ResourceId = starCfg.ResKey, World = world, Info = starInfo, IsEditorMode = isEditorMode });
            }

            foreach(AsteroidBeltInfo beltInfo in beltInfos)
            {
                int step = beltInfo.m_FarRadius - beltInfo.m_NearRadius;
                for(int s = 0;s<step;)
                {
                    int sIdStep = s * 1000;
                    for(int i=0;i<360;)
                    {
                        uint entityId = (uint)(sIdStep + beltInfo.m_StartEntityId + i);

                        world.AddComponent(new Transform2D(TSVector2.zero, 0.5f) { EntityId=entityId});
                        world.AddComponent(new StarObjectRotation() { EntityId=entityId,Speed = i%5 });
                        world.AddComponent(new Components.Star.StarObjectInfo() { EntityId=entityId,ConfigId=-1});
                        world.AddComponent(new Hp(600) { EntityId=entityId});
                        world.AddComponent(new Defence(100, DefenceType.Urbanweight) { EntityId=entityId});
                        if(Convert.ToSingle(beltInfo.m_RevolutionSpeed)!=0)
                        {
                            world.AddComponent(new StarObjectRevolution() { EntityId = entityId,Degree=i,Speed = Convert.ToSingle(beltInfo.m_RevolutionSpeed),Radius = beltInfo.m_NearRadius+s,ParentEntityId = beltInfo.m_ParentEntityId});
                        }
                        i += UnityEngine.Mathf.Max(1,beltInfo.m_Relaxation-beltInfo.m_Gradient*s);
                       
                        ModuleManager.GetModule<EntitySpawnModule>().CreateGameObject(new EntitySpawnModule.MapCreateResourceRequest() { Type = Misc.EntityType.Asteroid, EntityId = entityId, ConfigId = -1, ResourceId = 7, World = world, Info = beltInfo, IsEditorMode = isEditorMode });
                    }
                    s += beltInfo.m_ShellRelaxation;
                }
            }
            world.SortComponents();

        }
        public static void CreateEntityBySyncFrame(EntityWorld world,FrameIdxInfo info)
        {
            using (ByteBuffer buffer = new ByteBuffer(info.ParamsContent))
            {
                EntityType type = (EntityType)buffer.ReadByte();
                uint newEntityId = buffer.ReadUInt32();
                Transform2D senderTransform = world.GetComponentByEntityId<Transform2D>(info.EntityId);               
                if (senderTransform != null)
                {              
                    switch(type)
                    {
                        case EntityType.Bullet:
                            Transform2D newTransform = new Transform2D(senderTransform.Position + senderTransform.Toward * 2f, 1f, byte.MaxValue);
                            newTransform.EntityId = newEntityId;
                            world.AddComponent(newTransform);
                            Movement2D newMove = new Movement2D(0.8f, senderTransform.Toward);
                            newMove.EntityId = newEntityId;
                            world.AddComponent(newMove);
                            Countdown countdown = new Countdown(500);
                            countdown.EntityId = newEntityId;
                            world.AddComponent(countdown);
                            Bullet bullet = new Bullet(info.EntityId);
                            bullet.EntityId = newEntityId;
                            world.AddComponent(bullet);
                            Particle particle = new Particle(1);
                            particle.EntityId = newEntityId;
                            world.AddComponent(particle);
                            Attack attack = new Attack(500,AttackType.Common);
                            attack.EntityId = newEntityId;
                            world.AddComponent(attack);
                            world.SortComponents();
                            ModuleManager.GetModule<EntitySpawnModule>().CreateGameObject(new EntitySpawnModule.CreateResourceRequest() { Type = (Misc.EntityType)type, EntityId = newEntityId, ConfigId = -1, ResourceId = 11 });
                            break;
                    }
                }
            }
        }
        #endregion
    }
}
