using Config.Static;
using Managers;
using Managers.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Localization
{
    
    public class Language
    {
        private static Misc.LanguageType LanguageType = Misc.LanguageType.English;
        public static void SetLanguge(Misc.LanguageType type)
        {
            LanguageType = type;
        }
        public static string GetText(int configId)
        {
            var cfg = ModuleManager.GetModule<ConfigModule>().GetConfig<LanguageCFG>(configId);
            if(cfg!=null)
            {
                if (LanguageType == Misc.LanguageType.Chinese)
                    return cfg.Chinese;
                else if (LanguageType == Misc.LanguageType.English)
                    return cfg.English;
                //todo
            }
            return "TODO MESSAGE "+ configId;
        }
    }
}
