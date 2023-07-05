
using Synchronize.Game.Lockstep.Managers.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Managers
{
    class UILayerNode
    {
        public Layer name;
        public Transform transform;
        public LinkedList<UIView> viewerLinkedList;
        public UILayerNode(Layer name,Transform transformRoot)
        {
            this.name = name;
            transform = transformRoot;
            viewerLinkedList = new LinkedList<UIView>();
        }

    }
    public enum Layer
    {
        Bottom,
        Middle,
        Top,
    }
    public class UIModule : MonoBehaviour, IModule
    {
        Dictionary<Layer, UILayerNode> LayerViewDict;
        public void Init()
        {
            LayerViewDict = new Dictionary<Layer, UILayerNode>();
            LayerViewDict[Layer.Bottom] = new UILayerNode(Layer.Bottom, transform.Find("Bottom").transform);
            LayerViewDict[Layer.Middle] = new UILayerNode(Layer.Middle, transform.Find("Middle").transform);
            LayerViewDict[Layer.Top] = new UILayerNode(Layer.Top, transform.Find("Top").transform);
        }
        public void InsertLayerNode(Layer layer, Transform layerTransform)
        {
            LayerViewDict[layer] = new UILayerNode(layer,layerTransform);
        }
        public void Push(string uiPrefabFullName, Layer layer, object data)
        {
            StartCoroutine(PushCor(uiPrefabFullName, layer, data));
        }
        public void PopAll(Layer layer)
        {
            if(LayerViewDict.TryGetValue(layer, out UILayerNode value))
            {
                foreach(var item in value.viewerLinkedList)
                    item.OnClose();
                value.viewerLinkedList.Clear();
            }
        }
        private IEnumerator PushCor(string uiPrefabFullName, Layer layer, object data)
        {
            if (LayerViewDict.TryGetValue(layer, out UILayerNode layerNode))
            {
                foreach(var viewer in layerNode.viewerLinkedList)
                {
                    if(viewer.name == uiPrefabFullName)
                    {
                        if(layerNode.viewerLinkedList.Remove(viewer))
                            _ReusePush(viewer, layer, data);
                        yield break;
                    }
                }

                ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(uiPrefabFullName);
                yield return resourceRequest;
                GameObject uiGo = UnityEngine.Object.Instantiate(resourceRequest.asset, layerNode.transform) as GameObject;
                uiGo.name = uiPrefabFullName;
                UIView uiView = uiGo.GetComponent<UIView>();
                yield return null;
                _Push(uiView, layer, data);
            }
        }
        private void _ReusePush(UIView viewer,Layer layer,object data)
        {
            if (viewer == null) throw new NullReferenceException();
            if (LayerViewDict.TryGetValue(layer, out UILayerNode layerNode))
            {
                if (layerNode.viewerLinkedList.Count > 0)
                {
                    UIView currentView = layerNode.viewerLinkedList.Last.Value;
                    currentView.OnPause();
                }
                layerNode.viewerLinkedList.AddLast(viewer);
                viewer.CurrentLayer = layer;
                viewer.transform.SetAsLastSibling();
                viewer.OnResume();
                viewer.OnShow(data);
            }
        }
        private void _Push(UIView viewer, Layer layer, object data)
        {
            if (viewer == null) throw new System.NullReferenceException();
            if (LayerViewDict.TryGetValue(layer, out UILayerNode layerNode))
            {
                if (layerNode.viewerLinkedList.Count > 0)
                {
                    UIView currentView = layerNode.viewerLinkedList.Last.Value;
                    currentView.OnPause();
                }
                layerNode.viewerLinkedList.AddLast(viewer);
                viewer.CurrentLayer = layer;
                viewer.transform.SetAsLastSibling();
                viewer.OnInit();
                viewer.OnShow(data);
            }
        }

        public bool Pop(Layer layer, UIView view)
        {
            if (view != null && LayerViewDict.TryGetValue(layer, out UILayerNode layerNode))
            {
                if (layerNode.viewerLinkedList.Count > 0)
                {
                    if (layerNode.viewerLinkedList.Last.Value == view)
                    {
                        layerNode.viewerLinkedList.Remove(view);
                        view.OnClose();
                        if (layerNode.viewerLinkedList.Count > 0)
                            layerNode.viewerLinkedList.Last.Value.OnResume();
                    }
                    else
                    {
                        layerNode.viewerLinkedList.Remove(view);
                        view.OnClose();
                    }
                    return true;
                }
            }
            return false;
        }
        public string PeekViewName(Layer layer)
        {
            var view = Peek(layer);
            if (view == null) return null;
            return view.name;
        }
        public UIView Peek(Layer layer)
        {
            if(LayerViewDict.TryGetValue(layer,out UILayerNode layerNode))
            {
                if (layerNode.viewerLinkedList.Count > 0)
                    return layerNode.viewerLinkedList.Last.Value;
                return null;
            }
            return null;
        }
        public int PeekViewAmount(Layer layer)
        {
            if (LayerViewDict.TryGetValue(layer, out UILayerNode layerNode))
                return layerNode.viewerLinkedList.Count;
            return 0;
        }
        public bool Pop(Layer layer)
        {
            if (LayerViewDict.TryGetValue(layer, out UILayerNode layerNode))
            {
                if (layerNode.viewerLinkedList.Count > 0)
                {
                    UIView currentView = layerNode.viewerLinkedList.Last.Value;
                    layerNode.viewerLinkedList.RemoveLast();
                    currentView.OnClose();
                    if (layerNode.viewerLinkedList.Count > 0)
                    {
                        layerNode.viewerLinkedList.Last.Value.OnResume();
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
