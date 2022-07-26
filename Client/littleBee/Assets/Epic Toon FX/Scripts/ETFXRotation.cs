using UnityEngine;
using System.Collections;
 
namespace EpicToonFX
{
    public class ETFXRotation : MonoBehaviour
    {
 
        [Header("Rotate axises by degrees per second")]
        public Vector3 rotateVector = Vector3.zero;
 
        public enum spaceEnum { Local, World };
        public spaceEnum rotateSpace;
 
        // Use this for initialization
        void Start()
        {
 
        }
 
        // Update is called once per frame
        void Update()
        {
            if (rotateSpace == spaceEnum.Local)
                transform.Rotate(rotateVector * Time.deltaTime);
            if (rotateSpace == spaceEnum.World)
                transform.Rotate(rotateVector * Time.deltaTime, Space.World);
        }
    }
}