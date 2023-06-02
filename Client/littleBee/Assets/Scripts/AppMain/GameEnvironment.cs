using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Misc { 
public enum CameraControlType
{
    Cinemachine,
    Normal,
}

    public class GameEnvironment : MonoBehaviour
    {
        public static GameEnvironment Instance { private set; get; }

        public CameraControlType m_CameraControlType = CameraControlType.Cinemachine;
        public Camera m_MainCamera;
        public Camera m_BackgroundCamera;

        public enum State
        {
            InBattle,
            OutBattle,
        }


        private void Awake()
        {
            Instance = this;
            ResetCameraState();
        }

        public void ResetCameraState()
        {
            if (m_CameraControlType == CameraControlType.Normal)
            {
                m_MainCamera.orthographicSize = 100;
                m_MainCamera.transform.localPosition = new Vector3(0, 100, 0);
            }
        }



        /// <summary>
        /// camera follow transform;
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="isSelf"></param>
        public void SetCameraFollow(Transform transform, bool isSelf)
        {

        }
        public void SetState(State state)
        {
            m_BackgroundCamera.gameObject.SetActive(state == State.InBattle);
        }
        // Start is called before the first frame update
        void Start()
        {
            SetState(State.OutBattle);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
