using System.Reflection.Emit;
using Config.Static;
using Entitas;
using EntitySystems;
using Evt;
using LogicFrameSync.Src.LockStep.Behaviours;
using Managers;
using Misc;
using Net.Pt;
using NetServiceImpl;
using NetServiceImpl.OnlineMode.Gate;
using NetServiceImpl.OnlineMode.Room;
using Proxy;
using Src.Log;
using Src.Replays;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Data;
using UnityEngine;
using Localization;

namespace LogicFrameSync.Src.LockStep
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
            EventMgr<LoadingPanel.EventType, LoadingPanel.LoadingInfo>.TriggerEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Language.GetText(29), 0.75f));          
            EventMgr<LoadingPanel.EventType, LoadingPanel.LoadingInfo>.TriggerEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Language.GetText(30), 0.9f));
        }
        

       
        async public static void Stop()
        {
            SimulationManager.Instance.Stop();
            while (!SimulationManager.Instance.HasStopped)
                await System.Threading.Tasks.Task.Yield();
            var sim = SimulationManager.Instance.GetSimulation();            
            ReplayInfo replayInfo = new ReplayInfo();
            string name = ClientService.Get<RoomServices>().Session.Name;
            string path = PersistentDataPath + Const.LAST_REPLAY_FILENAME;

            replayInfo.MapId = uint.Parse(sim.GetEntityWorld().GetMeta (Meta.META_KEY_MAPID).ToString());
            replayInfo.MapVerificationCodes = (byte[])sim.GetEntityWorld().GetMeta(Meta.META_KEY_MAPASSET_HASH);
            replayInfo.Version = "1.0.0";
            replayInfo.InitEntityIds = (List<uint>)sim.GetEntityWorld().GetMeta(Meta.META_KEY_PLAYER_ENTITYIDS);
            replayInfo.Frames = sim.GetBehaviour<LogicFrameBehaviour>().GetFrameIdxInfos();

            await System.Threading.Tasks.Task.Run(()=> 
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
            await System.Threading.Tasks.Task.Delay(50);
            ClientService.Get<GateService>().Connect2GateServer();
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
                    ClientService.Get<NetServiceImpl.OnlineMode.Gate.GateService>().RequestLeaveRoom();
                    await System.Threading.Tasks.Task.Delay(50);
                }    
            }

            GameClientNetwork.Instance.CloseClient();
            await System.Threading.Tasks.Task.Delay(50);
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
            ModuleManager.GetModule<UIModule>().Push(Managers.UI.UITypes.LoadingPanel, Layer.Top, new LoadingPanel.LoadingInfo(Language.GetText(27), 0));
                                 
            replaySim.GetBehaviour<ReplayLogicFrameBehaviour>().SetFrameIdxInfos(replayInfo.Frames);
            MapIdCFG mapCfg = ModuleManager.GetModule<Managers.Config.ConfigModule>()
                .GetConfig<MapIdCFG>((int)replayInfo.MapId);
            await EntityManager.CreateMapEntity(replaySim.GetEntityWorld(),mapCfg.ResKey);
            replayInfo.InitEntityIds.ForEach(entityId => 
                EntityManager.CreatePlayerEntity(replaySim.GetEntityWorld(),entityId,false));
            EventMgr<LoadingPanel.EventType, LoadingPanel.LoadingInfo>.TriggerEvent(LoadingPanel.EventType.UpdateLoading, new LoadingPanel.LoadingInfo(Language.GetText(28), 1f));   
            await System.Threading.Tasks.Task.Delay(1000);
            ModuleManager.GetModule<UIModule>().Pop( Layer.Top);
            ModuleManager.GetModule<UIModule>().Push(Managers.UI.UITypes.BattlePanel, Layer.Bottom,PlayBattleMode.PlayReplayBattle); 
            
            SimulationManager.Instance.Start(DateTime.Now);
    
            
        }
        public async static void StopReplay()
        {
            SimulationManager.Instance.Stop();
            while (!SimulationManager.Instance.HasStopped)
                await System.Threading.Tasks.Task.Yield();
            
            Handler.Run(_=>DialogBox.Show(Language.GetText(22),Language.GetText(26),DialogBox.SelectType.Confirm, option=>{
                ModuleManager.GetModule<GameContentRootModule>().DestroyChildren();
                ModuleManager.GetModule<PoolModule>().Clear();
                SimulationManager.Instance.RemoveSimulation();
                ModuleManager.GetModule<UIModule>().Pop(Layer.Bottom);
            }),null);
        }
        #endregion
    }
}
