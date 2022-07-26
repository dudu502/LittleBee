using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using System.Collections.Generic;
using Treemap;
using UnityEditor;
using Assets.Editor.Treemap;
using System.Linq;
using UnityEngine.UI;

namespace MemoryProfilerWindow
{
    public class TreeMapView
    {
        CrawledMemorySnapshot _unpackedCrawl;
        private ZoomArea _ZoomArea;
        private Dictionary<string, Group> _groups = new Dictionary<string, Group>();
        private List<Item> _items = new List<Item>();
        private List<Mesh> _cachedMeshes = new List<Mesh>();
        private Item _selectedItem;
        private Group _selectedGroup;
        private Item _mouseDownItem;

        MemoryProfilerWindow _hostWindow;

        private Vector2 mouseTreemapPosition { get { return _ZoomArea.ViewToDrawingTransformPoint(Event.current.mousePosition); } }

        public void Setup(MemoryProfilerWindow hostWindow, CrawledMemorySnapshot _unpackedCrawl)
        {
            this._unpackedCrawl = _unpackedCrawl;
            this._hostWindow = hostWindow;

            _ZoomArea = new ZoomArea(true)
            {
                vRangeMin = -110f,
                vRangeMax = 110f,
                hRangeMin = -110f,
                hRangeMax = 110f,
                hBaseRangeMin = -110f,
                vBaseRangeMin = -110f,
                hBaseRangeMax = 110f,
                vBaseRangeMax = 110f,
                shownArea = new Rect(-110f, -110f, 220f, 220f)
            };
            RefreshCaches();
            RefreshMesh();
        }

        public void Draw()
        {
            if (_hostWindow == null)
                return;
            
            Rect r = new Rect(0f, 25f, _hostWindow.position.width - _hostWindow._inspector.width, _hostWindow.position.height - 25f);

            _ZoomArea.rect = r;
            _ZoomArea.BeginViewGUI();

            GUI.BeginGroup(r);
            Handles.matrix = _ZoomArea.drawingToViewMatrix;
            HandleMouseClick();
            RenderTreemap();
            GUI.EndGroup();

            _ZoomArea.EndViewGUI();
        }

        private void OnHoveredGroupChanged()
        {
            RefreshCachedRects(false);
        }

