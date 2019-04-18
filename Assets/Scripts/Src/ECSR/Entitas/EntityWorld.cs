using Components;
using LogicFrameSync.Src.LockStep;
using LogicFrameSync.Src.LockStep.Frame;
using LogicFrameSync.Src.LockStep.Net.Pt;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entitas
{
    public class EntityWorld
    {
        public static object SyncRoot = new object();
        public enum EntityOperationEvent
        {
            CreatePlayer,
            CreateBullet,
            CreateBox,
            RemoveBox,
        }
        public bool IsActive = true;
        Notify.Notifier m_Notifier = null;
        Dictionary<Guid,Entity> m_DictEntities;
        Dictionary<Type, List<IComponent>> m_DictAllComponents;
        Dictionary<Guid, List<IComponent>> m_DictEntityAllComponents;
        private EntityWorld()
        {       
            m_Notifier = new Notify.Notifier(this);
            m_DictAllComponents = new Dictionary<Type, List<IComponent>>();
            m_DictEntityAllComponents = new Dictionary<Guid, List<IComponent>>();
            m_DictEntities = new Dictionary<Guid, Entity>();
        }
        public void Reset()
        {
            foreach(Guid entityId in new List<Guid>(m_DictEntities.Keys))
            {
                Entity entity = m_DictEntities[entityId];
                m_DictEntities.Remove(entityId);
                entity.World = null;
                Entity.ObjectPool.ReturnObjectToPool(entity);
                entity = null;
            }

            m_DictAllComponents.Clear();
            m_DictEntityAllComponents.Clear();
        }
        public List<Entity> GetEntities() { return new List<Entity>(m_DictEntities.Values); }

        public IComponent GetComponentByEntityId(Guid entityId, Type componentType)
        {
            if (m_DictEntityAllComponents.ContainsKey(entityId))
            {
                IList<IComponent> components = m_DictEntityAllComponents[entityId];
                for (int i = components.Count - 1; i > -1; --i)
                {
                    if (componentType == components[i].GetType())
                        return components[i];
                }
            }
            return null;
        }


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


        public bool ContainEntity(Guid entityId)
        {
            return m_DictEntities.ContainsKey(entityId);
        }
        public Entity AddEntity(Guid entityId)
        {
            if (!ContainEntity(entityId))
            {
                Entity entity = Entity.ObjectPool.GetObject() as Entity;
                //Entity entity = new Entity();
                entity.Id = entityId;
                m_DictEntities[entityId] = entity;
                entity.World = this;
                return entity;
            }
            return m_DictEntities[entityId];
        }
        public bool RemoveEntity(Guid entityId)
        {
            Entity entity = GetEntity(entityId);
            if (entity != null)
            {
                m_DictEntities.Remove(entityId);
                entity.World = null;
                RemoveEntityComponentAll(entityId);
                Entity.ObjectPool.ReturnObjectToPool(entity);
                entity = null;
                return true;
            }
            return false;
        }

        private void RemoveEntityComponentAll(Guid entityId)
        {
            if (m_DictEntityAllComponents.ContainsKey(entityId))
            {
                List<IComponent> components = m_DictEntityAllComponents[entityId];
                if (components != null)
                {
                    for (int i = components.Count - 1; i > -1; --i)
                    {
                        IComponent com = components[i];
                        if (com != null) RemoveComponent(com);
                    }
                }
                m_DictEntityAllComponents.Remove(entityId);
            }
        }
        public Entity GetEntity(Guid id)
        {
            if (ContainEntity(id))
                return m_DictEntities[id];
            return null;
        }

        public List<Guid> FindAllEntitiesIds()
        {
            List<Guid> ids = new List<Guid>();
            foreach (Entity entity in m_DictEntities.Values)
                ids.Add(entity.Id);
            ids.Sort((a, b) => a.CompareTo(b));
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
            components.Sort((a,b)=>a.EntityId.CompareTo(b.EntityId));
            return components;
        }
        
        public void RollBack(EntityWorldFrameData data, PtKeyFrameCollection collection)
        {
            Reset();
            foreach(Guid entityId in data.EntityIds)
            {
                Entity entity = AddEntity(entityId);               
                foreach (IComponent com in data.Components)
                {
                    if(com.EntityId == entityId)
                    {
                        foreach(FrameIdxInfo info in collection.KeyFrames)
                        {
                            if(info.EqualsInfo(com))
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
            };

            foreach(FrameIdxInfo info in collection.KeyFrames)
            {
                if(!ContainEntity(info.EntityId))
                {
                    if(info.Cmd == FrameCommand.SYNC_CREATE_ENTITY)
                    {
                        NotifyCreateEntity(info);
                    }      
                }
                else
                {
                    if (info.Cmd == FrameCommand.SYNC_REMOVE_ENTITY)
                    {
                        NotifyRemoveEntity(info.EntityId);
                    }
                }
            }
        }

        #region Notify Notifications
        public void NotifyCreateEntity(FrameIdxInfo info)
        {
            Entity entity = AddEntity(info.EntityId);
            if(entity != null)
            {          
                m_Notifier.Send((EntityOperationEvent)int.Parse(info.Params[0]), entity,info);
            }
        }
        public void NotifyRemoveEntity(Guid entityId)
        {
            RemoveEntity(entityId);
            m_Notifier.Send(EntityOperationEvent.RemoveBox, entityId);
        }
        #endregion
    }
}
