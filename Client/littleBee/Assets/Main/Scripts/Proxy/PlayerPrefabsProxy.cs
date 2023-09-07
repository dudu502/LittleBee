using UnityEngine;

namespace Synchronize.Game.Lockstep.Proxy
{
    public class PlayerPrefabsProxy:DataProxy
    {
        protected override void OnInit()
        {
            base.OnInit();
        }
        public T GetObject<T>(string key)
        {
            return LitJson.JsonMapper.ToObject<T>(PlayerPrefs.GetString(key));
        }
        public void SetObject<T>(string key,T obj)
        {
            PlayerPrefs.SetString(key, LitJson.JsonMapper.ToJson(obj));
        }

        public string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }
        public void SetString(string key,string value)
        {
            PlayerPrefs.SetString(key, value);
        }
    }
}
