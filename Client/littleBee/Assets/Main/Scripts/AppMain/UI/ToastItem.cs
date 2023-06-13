using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Synchronize.Game.Lockstep.Notification
{
    public class ToastItem : MonoBehaviour
    {
        static readonly WaitForSeconds s_wait = new WaitForSeconds(2);
        public TMPro.TMP_Text m_Text;
        public bool IsIdle = true;

        public void SetText(string msg, Action<ToastItem> onDisplayFinish)
        {
            gameObject.SetActive(true);
            IsIdle = false;
            m_Text.text = msg;
            StartCoroutine(_Delay(onDisplayFinish));
        }
        IEnumerator _Delay(Action<ToastItem> action)
        {
            yield return s_wait;
            gameObject.SetActive(false);
            action?.Invoke(this);
            IsIdle = true;
        }
    }
}