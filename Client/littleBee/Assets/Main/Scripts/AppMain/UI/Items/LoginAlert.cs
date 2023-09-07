using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Data;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.Managers.UI;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Notification;
using Synchronize.Game.Lockstep.Proxy;

namespace Synchronize.Game.Lockstep.UI
{
    public class LoginAlert : MonoBehaviour
    {
        public TMPro.TMP_Text m_TxtTitle;
        public TMPro.TMP_Text m_TxtName;
        public TMPro.TMP_InputField m_InputName;
        public TMPro.TMP_Text m_TxtPwd;
        public TMPro.TMP_InputField m_InputPwd;
        public Button m_BtnConfirm;
        public Button m_BtnCancel;
        // Use this for initialization
        void Start()
        {
            m_BtnCancel.onClick.AddListener(() => gameObject.SetActive(false));
            m_BtnConfirm.onClick.AddListener(RequestLoginGate);

            string player_name = DataProxy.Get<PlayerPrefabsProxy>().GetString("player_name");
            m_InputName.text = string.IsNullOrEmpty(player_name) ? "Jerry" : player_name;
            m_InputPwd.text = "1";
        }
        void RequestLoginGate()
        {
            if (string.IsNullOrEmpty(m_InputName.text))
            {
                ToastManager.Instance.ShowToast(Localization.Localization.GetTranslation("Enter the user name"));
            }
            else
            {
#if NONE_DB_SERVER
                DataProxy.Get<UserDataProxy>().UserLoginInfo = new LoginJsonResult()
                {
                    results = new System.Collections.Generic.List<LoginJsonResultItem>(){
                    new LoginJsonResultItem(){
                        name=m_InputName.text,
                        pwd="1",
                        money = 100,
                        date="0000",
                        state = 1,
                    }
                }
                };
                print(DataProxy.Get<UserDataProxy>().UserLoginInfo.state);
                gameObject.SetActive(false);

                ModuleManager.GetModule<UIModule>().Push(UITypes.GatePanel, Layer.Bottom, null);
                DataProxy.Get<PlayerPrefabsProxy>().SetString("player_name", m_InputName.text);
#else
            if (!string.IsNullOrEmpty(m_InputPwd.text))
            {
                string url = DataProxy.Get<UserDataProxy>().WebServerAddress + "/login?name=" + m_InputName.text + "&password=" + "1";
                AsyncHttpTask.HttpGetRequest(url, (result_json) =>
                {
                    if (result_json != "")
                    {
                        var result = LitJson.JsonMapper.ToObject<LoginJsonResult>(result_json);
                        Debug.LogWarning(result_json);
                        if (result.results.Count > 0)
                        {
                            DataProxy.Get<UserDataProxy>().UserLoginInfo = result;
                            Debug.LogWarning(DataProxy.Get<UserDataProxy>().UserLoginInfo.state);
                            gameObject.SetActive(false);

                            ModuleManager.GetModule<UIModule>().Push(UITypes.GatePanel, Layer.Bottom, null);
                            DataProxy.Get<PlayerPrefabsProxy>().SetString("player_name", m_InputName.text);
                        }
                        else
                        {
                            DialogBox.Show(Language.GetText(22), Language.GetText(39), DialogBox.SelectType.All, option => {
                                if (option == DialogBox.SelectType.Confirm)
                                {
                                    string insertUser = DataProxy.Get<UserDataProxy>().WebServerAddress + "/insert_user_info?name=" + m_InputName.text + "&password=" + "1";
                                    Debug.LogWarning(insertUser);
                                    AsyncHttpTask.HttpGetRequest(insertUser, insert_result =>
                                    {
                                        Debug.LogWarning("insert success." + insert_result);
                                        RequestResult insert_Result = LitJson.JsonMapper.ToObject<RequestResult>(insert_result);
                                        if (insert_Result.IsSuccess())
                                            RequestLoginGate();
                                    });
                                }
                            });
                        }
                    }
                    else
                    {
                        Debug.Log("Error");
                    }
                });
            }
#endif
            }

        }
    }
}