using UnityEngine;
using System.Collections;

public class UICameraControl : MonoBehaviour
{
    public static UICameraControl Instance;
    private Camera m_Camera;
    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
        Instance = this;
    }

    public Camera GetCamera()
    {
        return m_Camera;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
