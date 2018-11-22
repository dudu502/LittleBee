using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using NetServiceImpl.Server.Data;
using Net;
using System;
using LogicFrameSync.Src.LockStep;
using LogicFrameSync.Src.LockStep.Behaviours;
using Unity.Mathematics;
using System.Collections.Generic;
using Components;

public class PanelMainPage : MonoBehaviour
{
    public Button m_BtnLan;
    public Button m_BtnServer;


    // Use this for initialization
    void Start()
    {
        m_BtnLan.onClick.AddListener(() => {
            gameObject.SetActive(false);
            GameClientNetwork.Instance.Connect();
        });

        m_BtnServer.onClick.AddListener(()=> {
            GameServerNetwork.Instance.Start();

            AddServerSim();
        });
    }

    private void AddServerSim()
    {
        Simulation sim = new Simulation(Const.SERVER_SIMULATION_ID);
        sim.AddBehaviour(new ServerLogicFrameBehaviour());
        SimulationManager.Instance.AddSimulation(sim);
    }

    TransformComponent child = new TransformComponent(new float2(1,1));
    TransformComponent parent = new TransformComponent(new float2(3, 3));
    TransformComponent pp = new TransformComponent(new float2(2,2));
    [ContextMenu("testlocal2world")]
    void testlocal2world()
    {
        parent.RotateDegreeZ = 45;
        child.Parent = parent;
        parent.Parent = pp;
        //print(child.ToString());
        //child.RotateDeltaDegree(-parent.RotateDegreeZ);
        //print(child.ToString());
        //child.Translate(parent.LocalPosition);
        //print(child.ToString());
        print(TransformComponent.LocalPosition2WorldPosition(child));
    }

    TransformComponent transcomp1 = new TransformComponent(new float2(3, 3+math.sqrt(2)));
    [ContextMenu("testworld2local")]
    void testworld2local()
    {
        transcomp1.Translate(new float2(-3, -3));
        transcomp1.RotateDeltaDegree(45);

        print(transcomp1.ToString());
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}


