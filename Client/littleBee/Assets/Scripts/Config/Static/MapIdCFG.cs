
using Synchronize.Game.Lockstep.Config.Static.Interface;

namespace Synchronize.Game.Lockstep.Config.Static
{
    public class MapIdCFG : ICFG
    {
        public int ConfigId { set; get; }
        public string Name { set; get; }
        public string Desc { set; get; }
        public string ResKey { set; get; }
        public byte PlayerCount { set; get; }
    }
}
