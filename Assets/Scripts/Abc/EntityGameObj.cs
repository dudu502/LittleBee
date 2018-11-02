using UnityEngine;

using LogicFrameSync.Src.LockStep;
using System;
using DG.Tweening;

using UnityEngine.UI;
using Components;

public class EntityGameObj : MonoBehaviour
{
    public Text m_Txt;
    public Entitas.Entity Ent;
    RectTransform rect;
    public int ID = 0;
    // Use this for initialization
    void Start()
    {
        rect = gameObject.GetComponent<RectTransform>();
    }


    Tweener t = null;
    void Update()
    {
        if (ID == 0) return;
        var sim = SimulationManager.Instance.GetSimulation("client");
        if (sim == null) return;
        Ent = sim.GetEntityWorld().GetEntity(ID);
        if (Ent == null) return;
        var pos = Ent.GetComponent<PositionComponent>();
        var anpos = rect.anchoredPosition;
        m_Txt.text = ID.ToString();
        if (pos != null)
        {
            double lerp = sim.GetFrameLerp()* sim.GetFrameMsLength ()/1000/ Time.deltaTime;
            var pos1 = pos.GetPosition();// new Vector2(pos.Pos.x,pos.Pos.y);
            var dir = Ent.GetComponent<MoveComponent>().GetDir();// new Vector2(Ent.GetComponent<MoveComponent>().Dir.x, Ent.GetComponent<MoveComponent>().Dir.y);
            var nextPos = pos1 + dir * (Ent.GetComponent<MoveComponent>().GetSpeed() * (float)(Time.deltaTime/ sim.GetFrameMsLength() / 1000));


            //rect.anchoredPosition = Vector2.Lerp(pos1, nextPos, (float)lerp);
            
            if (t != null)
            {
                t.Kill();
                t = null;
            }
                
            
            t = rect.DOLocalMove(Vector2.Lerp(pos1, nextPos, (float)lerp),1, false);
            
        }
    }
}
