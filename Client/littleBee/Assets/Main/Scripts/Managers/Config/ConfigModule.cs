
using Synchronize.Game.Lockstep.Config.Static;
using Synchronize.Game.Lockstep.Config.Static.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Managers
{
    public class ConfigModule : IModule
    {
        private static Dictionary<Type, IList> Configs = new Dictionary<Type, IList>();
        public void Init()
        {
            Configs[typeof(BattleshipCFG)] = LitJson.JsonMapper.ToObject<List<BattleshipCFG>>(Resources.Load<TextAsset>("Configs/Static/Battleships").text);
            Configs[typeof(MapElementCFG)] = LitJson.JsonMapper.ToObject<List<MapElementCFG>>(Resources.Load<TextAsset>("Configs/Static/MapElements").text);
            Configs[typeof(ResourceIdCFG)] = LitJson.JsonMapper.ToObject<List<ResourceIdCFG>>(Resources.Load<TextAsset>("Configs/Static/ResourceId").text);
            Configs[typeof(MapIdCFG)] = LitJson.JsonMapper.ToObject<List<MapIdCFG>>(Resources.Load<TextAsset>("Configs/Static/MapId").text);
        }
        public T GetConfig<T>(int configId) where T : ICFG
        {
            Type type = typeof(T);
            if (Configs.ContainsKey(type))
            {
                var list = Configs[type];
                foreach (T item in list)
                {
                    if (item.ConfigId == configId)
                        return item;
                }
            }
            return default;
        }

        public List<T> GetConfigs<T>() where T : ICFG
        {
            List<T> ret = null;
            Type type = typeof(T);
            if (Configs.ContainsKey(type))
            {
                return (List<T>)Configs[type];
            }
            return ret;
        }
    }
}
