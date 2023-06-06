using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Config.Static.Interface
{
    public interface IObjectCFG
    {

        string Name { set; get; }
        string Desc { set; get; }
        int ResKey { set; get; }
        int Mass { set; get; }
        int Diameter { set; get; }
    }
}