        private void HandleMouseClick()
        {
            if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp) && Event.current.button == 0)
            {
                if (_ZoomArea.drawRect.Contains(Event.current.mousePosition))
                {
                    Group group = _groups.Values.FirstOrDefault(i => i._position.Contains(mouseTreemapPosition));
                    Item item = _items.FirstOrDefault(i => i._position.Contains(mouseTreemapPosition));

                    if (item != null && _selectedGroup == item._group)
                    {
                        switch (Event.current.type)
                        {
                            case EventType.MouseDown:
                                _mouseDownItem = item;
                                break;

                            case EventType.MouseUp:
                                if (_mouseDownItem == item)
                                {
                                    _hostWindow.SelectThing(item._thingInMemory);
                                    Event.current.Use();
                                }
                                break;
                        }
                    }
                    else if (group != null)
                    {
                        switch (Event.current.type)
                        {
                            case EventType.MouseUp:
                                _hostWindow.SelectGroup(group);
                                Event.current.Use();
                                break;
                        }
                    }
                }
            }
        }

        public void SelectThing(ThingInMemory thing)
        {
            _selectedItem = _items.First(i => i._thingInMemory == thing);
            _selectedGroup = _selectedItem._group;
            RefreshCachedRects(false);
        }

        public void SelectGroup(Group group)
        {
            _selectedItem = null;
            _selectedGroup = group;
            RefreshCachedRects(false);
        }

        void RefreshCaches()
        {
            _items.Clear();
            _groups.Clear();

            foreach (ThingInMemory thingInMemory in _unpackedCrawl.allObjects)
            {
                string groupName = GetGroupName(thingInMemory);
                if (groupName.Length == 0)
                    continue;

                if (!_groups.ContainsKey(groupName))
                {
                    Group newGroup = new Group();
                    newGroup._name = groupName;
                    newGroup._items = new List<Item>();
                    _groups.Add(groupName, newGroup);
                }

                Item item = new Item(thingInMemory, _groups[groupName]);
                _items.Add(item);
                _groups[groupName]._items.Add(item);
            }

            foreach (Group group in _groups.Values)
            {
                group._items.Sort();
            }

            _items.Sort();
            RefreshCachedRects(true);
        }

        private void RefreshCachedRects(bool fullRefresh)
        {
            Rect space = new Rect(-100f, -100f, 200f, 200f);

            if (fullRefresh)
            {
                List<Group> groups = _groups.Values.ToList();
                groups.Sort();
                float[] groupTotalValues = new float[groups.Count];
                for (int i = 0; i < groups.Count; i++)
                {
                    groupTotalValues[i] = groups.ElementAt(i).totalMemorySize;
                }

                Rect[] groupRects = Utility.GetTreemapRects(groupTotalValues, space);
                for (int groupIndex = 0; groupIndex < groupRects.Length; groupIndex++)
                {
                    Group group = groups[groupIndex];
                    group._position = groupRects[groupIndex];
                }
            }

            if (_selectedGroup != null)
            {
                Rect[] rects = Utility.GetTreemapRects(_selectedGroup.memorySizes, _selectedGroup._position);

                for (int i = 0; i < rects.Length; i++)
                {
                    _selectedGroup._items[i]._position = rects[i];
                }
            }

            RefreshMesh();
        }

        public void CleanupMeshes ()
        {
            if (_cachedMeshes == null) {
                _cachedMeshes = new List<Mesh> ();
            }
            else {
                for (int i = 0; i < _cachedMeshes.Count; i++) {
                    UnityEngine.Object.DestroyImmediate (_cachedMeshes [i]);
                }
               
                _cachedMeshes.Clear ();
            }
        }

        private void RefreshMesh()
        {
            CleanupMeshes ();

            const int maxVerts = 32000;
            Vector3[] vertices = new Vector3[maxVerts];
            Color[] colors = new Color[maxVerts];
            int[] triangles = new int[maxVerts * 6 / 4];

            int meshItemIndex = 0;
            int totalItemIndex = 0;

            List<ITreemapRenderable> visible = new List<ITreemapRenderable>();
            foreach (Group group in _groups.Values)
            {
                if (group != _selectedGroup)
                {
                    visible.Add(group);
                }
                else
                {
                    foreach (Item item in group._items)
                    {
                        visible.Add(item);
                    }
                }
            }

            foreach (ITreemapRenderable item in visible)
            {
                int index = meshItemIndex * 4;
                vertices[index++] = new Vector3(item.GetPosition().xMin, item.GetPosition().yMin, 0f);
                vertices[index++] = new Vector3(item.GetPosition().xMax, item.GetPosition().yMin, 0f);
                vertices[index++] = new Vector3(item.GetPosition().xMax, item.GetPosition().yMax, 0f);
                vertices[index++] = new Vector3(item.GetPosition().xMin, item.GetPosition().yMax, 0f);

                index = meshItemIndex * 4;
                var color = item.GetColor();
                if (item == _selectedItem)
                    color *= 1.5f;

                colors[index++] = color;
                colors[index++] = color * 0.75f;
                colors[index++] = color * 0.5f;
                colors[index++] = color * 0.75f;

                index = meshItemIndex * 6;
                triangles[index++] = meshItemIndex * 4 + 0;
                triangles[index++] = meshItemIndex * 4 + 1;
                triangles[index++] = meshItemIndex * 4 + 3;
                triangles[index++] = meshItemIndex * 4 + 1;
                triangles[index++] = meshItemIndex * 4 + 2;
                triangles[index++] = meshItemIndex * 4 + 3;

                meshItemIndex++;
                totalItemIndex++;

                if (meshItemIndex >= maxVerts / 4 || totalItemIndex == visible.Count)
                {
                    Mesh mesh = new Mesh();
                    mesh.hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.NotEditable;
                    mesh.vertices = vertices;
                    mesh.triangles = triangles;
                    mesh.colors = colors;
                    _cachedMeshes.Add(mesh);

                    vertices = new Vector3[maxVerts];
                    colors = new Color[maxVerts];
                    triangles = new int[maxVerts * 6 / 4];
                    meshItemIndex = 0;
                }
            }
        }

        public void RenderTreemap()
        {
            if (_cachedMeshes == null)
                return;

            Material mat = (Material)EditorGUIUtility.LoadRequired("SceneView/2DHandleLines.mat");
            mat.SetPass(0);

            for (int i = 0; i < _cachedMeshes.Count; i++)
            {
                Graphics.DrawMeshNow(_cachedMeshes[i], Handles.matrix);
            }
            RenderLabels();
        }

        private void RenderLabels()
        {
            if (_groups == null)
                return;

            GUI.color = Color.black;

            foreach (Group group in _groups.Values)
            {
                if (Utility.IsInside(group._position, _ZoomArea.shownArea))
                {
                    if (_selectedItem != null && _selectedItem._group == group)
                    {
                        RenderGroupItems(group);
                    }
                    else
                    {
                        RenderGroupLabel(group);
                    }
                }
            }

            GUI.color = Color.white;
        }

        private void RenderGroupLabel(Group group)
        {
            Matrix4x4 mat = _ZoomArea.drawingToViewMatrix;

            Vector3 p1 = mat.MultiplyPoint(new Vector3(group._position.xMin, group._position.yMin));
            Vector3 p2 = mat.MultiplyPoint(new Vector3(group._position.xMax, group._position.yMax));

            if (p2.x - p1.x > 30f)
            {
                Rect rect = new Rect(p1.x, p2.y, p2.x - p1.x, p1.y - p2.y);
                GUI.Label(rect, group.GetLabel());
            }
        }

        private void RenderGroupItems(Group group)
        {
            Matrix4x4 mat = _ZoomArea.drawingToViewMatrix;

            foreach (Item item in group._items)
            {
                if (Utility.IsInside(item._position, _ZoomArea.shownArea))
                {
                    Vector3 p1 = mat.MultiplyPoint(new Vector3(item._position.xMin, item._position.yMin));
                    Vector3 p2 = mat.MultiplyPoint(new Vector3(item._position.xMax, item._position.yMax));

                    if (p2.x - p1.x > 30f)
                    {
                        Rect rect = new Rect(p1.x, p2.y, p2.x - p1.x, p1.y - p2.y);
                        string row1 = item._group._name;
                        string row2 = EditorUtility.FormatBytes(item.memorySize);
                        GUI.Label(rect, row1 + "\n" + row2);
                    }
                }
            }
        }

        public string GetGroupName(ThingInMemory thing)
        {
			if (thing is NativeUnityEngineObject)
				return (thing as NativeUnityEngineObject).className ?? "MissingName";
            if (thing is ManagedObject)
                return (thing as ManagedObject).typeDescription.name;
            return thing.GetType().Name;
        }
    }
}
