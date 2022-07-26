using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemRender : DynamicInfinityItem
{
    public Text m_TxtName;

    public Button m_Btn;
	// Use this for initialization
	void Start () {
		m_Btn.onClick.AddListener(() =>
		{
            print("Click "+mData.ToString());
		});
	}

    protected override void OnRenderer()
    {
        base.OnRenderer();
        m_TxtName.text = mData.ToString();
    }
}
