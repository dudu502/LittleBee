using Components.Common;
using Entitas;
using LogicFrameSync.Src.LockStep;
using NetServiceImpl;
using NetServiceImpl.OnlineMode.Room;
using ObjectPool.Common;
using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public UnityEngine.Transform PoolRoot;
    Simulation Sim;
    EntityWorld World;
    Pool HudPool;
    Dictionary<uint, GameObject> HudEntityGoDict;
    RectTransform rectTransform;
    // Start is called before the first frame update
    private void Awake()
    {
        rectTransform = transform as RectTransform;
    }
    void Start()
    {
        StartCoroutine(StartHub());
    }
    public IEnumerator StartHub()
    {
        yield return null;
        HudEntityGoDict = new Dictionary<uint, GameObject>();
        Sim = SimulationManager.Instance.GetSimulation();
        World = Sim.GetEntityWorld();
        HudPool = PoolMgr.CreatePool("UI/Renders/Info-hud", PoolRoot);
        HudPool.CreateGameObjectAction = () => {
            return GameObject.Instantiate(Resources.Load<GameObject>(HudPool.Path));
        };
    }
    // Update is called once per frame
    void Update()
    {
        if (Sim == null)
            return;
        if (World == null)
            return;
        World.ForEachComponent<HudInfo>(hudInfo =>
        {
            GameObject hudGo;
            if (!HudEntityGoDict.ContainsKey(hudInfo.EntityId))
            {
                hudGo = PoolMgr.PopObject(HudPool.Path, transform);
                HudEntityGoDict.Add(hudInfo.EntityId, hudGo);
            }
            else
            {
                hudGo = HudEntityGoDict[hudInfo.EntityId];
            }
            HudRender hudRender = hudGo.GetComponent<HudRender>();
            hudRender.EntityId = hudInfo.EntityId;
            hudRender.SetName("#"+hudInfo.EntityId);
            Hp hp = World.GetComponentByEntityId<Hp>(hudInfo.EntityId);
            if(hp!=null)
            {
                hudRender.SetHpEnable(true);
                hudRender.SetHp(hp.Value);
            }


            Components.Common.Transform2D com_Pos = World.GetComponentByEntityId<Components.Common.Transform2D>(hudInfo.EntityId);
            Components.Common.Movement2D com_Move = World.GetComponentByEntityId<Components.Common.Movement2D>(hudInfo.EntityId);
            if (com_Pos != null)
            {
                var dir = com_Pos.Toward;
                var pos1 = com_Pos.Position;
                var nextPos = pos1 + dir * (com_Move.Speed * (Time.deltaTime / SimulationManager.Instance.GetFrameMsLength() / 1000));
                var transpos = new Vector3(nextPos.x.AsFloat(), 0, nextPos.y.AsFloat());

                hudGo.transform.localPosition = Vector3.Lerp(hudGo.transform.localPosition, Common.Utils.WorldToUI(Camera.main, rectTransform, transpos)+new Vector3(0,25,0),0.5f);
            }
        });
    }
}
