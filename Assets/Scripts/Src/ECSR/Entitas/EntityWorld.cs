using Components;
using LogicFrameSync.Src.LockStep;
using LogicFrameSync.Src.LockStep.Frame;
using LogicFrameSync.Src.LockStep.Net.Pt;
using System;
using System.Collections.Generic;

namespace Entitas
{
    public class EntityWorld
    {
        List<Entity> m_Entities;
        Dictionary<Type, List<IComponent>> m_DictAllComponents;
        Dictionary<int, List<IComponent>> m_DictEntityAllComponents;
        private EntityWorld()
        {
            m_DictAllComponents = new Dictionary<Type, List<IComponent>>();
            m_DictEntityAllComponents = new Dictionary<int, List<IComponent>>();
            m_Entities = new List<Entity>();
        }
        public void Reset()
        {
            while (m_Entities.Count > 0)
            {
                Entity ent = m_Entities[0];
                m_Entities.RemoveAt(0);
                ent.World = null;
                Entity.ObjectPool.ReturnObjectToPool(ent);
            }
            m_DictAllComponents.Clear();
            m_DictEntityAllComponents.Clear();
        }

        public IComponent GetComponentByEntityId(int entityId, Type componentType)
        {
            if (m_DictEntityAllComponents.ContainsKey(entityId))
            {
                IList<IComponent> components = m_DictEntityAllComponents[entityId];
                foreach (IComponent comp in components)
                {
                    if (componentType == comp.GetType())
                        return comp;
                }
            }
            return null;
        }


        public List<Entity> GetEntities() { return m_Entities; }
        public List<IComponent> GetComponents<T>() where T : IComponent
        {
            if (m_DictAllComponents.ContainsKey(typeof(T)))
                return m_DictAllComponents[typeof(T)];
            return null;
        }
        public void AddComponent(IComponent component)
        {
            Type type = component.GetType();
            if (!m_DictAllComponents.ContainsKey(type))
                m_DictAllComponents[type] = new List<IComponent>();
            if (!m_DictAllComponents[type].Contains(component))
                m_DictAllComponents[type].Add(component);

            if (!m_DictEntityAllComponents.ContainsKey(component.EntityId))
                m_DictEntityAllComponents[component.EntityId] = new List<IComponent>();
            if (!m_DictEntityAllComponents[component.EntityId].Contains(component))
                m_DictEntityAllComponents[component.EntityId].Add(component);
        }
        public void RemoveComponent(IComponent component)
        {
            Type type = component.GetType();
            if (m_DictAllComponents.ContainsKey(type))
            {
                if (m_DictAllComponents[type].Contains(component))
                    m_DictAllComponents[type].Remove(component);
            }

            if (m_DictEntityAllComponents.ContainsKey(component.EntityId))
            {
                if (m_DictEntityAllComponents[component.EntityId].Contains(component))
                    m_DictEntityAllComponents[component.EntityId].Remove(component);
            }
        }
        public static EntityWorld Create()
        {
            return new EntityWorld();
        }


        public bool ContainEntity(int entityId)
        {
            foreach (Entity entity in m_Entities)
            {
                if (entity.Id == entityId)
                    return true;
            }
            return false;
        }
        public Entity AddEntity(int entityId)
        {
            if (!ContainEntity(entityId))
            {
                Entity entity = Entity.ObjectPool.GetObject() as Entity;
                entity.Id = entityId;
                m_Entities.Add(entity);
                entity.World = this;
                return entity;
            }
            return null;
        }
        public void RemoveEntity(int entityId)
        {
            Entity entity = GetEntity(entityId);
            if (entity != null)
            {
                m_Entities.Remove(entity);
                entity.World = null;
                RemoveEntityComponentAll(entityId);
                Entity.ObjectPool.ReturnObjectToPool(entity);
            }
        }

        private void RemoveEntityComponentAll(int entityId)
        {
            if (m_DictEntityAllComponents.ContainsKey(entityId))
            {
                IComponent[] components = m_DictEntityAllComponents[entityId].ToArray();
                if (components != null)
                {
                    for (int i = components.Length - 1; i > -1; --i)
                    {
                        IComponent com = components[i];
                        if (com != null) RemoveComponent(com);
                    }
                }
                m_DictEntityAllComponents.Remove(entityId);
            }
        }
        public Entity GetEntity(int id)
        {
            foreach (Entity en in m_Entities)
                if (id == en.Id) return en;
            return null;
        }

        public List<int> FindAllEntitiesIds()
        {
            List<int> ids = new List<int>();
            foreach (Entity entity in m_Entities)
                ids.Add(entity.Id);
            return ids;
        }

        public List<IComponent> FindAllCloneComponents()
        {
            List<IComponent> components = new List<IComponent>();
            foreach (IList<IComponent> list in m_DictAllComponents.Values)
            {
                foreach (IComponent comp in list)
                    components.Add(comp.Clone());
            }
            return components;
        }

        public void RollBack(EntityWorldFrameData data, PtKeyFrameCollection collection)
        {
            Reset();
            foreach (int roleId in data.m_Entities)
            {
                Entity entity = AddEntity(roleId);
                if (entity != null)
                {
                    foreach (IComponent com in data.m_Components)
                    {
                        if (com.EntityId == roleId)
                        {
                            foreach (FrameIdxInfo info in collection.KeyFrames)
                            {
                                if (info.EqualsInfo(com))
                                {
                                    IParamsUpdatable updatableCom = com as IParamsUpdatable;
                                    if (updatableCom != null)
                                        updatableCom.UpdateParams(info.Params);
                                    else
                                        throw new Exception("Component " + com.ToString() + " must be IParamsUpdatable");
                                    break;
                                }
                            }
                            entity.AddComponent(com);
                        }
                    }
                }
            }
        }
    }
}
