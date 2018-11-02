using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using NetServiceImpl;
using NetServiceImpl.Client;

public class PanelLan : MonoBehaviour
{
    public Button m_BtnCreate;
    public InputField m_Field;

    // Use this for initialization
    void Start()
    {
        m_BtnCreate.onClick.AddListener(()=> 
        {
            var name = m_Field.text;
            Service.Get<LoginService>().RequestEnterRoom(name);
            AllUI.Instance.Show("PanelCreation");
        });
        
    }

    // Update is called once per frame
    void Update()
    {

    }


}
