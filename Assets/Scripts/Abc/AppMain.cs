using UnityEngine;
using UnityEngine.UI;
using LogicFrameSync.Src.LockStep;

using LogicFrameSync.Src.LockStep.Behaviours;

using System.IO;
using NetServiceImpl.Client.Data;
using NetServiceImpl;
using NetServiceImpl.Client;
using Src.Replays;
using Src.Log;
using Components;
using EntitySystems;
using Renderers;
using LogicFrameSync.Src.LockStep.Frame;
using System.Collections.Concurrent;
using Entitas;
using System.Collections.Generic;
using Unity.Mathematics;

public class AppMain : MonoBehaviour
{
    public enum Notifications
    {
        ReadyPlayerAndAdd,
    }
    public static AppMain INS;
    public Notify.Notifier notifier;
    public Button m_BtnAddClickSim;
    public Button m_BtnReady;
    public Button m_BtnAddEntity;
    public Button m_BtnStopSimulation;  
    public Button m_BtnPlayReplay;

    public Text m_TxtDebug;


    bool IsReplayMode = false;
    // Use this for initialization
    void Start()
    {
        notifier = new Notify.Notifier(this);
        INS = this;
        m_BtnAddClickSim.onClick.AddListener(OnClickAddClient);
        m_BtnAddEntity.onClick.AddListener(OnClickAddEntity);
        m_BtnReady.onClick.AddListener(OnClickReady);
        m_BtnStopSimulation.onClick.AddListener(OnClickStopSim);
        m_BtnPlayReplay.onClick.AddListener(OnClickPlayReplay);           
    }

   
 
    void OnClickReady()
    {
        Service.Get<LoginService>().RequestPlayerReady();
    }
    void OnClickAddEntity()
    {
        GameClientData.SelfControlEntityId = Common.Utils.GuidToString();
        KeyFrameSender.AddCurrentFrameCommand(FrameCommand.SYNC_CREATE_ENTITY, GameClientData.SelfControlEntityId, new string[] { ((int)EntityWorld.EntityOperationEvent.CreatePlayer)+"" });
    }

    [Notify.Subscribe(Notifications.ReadyPlayerAndAdd)]
    void OnReadyPlayerAndAdd(Notify.Notification note)
    {
        print("Ready RoleId "+note.Params[0]);
    }
   
  
    void OnClickPlayReplay()
    {        
        Simulation sim = new Simulation(Const.CLIENT_SIMULATION_ID);
        var bytes = File.ReadAllBytes(Application.dataPath + "/replay_client_-1821673472.rep");
        var info = ReplayInfo.Read(bytes);//Simulation.ReadReplay(ByteBuffer.Decompress(bytes));
        sim.AddBehaviour(new ReplayLogicFrameBehaviour());
        sim.AddBehaviour(new EntityBehaviour());
        sim.AddBehaviour(new ReplayInputBehaviour());
        sim.GetBehaviour<EntityBehaviour>().AddSystem(new EntityMoveSystem()).AddSystem(new AutoRemovingEntitySystem());
        sim.GetBehaviour<ReplayLogicFrameBehaviour>().SetFrameIdxInfos(info.Frames);     
         
        SimulationManager.Instance.AddSimulation(sim);
        SimulationManager.Instance.Start();
        IsReplayMode = true;
    }
    void OnClickStopSim()
    {
        SimulationManager.Instance.Stop();
        var sim = SimulationManager.Instance.GetSimulation(Const.CLIENT_SIMULATION_ID);
        ReplayInfo replayInfo = new ReplayInfo();
        replayInfo.OwnerId = GameClientData.SelfPlayer.Id;
        string path = Application.dataPath + "/" + string.Format("replay_client_{0}.rep", GameClientData.SelfPlayer.Id);
        replayInfo.Frames = sim.GetBehaviour<LogicFrameBehaviour>().GetFrameIdxInfos();
        var bytes = ReplayInfo.Write(replayInfo);
        File.WriteAllBytes(path, bytes);

        var frameData = sim.GetBehaviour<ComponentsBackupBehaviour>().GetEntityWorldFrameData();
        var outstring = GameEntityWorldLog.Write(frameData, GameClientData.SelfPlayer.Id);
        File.WriteAllText(Application.dataPath + "/" + string.Format("log_client_{0}.txt", GameClientData.SelfPlayer.Id)
            , outstring);


        Debug.Log("create replay "+path);

        SimulationManager.Instance.RemoveSimulation(sim);
    }
    

    void OnClickAddClient()
    {
        //add a client simulation 
        Simulation sim = new Simulation(Const.CLIENT_SIMULATION_ID);
        sim.AddBehaviour(new LogicFrameBehaviour());
        sim.AddBehaviour(new RollbackBehaviour());                
        sim.AddBehaviour(new EntityBehaviour());
        sim.AddBehaviour(new InputBehaviour());
        //sim.AddBehaviour(new TestRandomInputBehaviour());
        sim.AddBehaviour(new ComponentsBackupBehaviour());
        EntityMoveSystem moveSystem = new EntityMoveSystem();
        EntityCollisionSystem colliderSystem = new EntityCollisionSystem();
        AutoRemovingEntitySystem autoRemoveSystem = new AutoRemovingEntitySystem();
        sim.GetBehaviour<EntityBehaviour>().AddSystem(moveSystem).AddSystem(colliderSystem).AddSystem(autoRemoveSystem);
        sim.GetBehaviour<RollbackBehaviour>().AddSystem(moveSystem).AddSystem(colliderSystem).AddSystem(autoRemoveSystem);
        SimulationManager.Instance.AddSimulation(sim);
    }

   
    void Update()
    {
        if (!IsReplayMode)
        {
            Simulation sim = SimulationManager.Instance.GetSimulation(Const.CLIENT_SIMULATION_ID);
            if (sim == null) return;
            var world = sim.GetEntityWorld();
            if (world == null) return;
            if (!world.IsActive) return;
            string str = "";
            var entities = world.GetEntities();
            for (int i = 0; i < entities.Count; ++i)
            {
                var e = entities[i];
                if (!e.IsActive) continue;
                TransformComponent posComp = e.GetComponent<TransformComponent>();
                if (posComp != null)
                    str += string.Format("EntityId {0} Position:{1}", e.Id, posComp.ToString()) + "\n";
            }
            str += "Message count:" + Service.Get<LoginService>().KeyframesCount + "\n";
            str += "Keyframe count:" + Service.Get<LoginService>().AllFramesCount + "\n";
            str += "DebugRoll KeyframeIdx :" + sim.GetBehaviour<RollbackBehaviour>().DebugFrameIdx + "\n";
            str += "FrameIdx:" + sim.GetBehaviour<LogicFrameBehaviour>().CurrentFrameIdx;
            m_TxtDebug.text = str;
        }        
    }

    private void OnApplicationQuit()
    {
        SimulationManager.Instance.Stop();
    }
}

