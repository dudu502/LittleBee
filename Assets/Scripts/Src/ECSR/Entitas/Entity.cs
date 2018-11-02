using Components;
using System;
using System.Collections.Generic;


namespace Entitas
{
    public class Entity
    {
        #region Recycle
        public ObjectPool Pool { set; get; }
        public static ObjectPool ObjectPool = new ObjectPool(
                                                             () => new Entity(),
                                                             (entity) =>
                                                             {
                                                                 Entity e = (Entity)entity;
                                                             },
                                                             (entity) =>
                                                             {
                                                                 Entity e = (Entity)entity;
                                                                 e.Id = 0;
                                                             }
                                                            );
        #endregion
        public int Id { get; set; }
        public EntityWorld World { set; get; }
        public Entity(int id)
        {
            Id = id;
        }
        public Entity()
        {

        }

        public bool ContainComponent(IComponent component)
        {
            return World.GetComponentByEntityId(Id, component.GetType()) != null;
        }

        public Entity AddComponent(IComponent component)
        {
            if (!ContainComponent(component))
            {
                component.EntityId = Id;
                component.Enable = true;
                World.AddComponent(component);
            }
            return this;
        }

        public T GetComponent<T>() where T : IComponent
        {
            return (T)World.GetComponentByEntityId(Id, typeof(T));
        }
        public IComponent GetComponent(int componentType)
        {
            return World.GetComponentByEntityId(Id, componentType);
        }
        public void RemoveComponent(IComponent component)
        {
            if (ContainComponent(component))
            {
                World.RemoveComponent(component);
                component.EntityId = 0;
            }
        }
    }
}
