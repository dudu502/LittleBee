using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Managers.UI
{
    public class UIView:MonoBehaviour
    {
        protected bool EnableUIFadeEffect = true;
        public virtual void OnInit() 
        {
            if (EnableUIFadeEffect)
            {
                transform.localScale = 0.9f * Vector3.one;
                transform.DOScale(Vector3.one, 0.2f);
            }
     
        }
        

        public virtual void OnShow(object obj)
        {

        }
        public virtual void OnClose()
        {
            Destroy(gameObject);
        }
        public virtual void OnPause()
        {
            gameObject.SetActive(false);
        }
        public virtual void OnResume()
        {
            gameObject.SetActive(true);
            
        }

    }
}
