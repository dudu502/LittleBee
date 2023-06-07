using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Localization
{
    public sealed class Localization
    {
        private class LanguageTagAttribute : Attribute
        {
            public string Tag;

            public LanguageTagAttribute(string tag)
            {
                Tag = tag;

            }
        }

        public enum Language
        {
            [LanguageTag("None")]
            None,
            [LanguageTag("简体中文")]
            ChineseSimplified,
            [LanguageTag("English")]
            English,
        }
        private static Dictionary<Language, Dictionary<string, string>> _languageMap;
        public static Language CurrentLanguage { private set; get; }
        public static event Action<Language> OnLanguageChanged;
        public static void Initialize()
        {
            _languageMap = new Dictionary<Language, Dictionary<string, string>>();

            CurrentLanguage = GetCurrentLanguage();
            
            LoadLanguage(CurrentLanguage);
        }
        static void LoadLanguage(Language lan)
        {
            if (!_languageMap.ContainsKey(lan) && lan!= Language.None)
            {
                var path = $"Configs/Static/Localization/{lan.ToString()}";
                TextAsset txtAsset = Resources.Load<TextAsset>(path);
                var map = JsonConvert.DeserializeObject<Dictionary<string, string>>(txtAsset.text);
                _languageMap[lan] = map;
                OnLanguageChanged?.Invoke(lan);
            }
        }

        public static Language[] GetSupportedLanguages()
        {
            return new Language[]{
                Language.ChineseSimplified,
                Language.English };
        }

        public static string GetLanguageTag(Language lan)
        {
            LanguageTagAttribute tag = typeof(Language).GetField(lan.ToString()).GetCustomAttributes(typeof(LanguageTagAttribute), true).FirstOrDefault() as LanguageTagAttribute;
            return tag.Tag;
        }
        
        public static Language GetCurrentLanguage()
        {
            return (Language)PlayerPrefs.GetInt("CurrentLanguage");
        }

        public static void SetLanguage(Language lan)
        {
            PlayerPrefs.SetInt("CurrentLanguage", (int)lan);
            CurrentLanguage = lan;
            if (!_languageMap.ContainsKey(lan))
                LoadLanguage(lan);
            else
                OnLanguageChanged?.Invoke(lan);
        }

        public static string GetTranslation(string key)
        {
            if (_languageMap.ContainsKey(CurrentLanguage))
            {
                if (_languageMap[CurrentLanguage].TryGetValue(key.ToLower(), out var value))
                    return value;
            }
            return key;
        }
    }
}
