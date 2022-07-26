using Config.Static.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Static
{
    public class ResourceIdCFG: ICFG
    {
        public int ConfigId { set; get; }
        public string Path { set; get; }
        public int Rtype { set; get; }
        public double ModelScaleRate { set; get; }
    }
}
