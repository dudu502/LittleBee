using Synchronize.Game.Lockstep.Config.Static;
using Synchronize.Game.Lockstep.Managers;

namespace Synchronize.Game.Lockstep.Localization
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
