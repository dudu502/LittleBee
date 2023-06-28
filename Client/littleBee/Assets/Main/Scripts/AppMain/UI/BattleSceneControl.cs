using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Synchronize.Game.Lockstep.Misc;
namespace Synchronize.Game.Lockstep.Controllers
{
    public class BattleSceneControl : MonoBehaviour
    {
        private string MouseScrollWheel = "Mouse ScrollWheel";
        Vector2 _screenPosBegin = Vector2.zero;
#if UNITY_ANDROID
    float _touchScaleValue = 0f;
#endif
        bool isMoving;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        float GetGameMainCameraAspect()
        {
            return 1f * Screen.width / Screen.height;
        }
        float GetGameMainCameraSize()
        {
            return GameEnvironment.Instance.m_MainCamera.orthographicSize;
        }
        void SetGameMainCameraSize(float value)
        {
            GameEnvironment.Instance.m_MainCamera.orthographicSize = Mathf.Max(1, value);
        }
        //https://blog.csdn.net/luoshao_/article/details/82801117
        void LateUpdate()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isMoving = true;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _screenPosBegin = Vector2.zero;
                    isMoving = false;
                }
                if (isMoving)
                {
                    if (_screenPosBegin != Vector2.zero)
                    {
                        Vector2 movement = GameEnvironment.Instance.m_MainCamera.ScreenToViewportPoint(Input.mousePosition) - GameEnvironment.Instance.m_MainCamera.ScreenToViewportPoint(_screenPosBegin);
                        GameEnvironment.Instance.m_MainCamera.transform.position -= new Vector3(movement.x * 2 * GetGameMainCameraSize() * GetGameMainCameraAspect(), 0, movement.y * 2 * GetGameMainCameraSize());
                        //GameEnvironment.Instance.m_MainCamera.transform.DOMove(GameEnvironment.Instance.m_MainCamera.transform.position - new Vector3(movement.x * 2 * GetGameMainCameraSize() * GetGameMainCameraAspect(), 0, movement.y * 2 * GetGameMainCameraSize()), 0.1f);
                    }
                    _screenPosBegin = Input.mousePosition;
                }
            }
            SetGameMainCameraSize(5 * Input.GetAxis(MouseScrollWheel) + GetGameMainCameraSize());
#elif UNITY_ANDROID
        //在Android的段运行时使用：
        if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))//没点到UI
        {
            Touch[] touches = Input.touches;
            if (touches.Length == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isMoving = true;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    _screenPosBegin = Vector2.zero;
                    isMoving = false;
                }
                if (isMoving)
                {
                    if (_screenPosBegin != Vector2.zero)
                    {
                        Vector2 movement = GameEnvironment.Instance.m_MainCamera.ScreenToViewportPoint(Input.GetTouch(0).position) - GameEnvironment.Instance.m_MainCamera.ScreenToViewportPoint(_screenPosBegin);
                        GameEnvironment.Instance.m_MainCamera.transform.position -= new Vector3(movement.x * 2 * GetGameMainCameraSize() * GetGameMainCameraAspect(), 0, movement.y * 2 * GetGameMainCameraSize());
                    }
                    _screenPosBegin = Input.mousePosition;
                }
            }
            else if (touches.Length == 2)
            {
                isMoving = false;
                _screenPosBegin = Vector2.zero;
                Vector2 twoFingerDir = GameEnvironment.Instance.m_MainCamera.ScreenToViewportPoint(Input.GetTouch(0).position) - GameEnvironment.Instance.m_MainCamera.ScreenToViewportPoint(Input.GetTouch(1).position);
                Debug.LogError("[Scale "+twoFingerDir);
                twoFingerDir.Set(twoFingerDir.x * GetGameMainCameraAspect(), twoFingerDir.y);
                Debug.LogError("]Scale " + twoFingerDir);
       
                if (_touchScaleValue!=0&&_touchScaleValue!=twoFingerDir.magnitude)
                {
                    if(_touchScaleValue > twoFingerDir.magnitude)
                        SetGameMainCameraSize(GetGameMainCameraSize() +twoFingerDir.magnitude);
                    else
                        SetGameMainCameraSize(GetGameMainCameraSize() -twoFingerDir.magnitude );
                }
                _touchScaleValue = twoFingerDir.magnitude;
            }
        }
#endif
        }
    }
}