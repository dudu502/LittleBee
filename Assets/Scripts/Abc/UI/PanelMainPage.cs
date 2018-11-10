using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using NetServiceImpl.Server.Data;
using Net;
using System;
using LogicFrameSync.Src.LockStep;
using LogicFrameSync.Src.LockStep.Behaviours;
using Unity.Mathematics;

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
        Simulation sim = new Simulation("server");
        sim.AddBehaviour(new ServerLogicFrameBehaviour());
        SimulationManager.Instance.AddSimulation(sim);
    }

    // Update is called once per frame
    void Update()
    {
        print(Vector3.Cross( new Vector3(0,0,-1), new Vector3(1, 0, 0)));
    }
}


