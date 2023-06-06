
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Synchronize.Game.Lockstep.Logger
{
    /// <summary>
    /// Logger
    /// </summary>
    public class GameEntityWorldLog
    {   
        public static string Write(Dictionary<int,EntityWorldFrameData> dict,string name)
        {
            string log = "[GameEntityInfoLog Name:"+name+"\n";
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
