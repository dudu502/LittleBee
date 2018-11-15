using UnityEngine;
using System.Collections;

public class ObjectPoolContainer : MonoBehaviour
{
    public static ObjectPoolContainer Instance;
    public Transform m_TransGo;
    private void Awake()
    {
        Instance = this;
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
