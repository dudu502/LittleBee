using Net.Pt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using Synchronize.Game.Lockstep.Behaviours;
using Synchronize.Game.Lockstep.Ecsr.Systems;
using Synchronize.Game.Lockstep.UI;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.Config.Static;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Managers.UI;
using Synchronize.Game.Lockstep.Replays;
using Synchronize.Game.Lockstep.Evt;
using Synchronize.Game.Lockstep.Net;
using Synchronize.Game.Lockstep.Notification;
using Synchronize.Game.Lockstep.Proxy;
using Synchronize.Game.Lockstep.Room;
using Synchronize.Game.Lockstep.Gate;

namespace Synchronize.Game.Lockstep
{
    public class BattleEntryPoint
    {
        public static string PersistentDataPath;

        #region Client Game Process
        public static Simulation CreateClientSimulation()
        {
            try
            {
                if (SimulationManager.Instance.GetSimulation() != null)
                {
                    throw new Exception($"Simulation {Const.CLIENT_SIMULATION_ID} is exist!");
                }

                Simulation sim = new Simulation(Const.CLIENT_SIMULATION_ID);
                sim.SetEntityWorld(EntityWorld.Create());
                sim.AddBehaviour(new LogicFrameBehaviour())
                    .AddBehaviour(new RollbackBehaviour())
                    .AddBehaviour(new EntityBehaviour())
                    .AddBehaviour(new KeyboardMovementInputBehaviour())
                    .AddBehaviour(new JoystickMovementInputBehaviour())
                    .AddBehaviour(new FunctionButtonInputBehaviour())
                    .AddBehaviour(new ComponentsBackupBehaviour());

                FsmSystem fsmSystem = new FsmSystem();
                EntityMoveSystem moveSystem = new EntityMoveSystem();
                StarMovingSystem starMovingSystem = new StarMovingSystem();
                NineBlockBoxCollisionSystem collisionSystem = new NineBlockBoxCollisionSystem();
                MacroWarSystem macroWarSystem = new MacroWarSystem();
                MicroWarSystem microWarSystem = new MicroWarSystem();
                StrategyWarSystem strategyWarSystem = new StrategyWarSystem();
                BrokenPieceSystem brokenPieceSystem = new BrokenPieceSystem();
                RemoveEntitySystem removeSystem = new RemoveEntitySystem();
                GravitationalSystem gravitySystem = new GravitationalSystem();
                RotationSystem rotationSystem = new RotationSystem();
                sim.GetBehaviour<EntityBehaviour>()
                    .AddSystem(fsmSystem)
                    .AddSystem(gravitySystem)
                    .AddSystem(moveSystem)
                    .AddSystem(starMovingSystem)
                    .AddSystem(collisionSystem)
                    .AddSystem(macroWarSystem)
                    .AddSystem(microWarSystem)
                    .AddSystem(strategyWarSystem)
                    .AddSystem(brokenPieceSystem)
                    .AddSystem(rotationSystem)
                    .AddSystem(removeSystem);
                sim.GetBehaviour<RollbackBehaviour>()
                    .AddSystem(fsmSystem)
                    .AddSystem(gravitySystem)
                    .AddSystem(moveSystem)
                    .AddSystem(starMovingSystem)
                    .AddSystem(collisionSystem)
                    .AddSystem(macroWarSystem)
                    .AddSystem(microWarSystem)
                    .AddSystem(strategyWarSystem)
                    .AddSystem(brokenPieceSystem)
                    .AddSystem(rotationSystem)
                    .AddSystem(removeSystem);
                SimulationManager.Instance.SetSimulation(sim);
                return sim;
            }
            catch(Exception exc)
            {
                Debug.LogError(exc.ToString()) ;
            }
            return null;
        }
        public static void Start(PtLaunchGameData ptLaunchGameData,string ip)
        {
            GameEnvironment.Instance.SetState(GameEnvironment.State.InBattle);
            ConnectRoom(ip, ptLaunchGameData);
            EventMgr<LoadingPanel.EventType, LoadingPanel.LoadingInfo>.TriggerEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Localization.Localization.GetTranslation("Connecting to Room Server"), 0.75f));          
            EventMgr<LoadingPanel.EventType, LoadingPanel.LoadingInfo>.TriggerEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Localization.Localization.GetTranslation("Initializing"), 0.9f));
        }
        

       
        async public static void Stop()
        {
            SimulationManager.Instance.Stop();
            while (!SimulationManager.Instance.NeedStop)
                await Task.Yield();
            var sim = SimulationManager.Instance.GetSimulation();            
            ReplayInfo replayInfo = new ReplayInfo();
            string name = DataProxy.Get<RoomServiceProxy>().Session.Name;
            string path = PersistentDataPath + Const.LAST_REPLAY_FILENAME;

            replayInfo.MapId = uint.Parse(sim.GetEntityWorld().GetMeta (Meta.META_KEY_MAPID).ToString());
            replayInfo.MapVerificationCodes = (byte[])sim.GetEntityWorld().GetMeta(Meta.META_KEY_MAPASSET_HASH);
            replayInfo.Version = "1.0.0";
            replayInfo.InitEntityIds = (List<uint>)sim.GetEntityWorld().GetMeta(Meta.META_KEY_PLAYER_ENTITYIDS);
            replayInfo.Frames = sim.GetBehaviour<LogicFrameBehaviour>().GetFrameIdxInfos();

            await Task.Run(()=> 
            {
                var bytesTask = ReplayInfo.Write(replayInfo);
                bytesTask.Wait();
                File.WriteAllBytes(path, bytesTask.Result);
                Debug.Log("create replay complete " + path);
            });
          
            ModuleManager.GetModule<GameContentRootModule>().DestroyChildren();
            ModuleManager.GetModule<PoolModule>().Clear();
            SimulationManager.Instance.RemoveSimulation();
            NetServiceImpl.Server.StandaloneLocalServer.Stop();
            GameClientNetwork.Instance.CloseClient();
            await Task.Yield();
            DataProxy.Get<GateServiceProxy>().Connect2GateServer();
        }
        static async void ConnectRoom(string address, PtLaunchGameData ptLaunchGameData)
        {
            int standaloneModePort = 0;
            if (ptLaunchGameData.IsStandaloneMode)
            {
                if(int.TryParse(UserSettingMgr.SettingList.Find(s => s.m_SettingTitle == UserSettingMgr.LAN_SERVER_PORT).m_SettingValue,out standaloneModePort)){
                    if(standaloneModePort == 0)
                    {
                        Debug.LogError("port error");
                        return;
                    }
                    NetServiceImpl.Server.StandaloneLocalServer.Start("Standalone", standaloneModePort, ptLaunchGameData.MapId, ptLaunchGameData.PlayerNumber);//fortest
                    DataProxy.Get<GateServiceProxy>().RequestLeaveRoom();
                    await Task.Yield();
                }    
            }

            GameClientNetwork.Instance.CloseClient();
            await Task.Yield();
            GameClientNetwork.Instance.Launch();
            if(!ptLaunchGameData.IsStandaloneMode)
                GameClientNetwork.Instance.Start(address, ptLaunchGameData.RSPort, ptLaunchGameData.ConnectionKey);
            else           
                GameClientNetwork.Instance.Start("127.0.0.1", standaloneModePort, "Standalone");              
        }
        #endregion

        #region Replays
        public static Simulation CreateReplaySimulation()
        {
            if (SimulationManager.Instance.GetSimulation() != null)
            {
                throw new Exception($"Simulation {Const.REPLAY_SIMULATION_ID} is exist!");
            }
            Simulation sim = new Simulation(Const.REPLAY_SIMULATION_ID);
            sim.SetEntityWorld(EntityWorld.Create());
            sim.AddBehaviour(new ReplayLogicFrameBehaviour())
                .AddBehaviour(new ReplayInputBehaviour())
                .AddBehaviour(new EntityBehaviour());
            FsmSystem fsmSystem = new FsmSystem();
            EntityMoveSystem moveSystem = new EntityMoveSystem();
            StarMovingSystem starMovingSystem = new StarMovingSystem();
            NineBlockBoxCollisionSystem collisionSystem = new NineBlockBoxCollisionSystem();
            MacroWarSystem macroWarSystem = new MacroWarSystem();
            MicroWarSystem microWarSystem = new MicroWarSystem();
            StrategyWarSystem strategyWarSystem = new StrategyWarSystem();
            BrokenPieceSystem brokenPieceSystem = new BrokenPieceSystem();
            RemoveEntitySystem removeSystem = new RemoveEntitySystem();
            GravitationalSystem gravitySystem = new GravitationalSystem();
         
            RotationSystem rotationSystem = new RotationSystem();
            sim.GetBehaviour<EntityBehaviour>()
                .AddSystem(fsmSystem)
                .AddSystem(gravitySystem)
                .AddSystem(moveSystem)
                .AddSystem(starMovingSystem)
                .AddSystem(collisionSystem)
                .AddSystem(macroWarSystem)
                .AddSystem(microWarSystem)
                .AddSystem(strategyWarSystem)
                .AddSystem(brokenPieceSystem)
                .AddSystem(rotationSystem)
                .AddSystem(removeSystem);
            SimulationManager.Instance.SetSimulation(sim);
            return sim;
        }
        public async static void StartReplay(Simulation replaySim, ReplayInfo replayInfo)
        {
            GameEnvironment.Instance.SetState(GameEnvironment.State.InBattle);
            ModuleManager.GetModule<UIModule>().Push(UITypes.LoadingPanel, Layer.Top, new LoadingPanel.LoadingInfo(Localization.Localization.GetTranslation("Loading"), 0));
            await Task.Yield();
            replaySim.GetBehaviour<ReplayLogicFrameBehaviour>().SetFrameIdxInfos(replayInfo.Frames);
            MapIdCFG mapCfg = ModuleManager.GetModule<ConfigModule>()
                .GetConfig<MapIdCFG>((int)replayInfo.MapId);
            await EntityManager.CreateMapEntity(replaySim.GetEntityWorld(),mapCfg.ResKey);
            replayInfo.InitEntityIds.ForEach(entityId => 
                EntityManager.CreatePlayerEntity(replaySim.GetEntityWorld(),entityId,false));
            EventMgr<LoadingPanel.EventType, LoadingPanel.LoadingInfo>.TriggerEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Localization.Localization.GetTranslation("Load complete"), 1f));

            await Task.Yield();
            ModuleManager.GetModule<UIModule>().Pop(Layer.Top);
            ModuleManager.GetModule<UIModule>().Push(UITypes.BattlePanel, Layer.Bottom,PlayBattleMode.PlayReplayBattle); 
            
            SimulationManager.Instance.Start(DateTime.Now);
        }
        public static void StopReplay()
        {
            SimulationManager.Instance.Stop();
            ModuleManager.GetModule<GameContentRootModule>().DestroyChildren();
            ModuleManager.GetModule<PoolModule>().Clear();
            SimulationManager.Instance.RemoveSimulation();
        }
        #endregion
    }
}
