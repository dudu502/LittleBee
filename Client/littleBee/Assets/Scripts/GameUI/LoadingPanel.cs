using UnityEngine;
using System.Collections;
using Managers.UI;
using Localization;
using Misc;
using UnityEngine.UI;
using Evt;
using Managers;

public class LoadingPanel : UIView, ILanguageApplicable
{
    public class LoadingInfo
    {
        public float Percent;
        public string Title;
        public LoadingInfo(string t, float p)
        {
            Title = t;
            Percent = p;
        }
    }
    public enum EventType
    {
        UpdateLoading,
        ClosePanel,
    }

    public Text m_TextLoadingHint;
    public Image m_ImgLoadingAsset;
    private LoadingInfo _loading;
    void Awake()
    {
        EnableUIFadeEffect = false;
        EventMgr<EventType, LoadingInfo>.AddListener(EventType.UpdateLoading, OnUpdateLoadingInfo);
        EventMgr<EventType, object>.AddListener(EventType.ClosePanel, OnCloseLoadingInfo);
    }
    void Start()
    {
        ApplyLocalizedLanguage();
    }

    public override void OnClose()
    {
        base.OnClose();
        EventMgr<EventType, LoadingInfo>.RemoveListener(EventType.UpdateLoading, OnUpdateLoadingInfo);
        EventMgr<EventType, object>.RemoveListener(EventType.ClosePanel, OnCloseLoadingInfo);
    }
    void OnUpdateLoadingInfo(LoadingInfo loadingInfo)
    {
        _loading = loadingInfo;
    }
    void OnCloseLoadingInfo(object obj)
    {
        ModuleManager.GetModule<UIModule>().Pop(Layer.Top,this);
    }
    void Update()
    {
        if (_loading != null)
        {
            m_ImgLoadingAsset.fillAmount = _loading.Percent;
            m_TextLoadingHint.text = _loading.Title + $" {UnityEngine.Mathf.CeilToInt(_loading.Percent * 100)}%";
        }
    }
    public override void OnShow(object paramObject)
    {
        base.OnShow(paramObject);
        OnUpdateLoadingInfo(paramObject as LoadingInfo);
    }
    public void ApplyLocalizedLanguage()
    {

    }
}
