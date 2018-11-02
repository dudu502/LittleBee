using UnityEngine;
using System.Collections;

public class AllUI : MonoBehaviour
{
    public static AllUI Instance;
    public GameObject[] m_Panels;
    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start()
    {

    }
    public void Show(string gameobjname)
    {
        foreach (GameObject item in m_Panels)
            item.SetActive(item.name == gameobjname);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
