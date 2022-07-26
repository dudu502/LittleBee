using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Localization;
using Misc;
using Service.HttpMisc;
using MappingData;
using Proxy;
using Managers;
using Managers.UI;

public class LoginAlert : MonoBehaviour, ILanguageApplicable
{
    public Text m_TxtTitle;
    public Text m_TxtName;
    public InputField m_InputName;
    public Text m_TxtPwd;
    public InputField m_InputPwd;
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
            ToastRoot.Instance.ShowToast(Language.GetText(44));
        }
        else
        {
#if NONE_DB_SERVER
            DataProxy.Get<UserDataProxy>().UserLoginInfo = new LoginJsonResult(){
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
    private void OnEnable()
    {
        ApplyLocalizedLanguage();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void ApplyLocalizedLanguage()
    {
        m_TxtTitle.text = Language.GetText(13);
        m_TxtName.text = Language.GetText(17);
        m_TxtPwd.text = Language.GetText(18);
        m_BtnConfirm.SetButtonText(Language.GetText(14));
        m_BtnCancel.SetButtonText(Language.GetText(16));

    }

}
