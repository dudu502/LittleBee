using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Config.Static.Interface
{
    public interface ICombativeCFG
    {
        int HaloType { set; get; }
        int HaloEffectRadius { set; get; }
        int AttackType { set; get; }
        int DefenceType { set; get; }
        int Attack { set; get; }
        int Defence { set; get; }
    }
}
