

using Synchronize.Game.Lockstep.Ecsr.Renderer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Managers {
    public class GameContentRootModule : MonoBehaviour, IModule
    {
        public Transform TransformWorld { private set; get; }
        public TrajectoryRenderer Trajectory;
        private void Awake()
        {
            TransformWorld = transform.Find("World");
            Trajectory = transform.Find("Global/TrajectoryRenderer").GetComponent<TrajectoryRenderer>();

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        public void Init()
        {

        }
        public void DestroyChildren()
        {
            for (int i = TransformWorld.childCount - 1; i > -1; --i)
            {
                var child = TransformWorld.GetChild(i);

                if (child != null && child.gameObject != null)
                    Destroy(child.gameObject);
            }
        }
        public void SetWorldEnable(bool value)
        {
            if (TransformWorld != null)
            {
                TransformWorld.gameObject.SetActive(value);
            }
        }
        public void AddChild(GameObject go)
        {
            if (go != null && gameObject != null)
            {
                go.transform.SetParent(TransformWorld);
                Common.Utils.SetChildrenLayer(go, gameObject.layer);
            }
        }
        // Update is called once per frame
        void Update()
        {
#if MAIN_THREAD_MODE
        if (!SimulationManager.Instance.m_StopState)
            SimulationManager.Instance.Run();
#endif
        }
    }
}
