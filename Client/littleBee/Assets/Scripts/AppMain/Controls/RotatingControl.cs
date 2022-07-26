using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotatingControl : EventTrigger
{

    Vector2 m_RotateDragOffset;
    // Use this for initialization
    void Start () {
		
	}

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        var centerPos = RectTransformUtility.WorldToScreenPoint(UICameraControl.Instance.GetCamera(), transform.position);
        var newAngleV = GetMousePos() - centerPos;
        int state = 0;
        float dotResult = Vector2.Dot((newAngleV - m_RotateDragOffset), GetCrossVector2());
        if (dotResult > 0)
            state = 1;
        else if(dotResult<0)
            state = -1;

        print("OnDrag " + state * Vector2.Angle(m_RotateDragOffset, newAngleV));
        transform.RotateAround(transform.position, new Vector3(0, 0, 1), state * Vector2.Angle(m_RotateDragOffset, newAngleV));
        m_RotateDragOffset = GetMousePos() - centerPos;

    }
    Vector2 GetCrossVector2()
    {
        var v1 = GetMousePos() - RectTransformUtility.WorldToScreenPoint(UICameraControl.Instance.GetCamera(), transform.position);
        Vector3 rrV3 = new Vector3(v1.x, v1.y, 0);
        Vector3 zV3 = new Vector3(0, 0, -1);
        var cross = Vector3.Cross(rrV3, zV3);
        return cross;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        m_RotateDragOffset = GetMousePos() - RectTransformUtility.WorldToScreenPoint(UICameraControl.Instance.GetCamera(), transform.position);

        print("OnBeginDrag "+eventData.ToString());
    }
    Vector2 GetMousePos()
    {
        return new Vector2(Input.mousePosition.x,Input.mousePosition.y);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        print("OnEndDrag "+eventData);
    }
    // Update is called once per frame
    void Update () {
        
    }
}
