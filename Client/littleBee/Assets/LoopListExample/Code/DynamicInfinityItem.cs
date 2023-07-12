using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 动态格子矩形
/// </summary>
public class DynamicRect
{
    /// <summary>
    /// 矩形数据
    /// </summary>
    private Rect mRect;
    /// <summary>
    /// 格子索引
    /// </summary>
    public int Index;
    public DynamicRect(float x, float y, float width, float height, int index)
    {
        this.Index = index;
        mRect = new Rect(x, y, width, height);
    }

    /// <summary>
    /// 是否相交
    /// </summary>
    /// <param name="otherRect"></param>
    /// <returns></returns>
    public bool Overlaps(DynamicRect otherRect)
    {
        return mRect.Overlaps(otherRect.mRect);
    }

    /// <summary>
    /// 是否相交
    /// </summary>
    /// <param name="otherRect"></param>
    /// <returns></returns>
    public bool Overlaps(Rect otherRect)
    {
        return mRect.Overlaps(otherRect);
    }
    public override string ToString()
    {
        return string.Format("index:{0},x:{1},y:{2},w:{3},h:{4}", Index, mRect.x, mRect.y, mRect.width, mRect.height);
    }


}

/// <summary>
/// 动态无限渲染器
/// @panhaijie
/// </summary>
public class DynamicInfinityItem : MonoBehaviour
{
    public class Event
    {
        public string EventName;
        public object Data;
        public Event(string name,object data)
        {
            EventName = name;
            Data = data;
        }
    }
    public delegate void OnSelect(DynamicInfinityItem item);
    public delegate void OnUpdateData(DynamicInfinityItem item);
    public delegate void OnEvent (Event evt);
    public OnSelect OnSelectHandler;
    public OnUpdateData OnUpdateDataHandler;
    public OnEvent OnEventHandler;
    /// <summary>
    /// 动态矩形
    /// </summary>
    protected DynamicRect mDRect;
    /// <summary>
    /// 动态格子数据
    /// </summary>
    protected object mData;
    public DynamicRect DRect
    {
        set
        {
            mDRect = value;
            //gameObject.SetActive(value != null);
        }
        get { return mDRect; }
    }

    void Start()
    {

    }
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="data"></param>
    public void SetData(object data)
    {
        if (data == null)
        {
            return;
        }

        mData = data;
        if (null != OnUpdateDataHandler)
            OnUpdateDataHandler(this);
        OnRenderer();
    }

    /// <summary>
    /// 复写
    /// </summary>
    protected virtual void OnRenderer()
    {

    }
    /// <summary>
    /// 返回数据
    /// </summary>
    /// <returns></returns>
    public object GetData()
    {
        return mData;
    }

    public T GetData<T>()
    {
        return (T)mData;
    }
}
