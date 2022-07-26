using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{

    public class Utils
    {
        public static string GuidToString()
        {
            return System.Guid.NewGuid().ToString();            
        }
        public static void SetChildrenLayer(UnityEngine.GameObject gameObject, int layer)
        {
            gameObject.layer = layer;

            var transforms = gameObject.GetComponentsInChildren<UnityEngine.Transform>(true);
            foreach(var transform in transforms)
            {
                transform.gameObject.layer = layer;
            }
        }

        public static Vector3 WorldToUI(Camera camera, RectTransform rect, Vector3 pos)
        {
            //CanvasScaler scaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();

            //float resolutionX = scaler.referenceResolution.x;
            //float resolutionY = scaler.referenceResolution.y;


            //Vector3 viewportPos = camera.WorldToViewportPoint(pos);

            //Vector3 uiPos = new Vector3(viewportPos.x * resolutionX - resolutionX * 0.5f,
            //    viewportPos.y * resolutionY - resolutionY * 0.5f, 0);

            //return uiPos;

            Vector2 screenPoint = camera.WorldToScreenPoint(pos);
            Vector2 uipos ;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, null, out uipos);
            return uipos;
        }
    }
}