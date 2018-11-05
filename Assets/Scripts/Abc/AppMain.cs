using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LogicFrameSync.Src.LockStep;
using System;

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

public class AppMain : MonoBehaviour
{
    public enum Notifications
    {
        InitPlayer,
        ReadyPlayerAndAdd,
    }
    public static AppMain INS;
    public Notify.Notifier notifier;
    public Button m_BtnAddClickSim;
    public Button m_BtnReady;
    public Button m_BtnStopSimulation;  
    public Button m_BtnPlayReplay;

    public Text m_TxtDebug;
    public Transform m_Content;
    // Use this for initialization
    void Start()
    {
        notifier = new Notify.Notifier(this);
        INS = this;
        m_BtnAddClickSim.onClick.AddListener(OnClickAddClient);
        m_BtnReady.onClick.AddListener(OnClickReady);
        m_BtnStopSimulation.onClick.AddListener(OnClickStopSim);
        m_BtnPlayReplay.onClick.AddListener(OnClickPlayReplay);           
    }

    void CreateNewEntityGO(int id)
    {
        //GameObject obj = GameObject.Instantiate(Resources.Load("EntityObj") as GameObject);
        //EntityGameObj objScript = obj.GetComponent<EntityGameObj>();

        //objScript.ID = id;
        GameObject obj = GameObject.Instantiate(Resources.Load("EntityObj") as GameObject);
        MoveActionRenderer renderers = obj.AddComponent<MoveActionRenderer>();
        renderers.SetEntityId(id);
        //foreach (ActionRenderer render in renderers)
        //    render.SetEntityId(id);
        obj.transform.SetParent(m_Content);
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);
    }
    void OnClickReady()
    {
        Service.Get<LoginService>().RequestPlayerReady();
    }

    [Notify.Subscribe(Notifications.ReadyPlayerAndAdd)]
    void OnReadyPlayerAndAdd(Notify.Notification note)
    {
        long id = (long)note.Params[0];  
        Simulation sim = SimulationManager.Instance.GetSimulation("client");
        sim.GetEntityWorld().AddEntity((int)id);
        sim.GetEntityWorld().GetEntity((int)id).
            AddComponent(new MoveComponent(20, Vector2.zero)).
            AddComponent(new PositionComponent(Vector2.zero));
        CreateNewEntityGO((int)id);
    }

    [Notify.Subscribe(Notifications.InitPlayer)]
    void OnInitPlayerHandler(Notify.Notification note)
    {
        /*
        long id = (long)note.Params[0];
        Simulation sim = SimulationManager.Instance.GetSimulation("client");
        sim.GetBehaviour<EntityBehaviour>().AddSystem(new EntityMoveSystem());
        sim.GetEntityWorld().AddEntity((int)id);
        sim.GetEntityWorld().GetEntity((int)id).
            AddComponent(new MoveComponent(10, Vector2.zero)).
            AddComponent(new PositionComponent(Vector2.zero));
        */
    }
    void OnClickPlayReplay()
    {
        /*
        Simulation sim = new Simulation("client");
        var bytes = File.ReadAllBytes(Application.dataPath + "/replay_client.rep");
        var info = Simulation.ReadReplay(ByteBuffer.Decompress(bytes));
        sim.AddBehaviour(new ReplayLogicFrameBehaviour());
        sim.AddBehaviour(new EntityBehaviour());
        sim.AddBehaviour(new ReplayInputBehaviour());
        sim.GetBehaviour<EntityBehaviour>().AddSystem(new EntityMoveSystem());
        sim.GetBehaviour<ReplayLogicFrameBehaviour>().SetFrameIdxInfos(info);
        sim.GetEntityWorld().AddEntity((int)GameClientData.SelfPlayer.Id );
        sim.GetEntityWorld().GetEntity((int)GameClientData.SelfPlayer.Id).
            AddComponent(new MoveComponent(10, Vector2.zero)).
            AddComponent(new PositionComponent(Vector2.zero));
         
        SimulationManager.Instance.AddSimulation(sim);
        */
    }
    void OnClickStopSim()
    {
        SimulationManager.Instance.Stop();
        var sim = SimulationManager.Instance.GetSimulation("client");
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
        Simulation sim = new Simulation("client");
        sim.AddBehaviour(new LogicFrameBehaviour());
        sim.AddBehaviour(new RollbackBehaviour());                
        sim.AddBehaviour(new EntityBehaviour());
        sim.AddBehaviour(new InputBehaviour());
        sim.AddBehaviour(new ComponentsBackupBehaviour());
        sim.GetBehaviour<EntityBehaviour>().AddSystem(new EntityMoveSystem());
        sim.GetBehaviour<RollbackBehaviour>().AddSystem(new EntityMoveSystem());
        SimulationManager.Instance.AddSimulation(sim);
    }

   
    void Update()
    {
        Simulation sim = SimulationManager.Instance.GetSimulation("client");
        if (sim == null) return;
        var world = sim.GetEntityWorld();
        if (world == null) return;
        if (world.GetEntities().Count == 0) return;
        string str = "";
        var entities = world.GetEntities();
        for(int i=0;i<entities.Count;++i)
        {
            var e = entities[i];
            PositionComponent posComp = e.GetComponent<PositionComponent>();
            if(posComp!=null)
                str += string.Format("EntityId {0} Position:{1}",e.Id,posComp.ToString())+"\n";
        }
        str += "Message count:"+Service.Get<LoginService>().KeyframesCount+"\n";
        str += "Keyframe count:" + Service.Get<LoginService>().AllFramesCount + "\n";
        str += "DebugRoll KeyframeIdx :"+ sim.GetBehaviour<RollbackBehaviour>().DebugFrameIdx+"\n";
        str += "FrameIdx:" + sim.GetBehaviour<LogicFrameBehaviour>().CurrentFrameIdx;
        
        m_TxtDebug.text = str;

    }

    private void OnApplicationQuit()
    {
        SimulationManager.Instance.Stop();
    }
}

