using Components;
using Frame;
using LogicFrameSync.Src.LockStep;
using Net.Pt;
using System;
using System.Collections;
using System.Collections.Generic;
using TrueSync.Collision;

namespace Entitas
{
    public class EntityWorld
    {
        public static object SyncRoot = new object();
        public bool IsActive = true;
        List<AbstractComponent> m_Components;
        Dictionary<uint, List<AbstractComponent>> m_EntityComponents;
        Dictionary<Type, List<AbstractComponent>> m_TypeComponents;
        ICollisionProvider m_CollisionProvider;       
        Dictionary<string, object> m_Metadata;

        public void Dispose()
        {
            m_Components.Clear();
            m_Components = null;
            m_EntityComponents.Clear();
            m_EntityComponents = null;
            m_TypeComponents.Clear();
            m_TypeComponents = null;
            m_CollisionProvider = null;
            m_Metadata.Clear();
            m_Metadata = null;
        }
        public void SortComponents() { }
        public void SetMeta(string key,object value)
        {
            m_Metadata[key] = value;
        }
        public object GetMeta(string key)
        {
            if (m_Metadata.ContainsKey(key)) return m_Metadata[key];
            return null;
        }
        private EntityWorld()
        {
            m_Metadata = new Dictionary<string, object>();
            m_Components = new List<AbstractComponent>();
            m_EntityComponents = new Dictionary<uint, List<AbstractComponent>>();
            m_TypeComponents = new Dictionary<Type, List<AbstractComponent>>();
        }

        public int GetEntityCount() 
        { 
            return m_EntityComponents.Count; 
        }
        public void SetCollisionProvider(ICollisionProvider provider)
        {
            m_CollisionProvider = provider;
        }
        public ICollisionProvider GetCollisionProvider()
        {
            return m_CollisionProvider;
        }
        public AbstractComponent GetComponentByEntityId(uint entityId,ushort cmd)
        {
            try
            {
                if (m_EntityComponents.ContainsKey(entityId))
                {
                    var componets = m_EntityComponents[entityId];
                    for (int i = componets.Count - 1; i > -1; --i)
                    {
                        var component = componets[i];
                        if (component.GetCommand() == cmd)
                            return component;
                    }
                }
            }
            catch {
                
            }
            return null;
        }

        public T GetComponentByEntityId<T>(uint entityId)
        {
            try
            {
                Type componentType = typeof(T);
                if (m_EntityComponents.ContainsKey(entityId))
                {
                    IList componets = m_EntityComponents[entityId];
                    for (int i = componets.Count - 1; i > -1; --i)
                    {
                        var component = componets[i];
                        if (component == null) return default(T);
                        if (component.GetType() == componentType)
                            return (T)component;
                    }
                }
            }
            catch { }
            return default(T);
        }

        public void ForeachComponent(uint entityid, Action<AbstractComponent> action)
        {
            if (m_EntityComponents.TryGetValue(entityid, out var components))
            {
                foreach (var comp in components)
                {
                    action(comp);
                }
            }
        }

        public void ForEachComponent<T>(Action<T> action)
        {
            try
            {
                Type componentType = typeof(T);
                if (m_TypeComponents.ContainsKey(componentType))
                {
                    IList components = m_TypeComponents[componentType];
                    for (int i = components.Count - 1; i > -1; --i)
                    {
                        var component = components[i];
                        if (component != null)
                            action((T)component);
                    }
                }
            }
            catch { }
        }
        
        /// <summary>
        /// 在调用之前确定已经添加entityId
        /// </summary>
        /// <param name="component"></param>
        public void AddComponent(AbstractComponent component)
        {
            Type componentType = component.GetType();
            m_Components.Add(component);
            if (!m_EntityComponents.ContainsKey(component.EntityId))
                m_EntityComponents[component.EntityId] = new List<AbstractComponent>();
            m_EntityComponents[component.EntityId].Add(component);
            if (!m_TypeComponents.ContainsKey(componentType))
                m_TypeComponents[componentType] = new List<AbstractComponent>();
            m_TypeComponents[componentType].Add(component);
        }

        public static EntityWorld Create()
        {
            return new EntityWorld();
        }

        public bool RemoveEntity(uint entityId)
        {
            if(m_EntityComponents.TryGetValue(entityId, out List<AbstractComponent> removeList))              
            {
                for (int i = removeList.Count - 1; i > -1; --i)
                {
                    AbstractComponent component = removeList[i];
                    if (component != null)
                    {
                        m_TypeComponents.TryGetValue(component.GetType(), out List<AbstractComponent> abstractComponents);
                        if (abstractComponents != null)
                            abstractComponents.Remove(component);
                    }
                }
                m_EntityComponents.Remove(entityId);
            }        
            m_Components.RemoveAll((c) => c.EntityId == entityId);           
            return true;
        }


        public List<AbstractComponent> GetAllCloneComponents()
        {
            List<AbstractComponent> components = new List<AbstractComponent>();
            int size = m_Components.Count;
            for(int i=0;i< size; ++i)
                components.Add(m_Components[i].Clone());
            return components;
        }

        public void RestoreWorld(EntityWorldFrameData entityWorldFrameData)
        {
            m_Components = entityWorldFrameData.Components;
            m_EntityComponents = entityWorldFrameData.EntityComponents;
            m_TypeComponents = entityWorldFrameData.TypeComponents;
        }
        /// <summary>
        /// 获取关键帧数据并且复原整个世界
        /// </summary>
        /// <param name="collection"></param>
        public void RestoreKeyframes(PtKeyFrameCollection collection)
        {
            for (int i = 0; i < collection.KeyFrames.Count; ++i)
            {
                FrameIdxInfo info = collection.KeyFrames[i];

                switch (info.Cmd)
                {
                    case FrameCommand.SYNC_MOVE:
                        AbstractComponent component = GetComponentByEntityId(info.EntityId, info.Cmd);
                        component?.UpdateParams(info.ParamsContent);
                        break;
                    case FrameCommand.SYNC_CREATE_ENTITY:
                        EntityManager.CreateEntityBySyncFrame(this, info);
                        break;
                }

         
            }
            //Need sort
        }

        public void RollBack(EntityWorldFrameData data, PtKeyFrameCollection collection)
        {
            m_Components = data.Components;
            m_EntityComponents = data.EntityComponents;
            m_TypeComponents = data.TypeComponents;

            for (int i = 0; i < collection.KeyFrames.Count; ++i)
            {
                FrameIdxInfo info = collection.KeyFrames[i];
                switch (info.Cmd)
                {
                    case FrameCommand.SYNC_CREATE_ENTITY:
                        EntityManager.CreateEntityBySyncFrame(this, info);
                        break;                
                    case FrameCommand.SYNC_MOVE:
                        AbstractComponent component = GetComponentByEntityId(info.EntityId, info.Cmd);
                        component?.UpdateParams(info.ParamsContent);
                        break;
                }

            }
         
            //Need sort
        }
    }
}
