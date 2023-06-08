using Synchronize.Game.Lockstep.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Synchronize.Game.Lockstep.UI
{
    public class UserSettingItem:MonoBehaviour
    {
        private TMP_InputField m_InputValue;
        private TMP_Text m_TxtTitle;
        private UserSettingData m_UserSettingData;
        private void Awake()
        {
            m_TxtTitle = transform.Find("Title").GetComponent<TMP_Text>();
            m_InputValue = transform.Find("InputField").GetComponent<TMP_InputField>();
            m_InputValue.onValueChanged.AddListener((value) => m_UserSettingData.m_SettingValue = value);
        }
        public void SetSettingData(UserSettingData userSettingData)
        {
            m_UserSettingData = userSettingData;
            UpdateView();
        }
        void UpdateView()
        {
            m_InputValue.gameObject.SetActive(true);
            if (!string.IsNullOrEmpty(m_UserSettingData.m_SettingValue))
                m_InputValue.text = m_UserSettingData.m_SettingValue;
            m_TxtTitle.text = m_UserSettingData.m_SettingTitle;           
        }
        public UserSettingData GetSettingData()
        {
            return m_UserSettingData;
        }

    }
}
