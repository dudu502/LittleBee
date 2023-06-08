using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Synchronize.Game.Lockstep.Managers.UI;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Notification;

namespace Synchronize.Game.Lockstep.UI
{
    public class SettingPanel : UIView
    {
        public TMPro.TMP_Text m_TxtTitle;
        public Button m_BtnBack;
        public Button m_BtnSave;
        public UserSettingItem[] m_SettingLists;
        public Transform m_SettngContentTransform;


        public override void OnInit()
        {
            base.OnInit();
            m_BtnBack.onClick.AddListener(() =>
            {
                ModuleManager.GetModule<UIModule>().Pop(Layer.Bottom);
            });

            m_BtnSave.onClick.AddListener(() => SaveSetting());
        }

        public override void OnClose()
        {
            base.OnClose();
            m_BtnBack.onClick.RemoveAllListeners();
            m_BtnSave.onClick.RemoveAllListeners();
        }
        void SetSetting()
        {
            for (int i = 0; i < m_SettingLists.Length; ++i)
            {
                var settingItem = m_SettingLists[i];
                if (i < UserSettingMgr.SettingList.Count)
                {
                    settingItem.gameObject.SetActive(true);
                    settingItem.SetSettingData(UserSettingMgr.SettingList[i]);
                }
                else
                {
                    settingItem.gameObject.SetActive(false);
                }
            }
        }
        void Start()
        {
            SetSetting();
        }
        public override void OnShow(object paramObject)
        {
            base.OnShow(paramObject);

        }

        void SaveSetting()
        {
            UserSettingItem[] settingItems = gameObject.GetComponentsInChildren<UserSettingItem>();
            List<UserSettingData> settingDatas = new List<UserSettingData>();

            foreach (UserSettingItem userSettingItem in settingItems)
            {
                var settingData = userSettingItem.GetSettingData();
                settingDatas.Add(settingData);
            }
            string json = LitJson.JsonMapper.ToJson(settingDatas);
            print(json);
            System.IO.File.WriteAllText(Application.persistentDataPath + "\\setting.json", json);
            UserSettingMgr.SettingList = settingDatas;
            ToastManager.Instance.ShowToast(Localization.Localization.GetTranslation("Saved successfully"));
        }

    }
}