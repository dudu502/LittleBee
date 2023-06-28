using Synchronize.Game.Lockstep;
using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.UI;
using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;

namespace Synchronize.Game.Lockstep.UI
{
    public class Hud : MonoBehaviour
    {
        const string HUD_INFO_PATH = "UI/Renders/Info-hud";
        public UnityEngine.Transform PoolRoot;
        Simulation Sim;
        EntityWorld World;
        PoolModule PoolModule;
        // Pool HudPool;
        Dictionary<uint, GameObject> HudEntityGoDict;
        RectTransform rectTransform;
        // Start is called before the first frame update
        private void Awake()
        {
            rectTransform = transform as RectTransform;
        }
        void Start()
        {
            PoolModule = ModuleManager.GetModule<PoolModule>();
            StartHud();
        }
        public void StartHud()
        {
            HudEntityGoDict = new Dictionary<uint, GameObject>();
            Sim = SimulationManager.Instance.GetSimulation();
            World = Sim.GetEntityWorld();

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
                    PoolModule.CreatePoolIfNotExist(HUD_INFO_PATH);
                    hudGo = PoolModule.Reuse(HUD_INFO_PATH);
                    HudEntityGoDict.Add(hudInfo.EntityId, hudGo);
                    hudGo.transform.SetParent(PoolRoot);
                }
                else
                {
                    hudGo = HudEntityGoDict[hudInfo.EntityId];
                }


                HudRender hudRender = hudGo.GetComponent<HudRender>();
                hudRender.SetEntityId(hudInfo.EntityId);
                hudRender.SetName("#" + hudInfo.EntityId);
                Hp hp = World.GetComponentByEntityId<Hp>(hudInfo.EntityId);
                if (hp != null)
                {
                    hudRender.SetHpEnable(true);
                    hudRender.SetHp(hp.Value);
                }


                Transform2D com_Pos = World.GetComponentByEntityId<Transform2D>(hudInfo.EntityId);
                Movement2D com_Move = World.GetComponentByEntityId<Movement2D>(hudInfo.EntityId);
                if (com_Pos != null)
                {
                    var dir = com_Pos.Toward;
                    var pos1 = com_Pos.Position;
                    var nextPos = pos1 + dir * (com_Move.Speed * (Time.deltaTime / SimulationManager.Instance.GetFrameMsLength() / 1000));
                    var transpos = new Vector3(nextPos.x.AsFloat(), 0, nextPos.y.AsFloat());

                    hudGo.transform.localPosition = Vector3.Lerp(hudGo.transform.localPosition, Common.Utils.WorldToUI(Camera.main, rectTransform, transpos) + new Vector3(0, 25, 0), 0.5f);
                }
            });
        }
    }
}
