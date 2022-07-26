using Config.Static.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Static
{
    public class LanguageCFG: ICFG
    {
        public int ConfigId { set; get; }

        public string Chinese { set; get; }

        public string English { set; get; }
    }
}
