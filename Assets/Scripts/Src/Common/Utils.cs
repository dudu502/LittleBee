using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

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