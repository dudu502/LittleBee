using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Synchronize.Game.Lockstep.MapEditor
{
    [Serializable]
    public class StarObjectInfo
    {
        public uint m_EntityId;
        [Tooltip("ConfigId in MapElements.xls")]
        public int m_ConfigId;
        public uint m_ParentEntityId;
        public int m_InitRevolutionDegree;
        public string m_RevolutionSpeed;
        public int m_RevolutionRedius;
        public string m_RotationSpeed;
        public bool m_Visable = true;

        public byte m_BrokenPieceCount;
        public uint m_BrokenPieceStartId;
        public static string Write(StarObjectInfo data)
        {
            return LitJson.JsonMapper.ToJson(data);
        }
        public static StarObjectInfo Read(string json)
        {
            return LitJson.JsonMapper.ToObject<StarObjectInfo>(json);
        }
    }

    public class StarGalaxyInfo
    {
        public string GalaxyName;
        public int MapWidth;
        public int MapHeight;
        public List<StarObjectInfo> Stars;
        public List<AsteroidBeltInfo> Belts;
    }
}
