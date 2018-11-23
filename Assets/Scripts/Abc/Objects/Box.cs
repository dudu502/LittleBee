using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Renderers;

public class Box : MonoBehaviour
{
    #region Recycle
    public static ObjectPool ObjectPool = new ObjectPool(
        () =>
        {
            GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load("Box") as GameObject);
            return obj;
        },
        (bullet) => {
            GameObject obj = bullet as GameObject;
        },
        (bullet) => {
            GameObject obj = bullet as GameObject;           
        });
    #endregion

    public Image m_ImgLife;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_ImgLife.fillAmount =1- GetComponent<FrameClockActionRenderer>().m_Rate;
    }
}
