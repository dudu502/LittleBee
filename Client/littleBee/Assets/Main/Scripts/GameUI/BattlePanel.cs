using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;
using Synchronize.Game.Lockstep.Managers.UI;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Behaviours;
using Synchronize.Game.Lockstep.Notification;

namespace Synchronize.Game.Lockstep.UI
{
    public class BattlePanel : UIView
    {
        public Button m_FireButton;
        public Button m_BackButton;
        public TMP_Text m_TxtLog;

        public GameObject m_GoJoystick;

        public GameObject m_GoReplayProgress;
        public Image m_ReplayProgressValue;
        public Slider m_ReplaySpeedSlider;
        private PlayBattleMode m_PlayMode;
        public override void OnInit()
        {
            base.OnInit();
            m_FireButton.onClick.AddListener(() =>
            {
                FunctionButtonInputManager.Instance.Func = FunctionButtonInputManager.Function.FIRE;
            });
            m_BackButton.onClick.AddListener(() =>
            {
                if (m_PlayMode == PlayBattleMode.PlayRealBattle)
                {
                    NotificationManager.Instance.Show(NotificationType.Warning, option =>
                    {
                        if (option == NotificationOption.OK)
                        {
                            BattleEntryPoint.Stop();
                            ModuleManager.GetModule<UIModule>().Pop(Layer.Bottom);
                        }
                    }, item =>
                    {
                        item.TitleKey = string.Format(item.TitleKey, Localization.Localization.GetTranslation("Tips"));
                        item.DescriptionKey = string.Format(item.DescriptionKey, Localization.Localization.GetTranslation("Do you really want to quit?"));
                    });
                }
                else if (m_PlayMode == PlayBattleMode.PlayReplayBattle)
                {
                    BattleEntryPoint.StopReplay();
                    ModuleManager.GetModule<UIModule>().Pop(Layer.Bottom);
                }
            });

            m_ReplaySpeedSlider.onValueChanged.AddListener(value => SimulationManager.Instance.UpdateFrameMsLength(value));
            Evt.EventMgr<EvtReplay, float>.AddListener(EvtReplay.UpdateFrameCount, OnUpdateReplayProgress);
        }

        void OnUpdateReplayProgress(float value)
        {
            Handler.Run(_ => m_ReplayProgressValue.fillAmount = value, null);
        }
        public override void OnShow(object obj)
        {
            base.OnShow(obj);
            m_PlayMode = (PlayBattleMode)obj;
            m_GoJoystick.SetActive(m_PlayMode == PlayBattleMode.PlayRealBattle);
            m_FireButton.gameObject.SetActive(m_PlayMode == PlayBattleMode.PlayRealBattle);
            m_GoReplayProgress.SetActive(m_PlayMode == PlayBattleMode.PlayReplayBattle);
            m_ReplaySpeedSlider.gameObject.SetActive(m_PlayMode == PlayBattleMode.PlayReplayBattle);
            GameEnvironment.Instance.ResetCameraState();
        }
        public override void OnClose()
        {
            base.OnClose();
            Evt.EventMgr<EvtReplay, float>.RemoveListener(EvtReplay.UpdateFrameCount, OnUpdateReplayProgress);
            m_FireButton.onClick.RemoveAllListeners();
            m_BackButton.onClick.RemoveAllListeners();
            m_ReplaySpeedSlider.onValueChanged.RemoveAllListeners();
        }
        private void OnEnable()
        {
            //GameContentRoot.Instance.SetWorldEnable(true);
            ModuleManager.GetModule<GameContentRootModule>().SetWorldEnable(true);
        }

        private void Update()
        {
            Simulation sim = SimulationManager.Instance.GetSimulation();
            if (sim == null) return;
            string str = "";
            if (m_PlayMode == PlayBattleMode.PlayRealBattle)
            {
                LogicFrameBehaviour logic = sim.GetBehaviour<LogicFrameBehaviour>();
                if (logic == null) return;

                lock (sim.GetEntityWorld())
                {
                    str += "CurrentFrameIdx " + logic.CurrentFrameIdx + "\nDateTime " + DateTime.Now.ToString() + "\nEntityCount " + sim.GetEntityWorld().GetEntityCount();
                    m_TxtLog.text = str;
                }

                if (Input.GetKeyDown(KeyCode.J))
                {
                    FunctionButtonInputManager.Instance.Func = FunctionButtonInputManager.Function.FIRE;
                }
            }
            else if (m_PlayMode == PlayBattleMode.PlayReplayBattle)
            {
                ReplayLogicFrameBehaviour replayLogic = sim.GetBehaviour<ReplayLogicFrameBehaviour>();
                if (replayLogic == null) return;
                lock (sim.GetEntityWorld())
                {
                    str += "CurrentFrameIdx " + replayLogic.CurrentFrameIdx + "\nDateTime " + DateTime.Now.ToString() + "\nEntityCount " + sim.GetEntityWorld().GetEntityCount();
                    m_TxtLog.text = str;
                }
            }

        }
    }
}