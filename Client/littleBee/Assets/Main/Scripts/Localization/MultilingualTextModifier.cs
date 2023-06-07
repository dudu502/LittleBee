using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Localization
{
    public class MultilingualTextModifier : MonoBehaviour
    {
        private Localization.Language _currentLanguage = Localization.Language.None;
        Dictionary<TMPro.TMP_Text, string> _textComponentMap;
        Dictionary<TMPro.TMP_Dropdown, List<string>> _dropdownComponentMap;
        private void OnEnable()
        {
            if (_textComponentMap == null)
            {
                GenerateTextMap();
            }

            if (_dropdownComponentMap == null)
            {
                GenerateDropdownComponentMap();
            }
            Localization.OnLanguageChanged += OnLanguageChanged;
            if (_currentLanguage != Localization.CurrentLanguage)
            {
                OnLanguageChanged(Localization.CurrentLanguage);
            }
        }

        private void OnLanguageChanged(Localization.Language lan)
        {
            if (_currentLanguage != lan)
            {
                foreach (var textComponent in _textComponentMap.Keys)
                {
                    if (textComponent != null && !textComponent.IsDestroyed() && _textComponentMap.TryGetValue(textComponent, out var text) && !string.IsNullOrEmpty(text) && textComponent.GetComponent<IgnoreMultilingual>() == null)
                    {
                        textComponent.text = Localization.GetTranslation(text);
                    }
                }

                foreach (var dropdownComponent in _dropdownComponentMap.Keys)
                {
                    if (dropdownComponent != null && !dropdownComponent.IsDestroyed() && _dropdownComponentMap.TryGetValue(dropdownComponent, out var originalOptions) && originalOptions.Count == dropdownComponent.options.Count)
                    {
                        for (int i = 0; i < dropdownComponent.options.Count; ++i)
                        {
                            dropdownComponent.options[i].text = Localization.GetTranslation(originalOptions[i]);
                        }
                        dropdownComponent.captionText.text = Localization.GetTranslation(originalOptions[dropdownComponent.value]);
                    }
                }

                _currentLanguage = lan;
            }
        }

        void GenerateTextMap()
        {
            _textComponentMap = new Dictionary<TMPro.TMP_Text, string>();
            var allTextComponents = gameObject.GetComponentsInChildren<TMPro.TMP_Text>(true);
            foreach (var textComponent in allTextComponents)
            {
                _textComponentMap[textComponent] = textComponent.text;
            }
        }


        void GenerateDropdownComponentMap()
        {
            _dropdownComponentMap = new Dictionary<TMPro.TMP_Dropdown, List<string>>();
            var allDropdownComponents = gameObject.GetComponentsInChildren<TMPro.TMP_Dropdown>(true);
            foreach (var dropdownComponent in allDropdownComponents)
            {
                List<string> options = new List<string>();
                foreach (var dropdownOption in dropdownComponent.options)
                    options.Add(dropdownOption.text);
                _dropdownComponentMap[dropdownComponent] = options;
            }
        }
        private void OnDisable()
        {
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }
    }
}