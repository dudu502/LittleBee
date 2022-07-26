using Config.Static.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Static
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
