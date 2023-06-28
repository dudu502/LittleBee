

using Synchronize.Game.Lockstep.Config.Static.Interface;

namespace Synchronize.Game.Lockstep.Config.Static
{
    public class ResourceIdCFG: ICFG
    {
        public int ConfigId { set; get; }
        public string Path { set; get; }
        public int Rtype { set; get; }
        public double ModelScaleRate { set; get; }
    }
}
