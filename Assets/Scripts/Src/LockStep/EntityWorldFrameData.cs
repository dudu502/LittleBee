
using Components;
using System;
using System.Collections.Generic;

namespace LogicFrameSync.Src.LockStep
{
    public class EntityWorldFrameData
    {
        public List<string> m_Entities;
        public List<IComponent> m_Components;
        public EntityWorldFrameData(List<string> entities, List<IComponent> comps)
        {
            m_Entities = entities;
            m_Components = comps;
        }


        public override string ToString()
        {
            string entitystring = string.Join(",", m_Entities);
            string componentstring = "";
            foreach(var icom in m_Components)
            {
                componentstring += icom.ToString()+"    ";
            }
            return string.Format("[Entitys]={0} [Components]={1}", entitystring, componentstring);
        }
    }
}
