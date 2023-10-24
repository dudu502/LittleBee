

using Synchronize.Game.Lockstep.Ecsr.Components;
using Synchronize.Game.Lockstep.Ecsr.Components.Common;
using Synchronize.Game.Lockstep.Ecsr.Components.Star;
using System;
using System.Collections.Generic;

namespace Synchronize.Game.Lockstep.Ecsr.Entitas
{
    /// <summary>
    /// 帧数据快照
    /// </summary>
    public class EntityWorldFrameData
    {
        public static readonly List<Type> ComponentTypes = new List<Type> 
        {
            typeof(StarObjectInfo),
            typeof(StarObjectRevolution),
            typeof(StarObjectRotation),
            typeof(Transform2D),
            typeof(Movement2D),
            typeof(Attack),
            typeof(Bullet),
            typeof(Countdown),
            typeof(Defence),
            typeof(FsmInfo),
            typeof(GravitationalField),
            typeof(Hp),
            typeof(HudInfo),
            typeof(Mp),
            typeof(Particle),
            typeof(RotationParameter),
            typeof(RotationValue),
            typeof(BreakableInfo),
            typeof(BrokenPiece),
        };

        /// <summary>
        /// 当前所有的Components快照信息
        /// </summary>
        public List<AbstractComponent> Components;
        public Dictionary<uint, List<AbstractComponent>> EntityComponents;
        public Dictionary<Type, List<AbstractComponent>> TypeComponents;
        public EntityWorldFrameData(List<AbstractComponent> comps)
        {
            Components = comps;
            EntityComponents = new Dictionary<uint, List<AbstractComponent>>();
            TypeComponents = new Dictionary<Type, List<AbstractComponent>>();
            foreach (var component in comps)
            {
                if (!EntityComponents.ContainsKey(component.EntityId))
                    EntityComponents[component.EntityId] = new List<AbstractComponent>();
                EntityComponents[component.EntityId].Add(component);

                Type type = component.GetType();
                if (!TypeComponents.ContainsKey(type))
                    TypeComponents[type] = new List<AbstractComponent>();
                TypeComponents[type].Add(component);
            }
        }
        public EntityWorldFrameData(List<AbstractComponent> comps, Dictionary<uint, List<AbstractComponent>> entityComps, Dictionary<Type, List<AbstractComponent>> typeComps)
        {
            Components = comps;
            EntityComponents = entityComps;
            TypeComponents = typeComps;
        }
        public void Clear()
        {
            Components.Clear();
            Components = null;
            EntityComponents.Clear();
            EntityComponents = null;
            TypeComponents.Clear();
            TypeComponents = null;
        }
        public override string ToString()
        {
            //Components.Sort();
            string str = "";
            foreach (AbstractComponent component in Components)
            {
                if((component is Transform2D || component is Movement2D || component is Hp))
                    str += component.ToString() + "\n";
            }
            str += "\n\n";
            return str;
        }

        public static byte[] Write(EntityWorldFrameData data)
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteInt32(data.Components.Count);
                for(int i=0;i<data.Components.Count;++i)
                {
                    int index = ComponentTypes.IndexOf(data.Components[i].GetType());
                    if (index == -1)
                    {
                        UnityEngine.Debug.LogError("ComponentTypes Write todo:" + data.Components[i].GetType().FullName);
                    }
                    else
                    {
                        buffer.WriteUInt16((ushort)index);
                        buffer.WriteBytes(data.Components[i].Serialize());
                    }
                }

                return buffer.Getbuffer();
            }
        }
        public static EntityWorldFrameData Read(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                int count = buffer.ReadInt32();
                List<AbstractComponent> components = new List<AbstractComponent>();
                for (int i = 0; i < count; ++i)
                {
                    int index = buffer.ReadUInt16();
                    AbstractComponent component = CreateComponent(ComponentTypes[index]);
                    component.Deserialize(buffer.ReadBytes());
                    components.Add(component);
                }
                return new EntityWorldFrameData(components);
            }
        }

        private static AbstractComponent CreateComponent(Type type)
        {
            if(type == typeof(StarObjectInfo))
                return new StarObjectInfo();
            if(type == typeof(StarObjectRevolution))
                return new StarObjectRevolution();
            if(type == typeof(StarObjectRotation))
                return new StarObjectRotation();
            if(type == typeof(Transform2D))
                return new Transform2D();
            if(type == typeof(Movement2D))
                return new Movement2D();
            if(type == typeof(Attack))
                return new Attack();
            if(type == typeof(Bullet))
                return new Bullet();
            if(type == typeof(Countdown))
                return new Countdown();
            if(type == typeof(Defence))
                return new Defence();
            if(type == typeof(FsmInfo))
                return new FsmInfo();
            if(type == typeof(GravitationalField))
                return new GravitationalField();
            if(type == typeof(Hp))
                return new Hp();
            if(type == typeof(HudInfo))
                return new HudInfo();   
            if(type == typeof(Mp))
                return new Mp();
            if(type == typeof(Particle))
                return new Particle();
            if(type == typeof(RotationParameter))
                return new RotationParameter();
            if(type == typeof(RotationValue))
                return new RotationValue();
            if(type == typeof(BreakableInfo))
                return new BreakableInfo();
            if(type == typeof(BrokenPiece))
                return new BrokenPiece();
            throw new Exception("Please TODO Add Component "+type.ToString());
        }
    }
}
