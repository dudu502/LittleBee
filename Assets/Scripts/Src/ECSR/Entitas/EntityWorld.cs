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
        public enum EntityOperationEvent
        {
            CreatePlayer,
            CreateBullet,
            CreateBox,
            RemoveBox,
        }
        public bool IsActive = true;
        Notify.Notifier m_Notifier = null;
        Dictionary<string,Entity> m_DictEntities;
        Dictionary<Type, List<IComponent>> m_DictAllComponents;
        Dictionary<string, List<IComponent>> m_DictEntityAllComponents;
        private EntityWorld()
        {       
            m_Notifier = new Notify.Notifier(this);
            m_DictAllComponents = new Dictionary<Type, List<IComponent>>();
            m_DictEntityAllComponents = new Dictionary<string, List<IComponent>>();
            m_DictEntities = new Dictionary<string, Entity>();
        }
        public void Reset()
        {
            foreach(string entityId in new List<string>(m_DictEntities.Keys))
            {
                Entity entity = m_DictEntities[entityId];
                m_DictEntities.Remove(entityId);
                entity.World = null;
                Entity.ObjectPool.ReturnObjectToPool(entity);
            }

            m_DictAllComponents.Clear();
            m_DictEntityAllComponents.Clear();
        }
        public List<Entity> GetEntities() { return new List<Entity>(m_DictEntities.Values); }

        public IComponent GetComponentByEntityId(string entityId, Type componentType)
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


        public bool ContainEntity(string entityId)
        {
            return m_DictEntities.ContainsKey(entityId);
        }
        public Entity AddEntity(string entityId)
        {
            if (!ContainEntity(entityId))
            {
                Entity entity = Entity.ObjectPool.GetObject() as Entity;
                entity.Id = entityId;
                m_DictEntities[entityId] = entity;
                entity.World = this;
                return entity;
            }
            return null;
        }
        public void RemoveEntity(string entityId)
        {
            Entity entity = GetEntity(entityId);
            if (entity != null)
            {
                m_DictEntities.Remove(entityId);
                entity.World = null;
                RemoveEntityComponentAll(entityId);
                Entity.ObjectPool.ReturnObjectToPool(entity);
            }
        }

        private void RemoveEntityComponentAll(string entityId)
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
        public Entity GetEntity(string id)
        {
            if (ContainEntity(id))
                return m_DictEntities[id];
            return null;
        }

        public List<string> FindAllEntitiesIds()
        {
            List<string> ids = new List<string>();
            foreach (Entity entity in m_DictEntities.Values)
                ids.Add(entity.Id);
            ids.Sort((a, b) => new Guid(a).CompareTo(new Guid(b)));
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
            components.Sort((a,b)=>new Guid(a.EntityId).CompareTo(new Guid(b.EntityId)));
            return components;
        }
        
        public void RollBack(EntityWorldFrameData data, PtKeyFrameCollection collection)
        {
            Reset();

            data.m_Entities.ForEach((entityId)=> 
            {
                Entity entity = AddEntity(entityId);
                if (entity != null)
                {
                    foreach (IComponent com in data.m_Components)
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
                }
            });

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
        public void NotifyRemoveEntity(string entityId)
        {
            RemoveEntity(entityId);
            m_Notifier.Send(EntityOperationEvent.RemoveBox, entityId);
        }
        #endregion
    }
}
