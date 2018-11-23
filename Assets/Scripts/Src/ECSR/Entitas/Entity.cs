using Components;
using System;
using System.Collections.Generic;


namespace Entitas
{
    public class Entity
    {
        #region Recycle
        public static ObjectPool ObjectPool = new ObjectPool(
                                                             () => new Entity(),
                                                             (entity) =>
                                                             {
                                                                 Entity e = (Entity)entity;
                                                                 e.IsActive = true;
                                                                 e.LifeState = 1;
                                                             },
                                                             (entity) =>
                                                             {
                                                                 Entity e = (Entity)entity;
                                                                 e.Id = "";
                                                                 e.IsActive = false;
                                                                 e.LifeState = 0;
                                                             }
                                                            );
        #endregion
        public byte LifeState = 1;
        public bool IsActive = true;
        public string Id { get; set; }
        public EntityWorld World { set; get; }
        public Entity(string id)
        {
            Id = id;
        }
        public Entity()
        {

        }

        public bool ContainComponent(IComponent component)
        {
            if (!IsActive) return false;
            return World.GetComponentByEntityId(Id, component.GetType()) != null;
        }

        public Entity AddComponent(IComponent component)
        {
            if (!ContainComponent(component))
            {
                component.EntityId = Id;
                World.AddComponent(component);
            }
            return this;
        }

        public T GetComponent<T>() where T : IComponent
        {
            if (!IsActive) return default(T);
            return (T)World.GetComponentByEntityId(Id, typeof(T));
        }

        public void RemoveComponent(IComponent component)
        {
            if (ContainComponent(component))
            {
                World.RemoveComponent(component);
                component.EntityId = "";
            }
        }
    }
}
