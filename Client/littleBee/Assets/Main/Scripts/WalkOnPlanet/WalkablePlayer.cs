using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkablePlayer : MonoBehaviour
{
    public Transform planetTrans;
    public float moveSpeed = 0.01f;
    public float rotateSpeed = 6;
    public float radius = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (planetTrans == null) return;
        Vector2 dir = Vector2.zero;
        if (InputManager.Instance.IsKeyCodeActive(KeyCode.A))
        {
            dir.x = -1;
        }
        if (InputManager.Instance.IsKeyCodeActive(KeyCode.S))
        {
            dir.y = -1;
        }
        if (InputManager.Instance.IsKeyCodeActive(KeyCode.W))
        {
            dir.y = 1;
        }
        if (InputManager.Instance.IsKeyCodeActive(KeyCode.D))
        {
            dir.x = 1;
        }
        if (!InputManager.Instance.IsAnyKeyCodeActive())
        {
            dir.x = 0;
            dir.y = 0;
        }
        Vector2 dirNormal = dir.normalized * moveSpeed;

        transform.rotation *= Quaternion.Euler(0, rotateSpeed * dir.x, 0);
        Vector3 gravityDir = transform.position - planetTrans.position;
        Vector3 nextPos = transform.position + transform.TransformDirection(new Vector3(0, 0, dirNormal.y));

        transform.position = (nextPos - planetTrans.position).normalized * radius + planetTrans.position;
        transform.rotation = Quaternion.FromToRotation(transform.up, gravityDir.normalized) * transform.rotation;

    }

    [ContextMenu("Refresh Position")]
    void RefreshPosition()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
        transform.position = planetTrans.position + randomDirection * radius;
    }
}
