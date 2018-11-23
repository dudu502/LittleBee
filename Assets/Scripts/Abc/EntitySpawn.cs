using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using Components;
using Unity.Mathematics;
using System.Collections.Concurrent;
using Renderers;
using LogicFrameSync.Src.LockStep.Frame;

/// <summary>
/// 生成Entity
/// </summary>
public class EntitySpawn : MonoBehaviour
{
    Notify.Notifier Notifier;
    Dictionary<string, GameObject> DictGameObject = new Dictionary<string, GameObject>();
    ConcurrentQueue<string> QueueCreateEntity = new ConcurrentQueue<string>();
    ConcurrentQueue<string> QueueCreateBoxEntity = new ConcurrentQueue<string>();
    ConcurrentQueue<string> QueueBulletEntity = new ConcurrentQueue<string>();
    ConcurrentQueue<string> QueueRemoveBoxEntity = new ConcurrentQueue<string>();
    // Use this for initialization
    void Start()
    {
        Notifier = new Notify.Notifier(this);
    }
    [Notify.Subscribe(Entitas.EntityWorld.EntityOperationEvent.CreatePlayer)]
    void OnCreateEntityFromThread(Notify.Notification note)
    {
        Entity entity = note.Params[0] as Entity;
        entity.AddComponent(new MoveComponent(15, float2.zero)).
            AddComponent(new TransformComponent(float2.zero)).
            AddComponent(new PlayerInfoComponent()).
            AddComponent(new SphereColliderComponent(VInt3.zero,20));
        QueueCreateEntity.Enqueue(entity.Id);
    }

    [Notify.Subscribe(Entitas.EntityWorld.EntityOperationEvent.CreateBox)]
    void OnCreateBoxEntityFromThread(Notify.Notification note)
    {
        Entity entity = note.Params[0] as Entity;
        FrameIdxInfo frameIdxInfo = note.Params[1] as FrameIdxInfo;
        entity.AddComponent(new TransformComponent(float2.zero)).
            AddComponent(new MoveComponent(int.Parse(frameIdxInfo.Params[1]),new float2( int.Parse(frameIdxInfo.Params[2]),int.Parse(frameIdxInfo.Params[3]) ))).
            AddComponent(new BoxColliderComponent(VInt3.zero, new VInt3(50, 50, 0))).
            AddComponent(new IntValueComponent(1)).
            AddComponent(new FrameClockComponent(100,1));
        QueueCreateBoxEntity.Enqueue(entity.Id);
    }

    [Notify.Subscribe(Entitas.EntityWorld.EntityOperationEvent.CreateBullet)]
    void OnCreateBulletEntityFromThread(Notify.Notification note)
    {
        Entity entity = note.Params[0] as Entity;
        entity.AddComponent(new MoveComponent(5, new float2(0, 1))).AddComponent(new TransformComponent(float2.zero)).AddComponent(new AutoRemovingEntityComponent(80));
        QueueBulletEntity.Enqueue(entity.Id);
    }


    [Notify.Subscribe(Entitas.EntityWorld.EntityOperationEvent.RemoveBox)]
    void OnRemoveEntityFromThread(Notify.Notification note)
    {
        print("OnRemoveEntityFromThread");
        string id = note.Params[0] as string;
        QueueRemoveBoxEntity.Enqueue(id);
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 10 != 0) return;
        if (QueueCreateEntity.Count > 0)
        {
            string id = "";
            if (QueueCreateEntity.TryDequeue(out id))
            {
                CreateNewEntityGO(id);
            }
        }       
        if(QueueCreateBoxEntity.Count>0)
        {
            string id = "";
            if(QueueCreateBoxEntity.TryDequeue(out id))
            {
                CreateBoxEntityGo(id);
            }
        }

        if(QueueRemoveBoxEntity.Count>0)
        {
            string id = "";
            if(QueueRemoveBoxEntity.TryDequeue(out id))
            {      
                if(DictGameObject.ContainsKey(id))
                {
                    Box.ObjectPool.ReturnGameObjectToPool(DictGameObject[id]);
                    DictGameObject.Remove(id);
                }
                    
            }
        }
    }
    void CreateBulletEntityGo(string id)
    {
        MoveActionRenderer renders = Bullet.ObjectPool.GetGameObject().GetComponent<MoveActionRenderer>();
        renders.SetEntityId(id);
        renders.transform.SetParent(transform);
        renders.transform.localPosition = Vector3.zero;
        renders.transform.localScale = Vector3.one;
      
    }
    void CreateNewEntityGO(string id)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load("EntityObj") as GameObject);
        MoveActionRenderer renderers = obj.AddComponent<MoveActionRenderer>();
        PlayerInfoDisplayActionRenderer red = obj.GetComponent<PlayerInfoDisplayActionRenderer>();
        red.SetEntityId(id);
        renderers.SetEntityId(id);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);
        DictGameObject[id] = renderers.gameObject;
    }

    void CreateBoxEntityGo(string id)
    {
        GameObject obj = Box.ObjectPool.GetGameObject();
        FrameClockActionRenderer renders = obj.GetComponent<FrameClockActionRenderer>();
        MoveActionRenderer move = obj.GetComponent<MoveActionRenderer>();

        renders.SetEntityId(id);
        if (move == null) move = obj.AddComponent<MoveActionRenderer>();
        move.SetEntityId(id);
        renders.transform.SetParent(transform);
        renders.transform.localPosition = Vector3.zero;
        renders.transform.localScale = Vector3.one;

       
        DictGameObject[id] = renders.gameObject;
    }
}
