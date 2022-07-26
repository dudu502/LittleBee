using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopListExample : MonoBehaviour
{
    public DynamicInfinityListRenderer m_Dl;

    public Button m_BtnSetDatas;

    public Button m_BtnMove2Data;

    public Button m_BtnRemoveData;

    public Button m_BtnAddData;
    // Use this for initialization
    void Start () {
	    m_Dl.InitRendererList(OnSelectHandler,null);
        m_BtnSetDatas.onClick.AddListener(() =>
        {
            List<int> datas = new List<int>();
            for (int i = 0; i < 500; ++i)
            {
                datas.Add(i);
            }
            m_Dl.SetDataProvider(datas);

        });

	    m_BtnMove2Data.onClick.AddListener(() =>
	    {
	        if (m_Dl.GetDataProvider() != null)
	        {
	            m_Dl.LocateRenderItemAtTarget(24, 1);
	        }
	        else
	        {
	            print("先设置数据吧");
            }

	    });

	    m_BtnRemoveData.onClick.AddListener(() =>
	    {
	        if (m_Dl.GetDataProvider() != null)
	        {
	            if (m_Dl.GetDataProvider().Contains(6))
	            {
	                m_Dl.GetDataProvider().Remove(6);
	                m_Dl.RefreshDataProvider();
	            }
	            else
	            {
	                print("找不到数据");
	            }
            }
	        else
	        {
	            print("先设置数据吧");
	        }	                     
	    });

        m_BtnAddData.onClick.AddListener(() =>
        {
            if (m_Dl.GetDataProvider() != null)
            {
                m_Dl.GetDataProvider().Add(999);
                m_Dl.RefreshDataProvider();
            }
            else
            {
                print("先设置数据吧");
            }
        });
    }

    void OnSelectHandler(DynamicInfinityItem item)
    {
        print("on select "+item.ToString());
    }

    // Update is called once per frame
    void Update () {
		
	}
}
