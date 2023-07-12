using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Synchronize.Game.Lockstep.Managers.UI
{
    public class UIView:MonoBehaviour
    {
        [HideInInspector]
        public Layer CurrentLayer;
        private Animator _uiAnimator; 
        private byte _disableState = 0;
        private const string ANI_OPEN_NAME = "open";
        private const string ANI_CLOSE_NAME = "close";
        public virtual void OnInit() 
        {
            _uiAnimator = GetComponent<Animator>();
            if (_uiAnimator != null)
                _uiAnimator.Play(ANI_OPEN_NAME);
        }
        
        public virtual void OnShow(object obj)
        {

        }
        public virtual void OnClose()
        {
            _disableState = 0;
            if (_uiAnimator != null)
                _uiAnimator.Play(ANI_CLOSE_NAME);
            else
                Destroy(gameObject);
        }
        public void OnCloseAnimationStop()
        {
            if(_disableState == 0)
                Destroy(gameObject);
        }
        public virtual void OnPause()
        {
            _disableState = 1;
            if (_uiAnimator != null)
                _uiAnimator.Play(ANI_CLOSE_NAME);
            else
                gameObject.SetActive(false);
        }
        public virtual void OnResume()
        {
            gameObject.SetActive(true);
            if (_uiAnimator != null)
                _uiAnimator.Play(ANI_OPEN_NAME);
        }
    }
}
