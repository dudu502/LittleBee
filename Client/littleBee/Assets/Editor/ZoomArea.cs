using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class ZoomArea
{
    // Global state
    private static Vector2 m_MouseDownPosition = new Vector2(-1000000, -1000000); // in transformed space
    private static int zoomableAreaHash = "ZoomableArea".GetHashCode();

    // Range lock settings
    [SerializeField]
    private bool m_HRangeLocked;
    [SerializeField]
    private bool m_VRangeLocked;
    public bool hRangeLocked { get { return m_HRangeLocked; } set { m_HRangeLocked = value; } }
    public bool vRangeLocked { get { return m_VRangeLocked; } set { m_VRangeLocked = value; } }


    [SerializeField]
    private float m_HBaseRangeMin = 0;
    [SerializeField]
    private float m_HBaseRangeMax = 1;
    [SerializeField]
    private float m_VBaseRangeMin = 0;
    [SerializeField]
    private float m_VBaseRangeMax = 1;
    public float hBaseRangeMin { get { return m_HBaseRangeMin; } set { m_HBaseRangeMin = value; } }
    public float hBaseRangeMax { get { return m_HBaseRangeMax; } set { m_HBaseRangeMax = value; } }
    public float vBaseRangeMin { get { return m_VBaseRangeMin; } set { m_VBaseRangeMin = value; } }
    public float vBaseRangeMax { get { return m_VBaseRangeMax; } set { m_VBaseRangeMax = value; } }
    [SerializeField]
    private bool m_HAllowExceedBaseRangeMin = true;
    [SerializeField]
    private bool m_HAllowExceedBaseRangeMax = true;
    [SerializeField]
    private bool m_VAllowExceedBaseRangeMin = true;
    [SerializeField]
    private bool m_VAllowExceedBaseRangeMax = true;
    public bool hAllowExceedBaseRangeMin { get { return m_HAllowExceedBaseRangeMin; } set { m_HAllowExceedBaseRangeMin = value; } }
    public bool hAllowExceedBaseRangeMax { get { return m_HAllowExceedBaseRangeMax; } set { m_HAllowExceedBaseRangeMax = value; } }
    public bool vAllowExceedBaseRangeMin { get { return m_VAllowExceedBaseRangeMin; } set { m_VAllowExceedBaseRangeMin = value; } }
    public bool vAllowExceedBaseRangeMax { get { return m_VAllowExceedBaseRangeMax; } set { m_VAllowExceedBaseRangeMax = value; } }
    public float hRangeMin
    {
        get { return (hAllowExceedBaseRangeMin ? Mathf.NegativeInfinity : hBaseRangeMin); }
        set { SetAllowExceed(ref m_HBaseRangeMin, ref m_HAllowExceedBaseRangeMin, value); }
    }
    public float hRangeMax
    {
        get { return (hAllowExceedBaseRangeMax ? Mathf.Infinity : hBaseRangeMax); }
        set { SetAllowExceed(ref m_HBaseRangeMax, ref m_HAllowExceedBaseRangeMax, value); }
    }
    public float vRangeMin
    {
        get { return (vAllowExceedBaseRangeMin ? Mathf.NegativeInfinity : vBaseRangeMin); }
        set { SetAllowExceed(ref m_VBaseRangeMin, ref m_VAllowExceedBaseRangeMin, value); }
    }
    public float vRangeMax
    {
        get { return (vAllowExceedBaseRangeMax ? Mathf.Infinity : vBaseRangeMax); }
        set { SetAllowExceed(ref m_VBaseRangeMax, ref m_VAllowExceedBaseRangeMax, value); }
    }
    private void SetAllowExceed(ref float rangeEnd, ref bool allowExceed, float value)
    {
        if (value == Mathf.NegativeInfinity || value == Mathf.Infinity)
        {
            rangeEnd = (value == Mathf.NegativeInfinity ? 0 : 1);
            allowExceed = true;
        }
        else
        {
            rangeEnd = value;
            allowExceed = false;
        }
    }

    private float m_HScaleMin = 0.001f;
    private float m_HScaleMax = 100000.0f;
    private float m_VScaleMin = 0.001f;
    private float m_VScaleMax = 100000.0f;

    // Window resize settings
    [SerializeField]
    private bool m_ScaleWithWindow = false;
    public bool scaleWithWindow { get { return m_ScaleWithWindow; } set { m_ScaleWithWindow = value; } }

    // Slider settings
    [SerializeField]
    private bool m_HSlider = true;
    [SerializeField]
    private bool m_VSlider = true;
    public bool hSlider { get { return m_HSlider; } set { Rect r = rect; m_HSlider = value; rect = r; } }
    public bool vSlider { get { return m_VSlider; } set { Rect r = rect; m_VSlider = value; rect = r; } }

    [SerializeField]
    private bool m_IgnoreScrollWheelUntilClicked = false;
    public bool ignoreScrollWheelUntilClicked { get { return m_IgnoreScrollWheelUntilClicked; } set { m_IgnoreScrollWheelUntilClicked = value; } }

    public bool m_UniformScale;
    public bool uniformScale { get { return m_UniformScale; } set { m_UniformScale = value; } }

    // View and drawing settings
    [SerializeField]
    private Rect m_DrawArea = new Rect(0, 0, 100, 100);
    internal void SetDrawRectHack(Rect r) { m_DrawArea = r; }
    [SerializeField]
    internal Vector2 m_Scale = new Vector2(1, -1);
    [SerializeField]
    internal Vector2 m_Translation = new Vector2(0, 0);
    [SerializeField]
    private float m_MarginLeft, m_MarginRight, m_MarginTop, m_MarginBottom;
    [SerializeField]
    private Rect m_LastShownAreaInsideMargins = new Rect(0, 0, 100, 100);

    public Vector2 scale { get { return m_Scale; } }
    public float margin { set { m_MarginLeft = m_MarginRight = m_MarginTop = m_MarginBottom = value; } }
    public float leftmargin { get { return m_MarginLeft; } set { m_MarginLeft = value; } }
    public float rightmargin { get { return m_MarginRight; } set { m_MarginRight = value; } }
    public float topmargin { get { return m_MarginTop; } set { m_MarginTop = value; } }
    public float bottommargin { get { return m_MarginBottom; } set { m_MarginBottom = value; } }

    [SerializeField]
    bool m_MinimalGUI;

    [System.Serializable]
    public class Styles
    {
#if UNITY_5_5_OR_NEWER
        public GUIStyle background = "AnimationKeyframeBackground";
#else
        public GUIStyle background = "AnimationCurveEditorBackground";
#endif
        public GUIStyle horizontalScrollbar;
        public GUIStyle horizontalMinMaxScrollbarThumb;
        public GUIStyle horizontalScrollbarLeftButton;
        public GUIStyle horizontalScrollbarRightButton;
        public GUIStyle verticalScrollbar;
        public GUIStyle verticalMinMaxScrollbarThumb;
        public GUIStyle verticalScrollbarUpButton;
        public GUIStyle verticalScrollbarDownButton;

        public float sliderWidth;
        public float visualSliderWidth;
        public Styles(bool minimalGUI)
        {
            if (minimalGUI)
            {
                visualSliderWidth = 0;
                sliderWidth = 15;
            }
            else
            {
                visualSliderWidth = 15;
                sliderWidth = 15;
            }
        }

        public void InitGUIStyles(bool minimalGUI)
        {
            if (minimalGUI)
            {
                horizontalMinMaxScrollbarThumb = "MiniMinMaxSliderHorizontal";
                horizontalScrollbarLeftButton = GUIStyle.none;
                horizontalScrollbarRightButton = GUIStyle.none;
                horizontalScrollbar = GUIStyle.none;
                verticalMinMaxScrollbarThumb = "MiniMinMaxSlidervertical";
                verticalScrollbarUpButton = GUIStyle.none;
                verticalScrollbarDownButton = GUIStyle.none;
                verticalScrollbar = GUIStyle.none;
            }
            else
            {
                horizontalMinMaxScrollbarThumb = "horizontalMinMaxScrollbarThumb";
                horizontalScrollbarLeftButton = "horizontalScrollbarLeftbutton";
                horizontalScrollbarRightButton = "horizontalScrollbarRightbutton";
                horizontalScrollbar = GUI.skin.horizontalScrollbar;
                verticalMinMaxScrollbarThumb = "verticalMinMaxScrollbarThumb";
                verticalScrollbarUpButton = "verticalScrollbarUpbutton";
                verticalScrollbarDownButton = "verticalScrollbarDownbutton";
                verticalScrollbar = GUI.skin.verticalScrollbar;
            }
        }
    }

    private Styles m_Styles;
    private Styles styles
    {
        get
        {
            if (m_Styles == null)
                m_Styles = new Styles(m_MinimalGUI);
            return m_Styles;
        }
    }

    public Rect rect
    {
        get { return new Rect(drawRect.x, drawRect.y, drawRect.width + (m_VSlider ? styles.visualSliderWidth : 0), drawRect.height + (m_HSlider ? styles.visualSliderWidth : 0)); }
        set
        {
            Rect newDrawArea = new Rect(value.x, value.y, value.width - (m_VSlider ? styles.visualSliderWidth : 0), value.height - (m_HSlider ? styles.visualSliderWidth : 0));
            if (newDrawArea != m_DrawArea)
            {
                if (m_ScaleWithWindow)
                {
                    m_DrawArea = newDrawArea;
                    shownAreaInsideMargins = m_LastShownAreaInsideMargins;
                }
                else
                {
                    m_Translation += new Vector2((newDrawArea.width - m_DrawArea.width) / 2, (newDrawArea.height - m_DrawArea.height) / 2);
                    m_DrawArea = newDrawArea;
                }
            }
            EnforceScaleAndRange();
        }
    }
    public Rect drawRect { get { return m_DrawArea; } }

    public void SetShownHRangeInsideMargins(float min, float max)
    {
        m_Scale.x = (drawRect.width - leftmargin - rightmargin) / (max - min);
        m_Translation.x = -min * m_Scale.x + leftmargin;
        EnforceScaleAndRange();
    }

    public void SetShownHRange(float min, float max)
    {
        m_Scale.x = drawRect.width / (max - min);
        m_Translation.x = -min * m_Scale.x;
        EnforceScaleAndRange();
    }

    public void SetShownVRangeInsideMargins(float min, float max)
    {
        m_Scale.y = -(drawRect.height - topmargin - bottommargin) / (max - min);
        m_Translation.y = drawRect.height - min * m_Scale.y - topmargin;
        EnforceScaleAndRange();
    }

    public void SetShownVRange(float min, float max)
    {
        m_Scale.y = -drawRect.height / (max - min);
        m_Translation.y = drawRect.height - min * m_Scale.y;
        EnforceScaleAndRange();
    }

    // ShownArea is in curve space
    public Rect shownArea
    {
        set
        {
            m_Scale.x = drawRect.width / value.width;
            m_Scale.y = -drawRect.height / value.height;
            m_Translation.x = -value.x * m_Scale.x;
            m_Translation.y = drawRect.height - value.y * m_Scale.y;
            EnforceScaleAndRange();
        }
        get
        {
            return new Rect(
                -m_Translation.x / m_Scale.x,
                -(m_Translation.y - drawRect.height) / m_Scale.y,
                drawRect.width / m_Scale.x,
                drawRect.height / -m_Scale.y
                );
        }
    }

    public Rect shownAreaInsideMargins
    {
        set
        {
            shownAreaInsideMarginsInternal = value;
            EnforceScaleAndRange();
        }
        get
        {
            return shownAreaInsideMarginsInternal;
        }
    }

    private Rect shownAreaInsideMarginsInternal
    {
        set
        {
            m_Scale.x = (drawRect.width - leftmargin - rightmargin) / value.width;
            m_Scale.y = -(drawRect.height - topmargin - bottommargin) / value.height;
            m_Translation.x = -value.x * m_Scale.x + leftmargin;
            m_Translation.y = drawRect.height - value.y * m_Scale.y - topmargin;
        }
        get
        {
            float leftmarginRel = leftmargin / m_Scale.x;
            float rightmarginRel = rightmargin / m_Scale.x;
            float topmarginRel = topmargin / m_Scale.y;
            float bottommarginRel = bottommargin / m_Scale.y;

            Rect area = shownArea;
            area.x += leftmarginRel;
            area.y -= topmarginRel;
            area.width -= leftmarginRel + rightmarginRel;
            area.height += topmarginRel + bottommarginRel;
            return area;
        }
    }

    public virtual Bounds drawingBounds
    {
        get
        {
            return new Bounds(
                new Vector3((hBaseRangeMin + hBaseRangeMax) * 0.5f, (vBaseRangeMin + vBaseRangeMax) * 0.5f, 0),
                new Vector3(hBaseRangeMax - hBaseRangeMin, vBaseRangeMax - vBaseRangeMin, 1)
                );
        }
    }


    // Utility transform functions

    public Matrix4x4 drawingToViewMatrix
    {
        get
        {
            return Matrix4x4.TRS(m_Translation, Quaternion.identity, new Vector3(m_Scale.x, m_Scale.y, 1));
        }
    }

    public Vector2 DrawingToViewTransformPoint(Vector2 lhs)
    { return new Vector2(lhs.x * m_Scale.x + m_Translation.x, lhs.y * m_Scale.y + m_Translation.y); }
    public Vector3 DrawingToViewTransformPoint(Vector3 lhs)
    { return new Vector3(lhs.x * m_Scale.x + m_Translation.x, lhs.y * m_Scale.y + m_Translation.y, 0); }

    public Vector2 ViewToDrawingTransformPoint(Vector2 lhs)
    { return new Vector2((lhs.x - m_Translation.x) / m_Scale.x, (lhs.y - m_Translation.y) / m_Scale.y); }
    public Vector3 ViewToDrawingTransformPoint(Vector3 lhs)
    { return new Vector3((lhs.x - m_Translation.x) / m_Scale.x, (lhs.y - m_Translation.y) / m_Scale.y, 0); }

    public Vector2 DrawingToViewTransformVector(Vector2 lhs)
    { return new Vector2(lhs.x * m_Scale.x, lhs.y * m_Scale.y); }
    public Vector3 DrawingToViewTransformVector(Vector3 lhs)
    { return new Vector3(lhs.x * m_Scale.x, lhs.y * m_Scale.y, 0); }

    public Vector2 ViewToDrawingTransformVector(Vector2 lhs)
    { return new Vector2(lhs.x / m_Scale.x, lhs.y / m_Scale.y); }
    public Vector3 ViewToDrawingTransformVector(Vector3 lhs)
    { return new Vector3(lhs.x / m_Scale.x, lhs.y / m_Scale.y, 0); }

    public Vector2 mousePositionInDrawing
    {
        get { return ViewToDrawingTransformPoint(Event.current.mousePosition); }
    }

    public Vector2 NormalizeInViewSpace(Vector2 vec)
    {
        vec = Vector2.Scale(vec, m_Scale);
        vec /= vec.magnitude;
        return Vector2.Scale(vec, new Vector2(1 / m_Scale.x, 1 / m_Scale.y));
    }

    // Utility mouse event functions

    private bool IsZoomEvent()
    {
        return (
            (Event.current.button == 1 && Event.current.alt) // right+alt drag
            //|| (Event.current.button == 0 && Event.current.command) // left+commend drag
            //|| (Event.current.button == 2 && Event.current.command) // middle+command drag

            );
    }

    private bool IsPanEvent()
    {
        return (
            (Event.current.button == 0 && Event.current.alt) // left+alt drag
            || (Event.current.button == 2 && !Event.current.command) // middle drag
            );
    }

    public ZoomArea()
    {
        m_MinimalGUI = false;
    }

    public ZoomArea(bool minimalGUI)
    {
        m_MinimalGUI = minimalGUI;
    }

    public void BeginViewGUI()
    {
        if (styles.horizontalScrollbar == null)
            styles.InitGUIStyles(m_MinimalGUI);

        GUILayout.BeginArea(m_DrawArea, styles.background);
        HandleZoomAndPanEvents(m_DrawArea);
        GUILayout.EndArea();
    }

    public void HandleZoomAndPanEvents(Rect area)
    {
        area.x = 0;
        area.y = 0;
        int id = GUIUtility.GetControlID(zoomableAreaHash, FocusType.Passive, area);

        switch (Event.current.GetTypeForControl(id))
        {
            case EventType.MouseDown:
                if (area.Contains(Event.current.mousePosition))
                {
                    // Catch keyboard control when clicked inside zoomable area
                    // (used to restrict scrollwheel)
                    GUIUtility.keyboardControl = id;

                    if (IsZoomEvent() || IsPanEvent())
                    {
                        GUIUtility.hotControl = id;
                        m_MouseDownPosition = mousePositionInDrawing;

                        Event.current.Use();
                    }
                }
                break;
            case EventType.MouseUp:
                //Debug.Log("mouse-up!");
                if (GUIUtility.hotControl == id)
                {
                    GUIUtility.hotControl = 0;

                    // If we got the mousedown, the mouseup is ours as well
                    // (no matter if the click was in the area or not)
                    m_MouseDownPosition = new Vector2(-1000000, -1000000);
                    //Event.current.Use();
                }
                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl != id) break;

                if (IsZoomEvent())
                {
                    // Zoom in around mouse down position
                    Zoom(m_MouseDownPosition, false);
                    Event.current.Use();
                }
                else if (IsPanEvent())
                {
                    // Pan view
                    Pan();
                    Event.current.Use();
                }
                break;
            case EventType.ScrollWheel:
                if (!area.Contains(Event.current.mousePosition))
                    break;
                if (m_IgnoreScrollWheelUntilClicked && GUIUtility.keyboardControl != id)
                    break;

                // Zoom in around cursor position
                Zoom(mousePositionInDrawing, true);
                Event.current.Use();
                break;
        }
    }

    public void EndViewGUI()
    {
    }

    private void Pan()
    {
        if (!m_HRangeLocked)
            m_Translation.x += Event.current.delta.x;
        if (!m_VRangeLocked)
            m_Translation.y += Event.current.delta.y;

        EnforceScaleAndRange();
    }

    private void Zoom(Vector2 zoomAround, bool scrollwhell)
    {
        // Get delta (from scroll wheel or mouse pad)
        // Add x and y delta to cover all cases
        // (scrool view has only y or only x when shift is pressed,
        // while mouse pad has both x and y at all times)
        float delta = Event.current.delta.x + Event.current.delta.y;

        if (scrollwhell)
            delta = -delta;

        // Scale multiplier. Don't allow scale of zero or below!
        float scale = Mathf.Max(0.01F, 1 + delta * 0.01F);

        if (!m_HRangeLocked && !Event.current.shift)
        {
            // Offset to make zoom centered around cursor position
            m_Translation.x -= zoomAround.x * (scale - 1) * m_Scale.x;

            // Apply zooming
            m_Scale.x *= scale;
        }
        if (!m_VRangeLocked && !EditorGUI.actionKey)
        {
            // Offset to make zoom centered around cursor position
            m_Translation.y -= zoomAround.y * (scale - 1) * m_Scale.y;

            // Apply zooming
            m_Scale.y *= scale;
        }

        EnforceScaleAndRange();
    }

    public void EnforceScaleAndRange()
    {
        float hScaleMin = m_HScaleMin;
        float vScaleMin = m_VScaleMin;
        float hScaleMax = m_HScaleMax;
        float vScaleMax = m_VScaleMax;
        if (hRangeMax != Mathf.Infinity && hRangeMin != Mathf.NegativeInfinity)
            hScaleMax = Mathf.Min(m_HScaleMax, hRangeMax - hRangeMin);
        if (vRangeMax != Mathf.Infinity && vRangeMin != Mathf.NegativeInfinity)
            vScaleMax = Mathf.Min(m_VScaleMax, vRangeMax - vRangeMin);

        Rect oldArea = m_LastShownAreaInsideMargins;
        Rect newArea = shownAreaInsideMargins;
        if (newArea == oldArea)
            return;

        float epsilon = 0.00001f;

        if (newArea.width < oldArea.width - epsilon)
        {
            float xLerp = Mathf.InverseLerp(oldArea.width, newArea.width, hScaleMin);
            newArea = new Rect(
                    Mathf.Lerp(oldArea.x, newArea.x, xLerp),
                    newArea.y,
                    Mathf.Lerp(oldArea.width, newArea.width, xLerp),
                    newArea.height
                    );
        }
        if (newArea.height < oldArea.height - epsilon)
        {
            float yLerp = Mathf.InverseLerp(oldArea.height, newArea.height, vScaleMin);
            newArea = new Rect(
                    newArea.x,
                    Mathf.Lerp(oldArea.y, newArea.y, yLerp),
                    newArea.width,
                    Mathf.Lerp(oldArea.height, newArea.height, yLerp)
                    );
        }
        if (newArea.width > oldArea.width + epsilon)
        {
            float xLerp = Mathf.InverseLerp(oldArea.width, newArea.width, hScaleMax);
            newArea = new Rect(
                    Mathf.Lerp(oldArea.x, newArea.x, xLerp),
                    newArea.y,
                    Mathf.Lerp(oldArea.width, newArea.width, xLerp),
                    newArea.height
                    );
        }
        if (newArea.height > oldArea.height + epsilon)
        {
            float yLerp = Mathf.InverseLerp(oldArea.height, newArea.height, vScaleMax);
            newArea = new Rect(
                    newArea.x,
                    Mathf.Lerp(oldArea.y, newArea.y, yLerp),
                    newArea.width,
                    Mathf.Lerp(oldArea.height, newArea.height, yLerp)
                    );
        }

        // Enforce ranges
        if (newArea.xMin < hRangeMin)
            newArea.x = hRangeMin;
        if (newArea.xMax > hRangeMax)
            newArea.x = hRangeMax - newArea.width;
        if (newArea.yMin < vRangeMin)
            newArea.y = vRangeMin;
        if (newArea.yMax > vRangeMax)
            newArea.y = vRangeMax - newArea.height;

        shownAreaInsideMarginsInternal = newArea;
        m_LastShownAreaInsideMargins = newArea;
    }

    public float PixelToTime(float pixelX, Rect rect)
    {
        return ((pixelX - rect.x) * shownArea.width / rect.width + shownArea.x);
    }

    public float TimeToPixel(float time, Rect rect)
    {
        return (time - shownArea.x) / shownArea.width * rect.width + rect.x;
    }

    public float PixelDeltaToTime(Rect rect)
    {
        return shownArea.width / rect.width;
    }
}
