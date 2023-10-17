
using Net.Pt;
using Synchronize.Game.Lockstep.Ecsr.Components;
using Synchronize.Game.Lockstep.Frame;
using System;
using System.Collections;
using System.Collections.Generic;
using TrueSync.Collision;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Ecsr.Entitas
{
    public class EntityWorld
    {
        public static object SyncRoot = new object();
        public bool IsActive = true;
        EntityWorldFrameData m_FrameData;
        ICollisionProvider m_CollisionProvider;       
        Dictionary<string, object> m_Metadata;

        public void Dispose()
        {
            m_FrameData.Clear();
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
            m_FrameData = new EntityWorldFrameData(new List<AbstractComponent>(),
                                                    new Dictionary<uint, List<AbstractComponent>>(),
                                                    new Dictionary<Type, List<AbstractComponent>>());
        }

        public int GetEntityCount() 
        { 
            return m_FrameData.EntityComponents.Count;
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
                if (m_FrameData.EntityComponents.ContainsKey(entityId))
                {
                    var componets = m_FrameData.EntityComponents[entityId];
                    for (int i = componets.Count - 1; i > -1; --i)
                    {
                        var component = componets[i];
                        if (component.GetCommand() == cmd)
                            return component;
                    }
                }
            }
            catch {
                Debug.LogError("GetComponentByEntityId Error");
            }
            return null;
        }

        public T GetComponentByEntityId<T>(uint entityId)
        {
            try
            {
                Type componentType = typeof(T);
                if (m_FrameData.EntityComponents.ContainsKey(entityId))
                {
                    IList componets = m_FrameData.EntityComponents[entityId];
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
        public void ForEachComponent(Type componentType,Action<AbstractComponent> action)
        {
            if (m_FrameData.TypeComponents.ContainsKey(componentType))
            {
                IList components = m_FrameData.TypeComponents[componentType];
                for (int i = components.Count - 1; i > -1; --i)
                {
                    var component = components[i];
                    if (component != null)
                        action((AbstractComponent)component);
                }
            }
        }
        public void ForEachComponent<T>(Action<T> action)
        {
            try
            {
                Type componentType = typeof(T);
                if (m_FrameData.TypeComponents.ContainsKey(componentType))
                {
                    IList components = m_FrameData.TypeComponents[componentType];
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
            m_FrameData.Components.Add(component);
            if (!m_FrameData.EntityComponents.ContainsKey(component.EntityId))
                m_FrameData.EntityComponents[component.EntityId] = new List<AbstractComponent>();
            m_FrameData.EntityComponents[component.EntityId].Add(component);
            if (!m_FrameData.TypeComponents.ContainsKey(componentType))
                m_FrameData.TypeComponents[componentType] = new List<AbstractComponent>();
            m_FrameData.TypeComponents[componentType].Add(component);
        }

        public static EntityWorld Create()
        {
            return new EntityWorld();
        }

        public bool RemoveEntity(uint entityId)
        {
            if(m_FrameData.EntityComponents.TryGetValue(entityId, out List<AbstractComponent> removeList))              
            {
                for (int i = removeList.Count - 1; i > -1; --i)
                {
                    AbstractComponent component = removeList[i];
                    if (component != null)
                    {
                        m_FrameData.TypeComponents.TryGetValue(component.GetType(), out List<AbstractComponent> abstractComponents);
                        if (abstractComponents != null)
                            abstractComponents.Remove(component);
                    }
                }
                m_FrameData.EntityComponents.Remove(entityId);
            }        
            m_FrameData.Components.RemoveAll((c) => c.EntityId == entityId);           
            return true;
        }


        public List<AbstractComponent> GetAllCloneComponents()
        {
            List<AbstractComponent> components = new List<AbstractComponent>();
            int size = m_FrameData.Components.Count;
            for(int i=0;i< size; ++i)
                components.Add(m_FrameData.Components[i].Clone());
            return components;
        }

        public void RestoreWorld(EntityWorldFrameData entityWorldFrameData)
        {
            m_FrameData.Components = entityWorldFrameData.Components;
            m_FrameData.EntityComponents = entityWorldFrameData.EntityComponents;
            m_FrameData.TypeComponents = entityWorldFrameData.TypeComponents;
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
            m_FrameData.Components = data.Components;
            m_FrameData.EntityComponents = data.EntityComponents;
            m_FrameData.TypeComponents = data.TypeComponents;

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
