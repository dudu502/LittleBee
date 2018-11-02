using LogicFrameSync.Src.LockStep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Log
{
    public class GameEntityWorldLog
    {   
        public static string Write(Dictionary<int,EntityWorldFrameData> dict,long roleId)
        {
            string log = "[GameEntityInfoLog--RoleId]"+roleId+"\n";
            var keys = dict.Keys;
            List<int> keysList = new List<int>();
            foreach (int k in keys)
                keysList.Add(k);
            keysList.Sort((a, b) => a - b);

            foreach (int key in keysList)
            {
                EntityWorldFrameData data = dict[key];

                log += string.Format("FrameIdx:{0} Data:{1}", key, data.ToString())+"\n\n";
            }

            return log;
        }

        
    }
}
