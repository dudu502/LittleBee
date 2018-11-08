using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common
{

    public class Utils
    {
        public static string GuidToString()
        {
            return System.Guid.NewGuid().ToString();
            
        }
    }
}

