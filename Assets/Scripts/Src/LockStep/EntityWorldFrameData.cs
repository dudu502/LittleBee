
using Components;
using System;
using System.Collections.Generic;

namespace LogicFrameSync.Src.LockStep
{
    /// <summary>
    /// 帧数据快照
    /// </summary>
    public class EntityWorldFrameData
    {
        /// <summary>
        /// 当前所有的EntityIds快照信息
        /// </summary>
        public List<string> EntityIds { private set; get; }
        /// <summary>
        /// 当前所有的Components快照信息
        /// </summary>
        public List<IComponent> Components { private set; get; }
        public EntityWorldFrameData(List<string> entities, List<IComponent> comps)
        {
            EntityIds = entities;
            Components = comps;
        }


        /// <summary>
        /// 获得一个深度拷贝
        /// </summary>
        /// <returns></returns>
        public EntityWorldFrameData Clone()
        {
            List<string> cloneEntities = new List<string>();
            EntityIds.ForEach((a)=>cloneEntities.Add(a));

            List<IComponent> cloneComps = new List<IComponent>();
            Components.ForEach((a)=>cloneComps.Add(a.Clone()));
            EntityWorldFrameData data = new EntityWorldFrameData(cloneEntities,cloneComps);
            return data;
        }
        public override string ToString()
        {
            string entitystring = string.Join(",", EntityIds);
            string componentstring = "";
            foreach(var icom in Components)
            {
                componentstring += icom.ToString()+"    ";
            }
            return string.Format("[Entitys]={0} [Components]={1}", entitystring, componentstring);
        }
    }
}
