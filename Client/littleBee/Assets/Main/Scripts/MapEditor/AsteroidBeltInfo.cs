using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.MapEditor
{
    [Serializable]
    public class AsteroidBeltInfo
    {
        public string m_AsteroidBeltName;
        public uint m_ParentEntityId;
        public uint m_StartEntityId;
        public int m_Seed;
        public int m_NearRadius;
        public int m_FarRadius;
        public string m_RevolutionSpeed;
        public int m_Relaxation = 1;
        public int m_ShellRelaxation = 1;
        public int m_Gradient = 1;

        public AsteroidBeltInfo()
        {

        }
    }
}
