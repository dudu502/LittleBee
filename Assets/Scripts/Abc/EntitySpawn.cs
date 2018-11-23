using UnityEngine;
using System.Collections;
using Entitas;
using Components;
using Unity.Mathematics;
using System.Collections.Concurrent;
using Renderers;

/// <summary>
/// 生成Entity
/// </summary>
public class EntitySpawn : MonoBehaviour
{
    Notify.Notifier Notifier;
    ConcurrentQueue<string> QueueCreateEntity = new ConcurrentQueue<string>();
    ConcurrentQueue<string> QueueBulletEntity = new ConcurrentQueue<string>();
    ConcurrentQueue<string> QueueRemoveBulletEntity = new ConcurrentQueue<string>();
    // Use this for initialization
    void Start()
    {
        Notifier = new Notify.Notifier(this);
    }
    [Notify.Subscribe(Entitas.EntityWorld.EntityOperationEvent.CreatePlayer)]
    void OnCreateEntityFromThread(Notify.Notification note)
    {
        Entity entity = note.Params[0] as Entity;
        entity.AddComponent(new MoveComponent(15, float2.zero)).AddComponent(new TransformComponent(float2.zero)).
            AddComponent(new SphereColliderComponent(VInt3.zero,5));
        QueueCreateEntity.Enqueue(entity.Id);
    }
    [Notify.Subscribe(Entitas.EntityWorld.EntityOperationEvent.CreateBullet)]
    void OnCreateBulletEntityFromThread(Notify.Notification note)
    {
        Entity entity = note.Params[0] as Entity;
        entity.AddComponent(new MoveComponent(5, new float2(0, 1))).AddComponent(new TransformComponent(float2.zero)).AddComponent(new AutoRemovingEntityComponent(80));
        QueueBulletEntity.Enqueue(entity.Id);
    }

    [Notify.Subscribe(Entitas.EntityWorld.EntityOperationEvent.Remove)]
    void OnRemoveEntityFromThread(Notify.Notification note)
    {
        print("OnRemoveEntityFromThread");
        string id = note.Params[0] as string;
        QueueRemoveBulletEntity.Enqueue(id);
    }
    // Update is called once per frame
    void Update()
    {
        if (QueueCreateEntity.Count > 0)
        {
            string id = "";
            if (QueueCreateEntity.TryDequeue(out id))
            {
                CreateNewEntityGO(id);
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
        renderers.SetEntityId(id);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);
    }


}
