
using Synchronize.Game.Lockstep.Config.Static.Interface;

namespace Synchronize.Game.Lockstep.Config.Static
{
    public class LanguageCFG: ICFG
    {
        public int ConfigId { set; get; }

        public string Chinese { set; get; }

        public string English { set; get; }
    }
}
