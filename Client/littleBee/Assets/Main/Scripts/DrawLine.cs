using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Starry
{
    public class DrawLine : MonoBehaviour
    {
        LineRenderer lineRenderer;
        public float r=10f;
        public Vector3 pos = Vector3.zero;
        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 360;
      
        }
        private void Update()
        {          
            for(int d = 0;d<360;++d)
            {
                float rad = Mathf.Deg2Rad * d;
                float x = Mathf.Cos(rad) * r;
                float z = Mathf.Sin(rad) * r;
                Vector3 p = new Vector3(x, 0, z);
                lineRenderer.SetPosition(d, p);
            }
        }

        private void CreateLine()
        {
            
        }

       
    }
}