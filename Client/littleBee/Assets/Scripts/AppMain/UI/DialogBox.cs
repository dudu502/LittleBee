using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Localization;

public class DialogBox : MonoBehaviour
{
    private static DialogBox _Inst = null;
    public GameObject m_MaskGround;
    public Button m_ButtonConfirm;
    public Button m_ButtonCancel;
    public Text m_TextTitle;
    public Text m_TextContent;
    private Action<SelectType> m_ActionCaller;
    public enum SelectType
    {
        None = 0,
        Confirm = 1,
        Cancel = 2,
        All= 3,
    }
    public static void Init()
    {
        _Inst = GameObject.FindObjectOfType<DialogBox>();
        if(_Inst!=null)
        {
            _Inst.m_ButtonConfirm.onClick.AddListener(() => { _Inst.m_ActionCaller?.Invoke(SelectType.Confirm); _Inst.m_MaskGround.SetActive(false); });
            _Inst.m_ButtonCancel.onClick.AddListener(() => { _Inst.m_ActionCaller?.Invoke(SelectType.Cancel); _Inst.m_MaskGround.SetActive(false); });
            _Inst.m_MaskGround.SetActive(false);
        }

    }
    public static void Show(string title,string content, SelectType selectType,Action<SelectType> onActionCaller)
    {
        _Inst._Show(title, content, selectType, onActionCaller);
    }

    void _Show(string title,string content, SelectType selectType, Action<SelectType> onActionCaller)
    {
        m_MaskGround.SetActive(true);
        transform.localScale = Vector3.one * 1.05f;
        transform.DOScale(Vector3.one, 0.2f);
        m_ActionCaller = onActionCaller;
        m_TextTitle.text = title;
        m_TextContent.text = content;
        m_ButtonConfirm.gameObject.SetActive(SelectType.Confirm == (selectType & SelectType.Confirm));
        m_ButtonCancel.gameObject.SetActive(SelectType.Cancel == (selectType & SelectType.Cancel));
        m_ButtonCancel.SetButtonText(Language.GetText(16));
        m_ButtonConfirm.SetButtonText(Language.GetText(14));
    }
}

