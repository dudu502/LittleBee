
using Synchronize.Game.Lockstep.Ecsr.Renderer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Synchronize.Game.Lockstep.UI
{
    public class HudRender : ActionRenderer
    {
        [SerializeField] TMP_Text m_NameText;
        [SerializeField] Image m_HpProcessImage;
        [SerializeField] Image m_MpProcessImage;
        void Start()
        {
            SetHpEnable(false);
            SetMpEnable(false);
        }
        public void SetName(string value)
        {
            m_NameText.text = value;
        }


        public void SetHp(float value)
        {
            m_HpProcessImage.fillAmount = value;
            m_HpProcessImage.color = Color.Lerp(Color.red, Color.green, value);
        }
        public void SetMp(float value)
        {
            m_MpProcessImage.fillAmount = value;
        }
        public void SetHpEnable(bool value)
        {
            m_HpProcessImage.gameObject.SetActive(value);
            m_HpProcessImage.transform.parent.gameObject.SetActive(value);
        }
        public void SetMpEnable(bool value)
        {
            m_MpProcessImage.gameObject.SetActive(value);
            m_MpProcessImage.transform.parent.gameObject.SetActive(value);
        }

    }
}