using UnityEngine;
using System.Collections;
using Unity.Mathematics;

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
    float3x3 float3x3_translation = new float3x3();

    float3x3 float3x3_scale = new float3x3();

    float3x3 float3x3_rotation = new float3x3();
    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("Translate")]
    void Teset()
    {
        float3x3_translation.c0 = new float3(1, 0, -4);
        float3x3_translation.c1 = new float3(0, 1, -5);
        float3x3_translation.c2 = new float3(0, 0, 1);

        var i = new float3(100, 100, 1);

        var sult = math.mul(i, float3x3_translation);

        print(sult);
    }

   
}
